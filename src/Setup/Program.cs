// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using WixSharp;
using Assembly = System.Reflection.Assembly;
using File = WixSharp.File;

namespace WixSharpSetup
{
    internal class Program
    {
#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif

        private const string AppIcon = @"..\..\images\nexus.ico";
        private const string AppName = @"ParkitectNexus Client";
        private const string AppExecutable = @"ParkitectNexus Client.exe";
        private const string AppBinariesPath = @"..\..\bin\" + Configuration + @"\";

        private static void Main()
        {
            // Get version of the client.
            var assembly = Assembly.LoadFrom(AppBinariesPath + AppExecutable);
            var assemblyName = assembly.GetName();
            var version = assemblyName.Version;
            var guid = new Guid(((GuidAttribute) assembly.GetCustomAttributes(typeof (GuidAttribute), true)[0]).Value);

            //Create the installer project.
            var project = new ManagedProject(AppName,
                new Dir(new Id("INSTALL_DIR"), @"%ProgramFiles%\" + AppName,
                    new File(AppExecutable),
                    new File(@"CommandLine.dll"),
                    new File(@"Newtonsoft.Json.dll"),
                    new File(@"Octokit.dll"),
                    new File(@"ParkitectNexus.AssetMagic.dll"),
                    new File(@"ParkitectNexus.Client.Base.dll"),
                    new File(@"ParkitectNexus.Data.dll"),
                    new File(@"ParkitectNexus.Mod.ModLoader.dll"),
                    new File(@"StructureMap.dll"),
                    new File(@"StructureMap.Net4.dll"),
                    new File(@"Xwt.dll"),
                    new File(@"Xwt.WPF.dll")
                    ),
                new Dir(@"%ProgramMenu%\" + AppName,
                    new ExeFileShortcut(AppName, $"[INSTALL_DIR]{AppExecutable}", ""),
                    new ExeFileShortcut("Launch Parkitect with Mods", $"[INSTALL_DIR]{AppExecutable}", "--launch"),
                    new ExeFileShortcut("Uninstall " + AppName, "[System64Folder]msiexec.exe", "/x [ProductCode]")
                    ),
                new Dir(@"%Desktop%",
                    new ExeFileShortcut(AppName, $"[INSTALL_DIR]{AppExecutable}", "")
                    ),
                new InstalledFileAction(AppExecutable, "--silent")
                )
            {
                GUID = guid,
                UI = WUI.WixUI_InstallDir,
                SourceBaseDir = AppBinariesPath,
                OutFileName =
                    "parkitectnexus-client" +
                    (Configuration != "Release" ? "-" + Configuration.ToLower() : string.Empty) + "-" + version,
                OutDir = @"..\..\bin",
                Version = version,
                Description = "An installer for Parkitect assets.",
                MajorUpgradeStrategy = MajorUpgradeStrategy.Default,
                LicenceFile = Directory.GetCurrentDirectory() + @"\..\..\tos.rtf",
                BannerImage = Directory.GetCurrentDirectory() + @"\..\..\images\dialog_banner.png",
                BackgroundImage = Directory.GetCurrentDirectory() + @"\..\..\images\dialog_bg.png",
                ControlPanelInfo =
                {
                    NoRepair = true,
                    HelpLink = "https://parkitectnexus.com",
                    ProductIcon = AppIcon,
                    Manufacturer = "ParkitectNexus, Tim Potze"
                }
            };

            // Set message to indicate a newer version has already been installed.
            project.MajorUpgradeStrategy.NewerProductInstalledErrorMessage = "A newer version of " + AppName +
                                                                             " has already been installed.";

            // Allow upgrading to newer versions only.
            project.MajorUpgradeStrategy.UpgradeVersions = VersionRange.ThisAndOlder;

            // Remove existing products. Allows upgrading.
            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            // Build the installer.
            project.BuildMsi();
        }
    }
}
