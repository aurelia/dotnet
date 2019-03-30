using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Aurelia.DotNet.Spa.Hubs
{
    public class SampleHub : Hub
    {
        public Task Send()
        {
            return Clients.All.SendAsync("current-date", new DateTime());
        }
    }
}
