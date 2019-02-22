using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.NodeServices.Util;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.Extensions.Util;
using Microsoft.AspNetCore.SpaServices.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Extensions
{
    internal static class AureliaCliDevelopmentServer
    {
        private const string LogCategoryName = "Aurelia.Tools.DotNet.Extensions";
        private const string AureliaJsonFile = "aurelia_project/aurelia.json";
        private static TimeSpan RegexMatchTimeout = TimeSpan.FromMinutes(1); // This is a development-time only feature, so a very long timeout is fine


        private static async Task<int> FindTcpPortAsync(string sourcePath)
        {

            int FirstOpenPort()
            {
                var listener = new TcpListener(IPAddress.Loopback, 0);
                listener.Start();
                try
                {
                    return ((IPEndPoint)listener.LocalEndpoint).Port;
                }
                finally
                {
                    listener.Stop();
                }
            }

            using (var file = File.Open(Path.Combine(sourcePath, AureliaJsonFile), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(file))
            {
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    if (line.Contains("port"))
                    {
                        var port = line.Replace(",", "").Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (port.Length == 2 && int.TryParse(port[1], out var portNumber))
                        {
                            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                            var tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections().ToList();
                            if (!tcpConnInfoArray.Any(y => y.LocalEndPoint.Port == portNumber)) { return portNumber; }
                        }
                        // meh found the port section but broke
                        return FirstOpenPort();
                    }
                }
            }

            //didn't find squat
            return FirstOpenPort();


        }

        public static void Attach(ISpaBuilder spaBuilder, string nodeScriptName, PackageManager packageManager, bool hotModuleReload)
        {
            var sourcePath = spaBuilder.Options.SourcePath;
            if (string.IsNullOrEmpty(sourcePath))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(sourcePath));
            }

            if (string.IsNullOrEmpty(nodeScriptName))
            {
                throw new ArgumentException("Cannot be null or empty", nameof(nodeScriptName));
            }

            // Start aurelia cli server and attach to middleware pipeline
            var appBuilder = spaBuilder.ApplicationBuilder;
            var logger = LoggerFinder.GetOrCreateLogger(appBuilder, LogCategoryName);
            var portTask = StartCreateAureliaCliAppServerAsync(sourcePath, nodeScriptName, logger, packageManager, hotModuleReload);

            // Everything we proxy is hardcoded to target http://localhost because:
            // - the requests are always from the local machine (we're not accepting remote
            //   requests that go directly to the vue-cli-service server)
            // - given that, there's no reason to use https, and we couldn't even if we
            //   wanted to, because in general the vue-cli-service server has no certificate
            var targetUriTask = portTask.ContinueWith(
                task => new UriBuilder("http", "localhost", task.Result).Uri);

            SpaProxyingExtensions.UseProxyToSpaDevelopmentServer(spaBuilder, () =>
            {
                // On each request, we create a separate startup task with its own timeout. That way, even if
                // the first request times out, subsequent requests could still work.
                var timeout = spaBuilder.Options.StartupTimeout;
                return targetUriTask.WithTimeout(timeout,
                    $"The aurelia cli server did not start listening for requests " +
                    $"within the timeout period of {timeout.Seconds} seconds. " +
                    $"Check the log output for error information.");
            });
        }

        private static async Task<int> StartCreateAureliaCliAppServerAsync(
            string sourcePath, string nodeScriptName, ILogger logger, PackageManager packageManager, bool hotModuleReload)
        {
            var portNumber = await FindTcpPortAsync(sourcePath);
            logger.LogInformation($"Starting aurelia cli server on port {portNumber}...");

            var envVars = new Dictionary<string, string> { };
            var nodeScriptRunner = new ScriptRunner(sourcePath, nodeScriptName, hotModuleReload ? "--hmr" : string.Empty, envVars, packageManager);

            using (var stdErrReader = new EventedStreamStringReader(nodeScriptRunner.StdErr))
            {
                try
                {
                    // Although the dev server may eventually tell us the URL it's listening on,
                    // it doesn't do so until it's finished compiling, and even then only if there were
                    // no compiler warnings. So instead of waiting for that, consider it ready as soon
                    // as it starts listening for requests.
                    await nodeScriptRunner.StdOut.WaitForMatch(new Regex("Project is running at", RegexOptions.None, RegexMatchTimeout));
                }
                catch (EndOfStreamException ex)
                {
                    throw new InvalidOperationException(
                        $"The node script '{nodeScriptName}' exited without indicating that the " +
                        $"node server was listening for requests. The error output was: " +
                        $"{stdErrReader.ReadAsString()}", ex);
                }
            }

            return portNumber;
        }
    }
}
