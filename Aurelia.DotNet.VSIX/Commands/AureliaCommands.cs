namespace Aurelia.DotNet.VSIX.Commands
{
    using System;

    /// <summary>
    /// Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string guidAureliaCommandSetString = "115E73A4-99E8-49D0-8867-360FCED1B27E";
        public const string guidAureliaContextString = "9AAB4142-64D7-45A5-87DF-200680CBFF35";

        public static Guid guidAureliaCommandsPackage = new Guid(AVSIXPackage.PackageGuidString);
        public static Guid guidAureliaCommandsSet = new Guid(guidAureliaCommandSetString);
        public static Guid guidAureliaContext = new Guid(guidAureliaContextString);
    }
    /// <summary>
    /// Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int MyMenu = 0x1010;
        public const int MyMenuGroup = 0x1020;
        public const int CommandGroup = 0x2000;

        public const int cmdGenerateElement = 0x0100;
        public const int cmdUpdateAurelia = 0x0150;
    }
}