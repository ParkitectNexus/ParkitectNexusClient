// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using System.Runtime.InteropServices;
using WixSharp;
using WixSharp.Controls;
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
        private const string AppExecutable = @"ParkitectNexusClient.exe";
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
                new Binary(new Id("Install_VC"), @"..\..\vc_redist.x86.exe"),
                new BinaryFileAction("Install_VC", "/q /norestart",
                    Return.asyncNoWait,
                    When.Before,
                    Step.InstallFinalize,
                    Condition.NOT_Installed),
                new Dir(new Id("INSTALL_DIR"), @"%ProgramFiles%\" + AppName,
                    new File(AppExecutable),
                    new File(@"CommandLine.dll"),
                    new File(@"Newtonsoft.Json.dll"),
                    new File(@"Octokit.dll"),
                    new File(@"ParkitectNexus.Data.dll"),
                    new File(@"ParkitectNexus.ModLoader.dll"),
                    new File(@"ParkitectNexus.Mod.ModLoader.dll")
                    ),
                new Dir(@"%ProgramMenu%\" + AppName,
                    new ExeFileShortcut(AppName, $"[INSTALL_DIR]{AppExecutable}", ""),
                    new ExeFileShortcut("Launch Parkitect with Mods", $"[INSTALL_DIR]{AppExecutable}", "--launch"),
                    new ExeFileShortcut("Uninstall " + AppName, "[System64Folder]msiexec.exe", "/x [ProductCode]")
                    ),
                new Dir(@"%Desktop%",
                    new ExeFileShortcut(AppName, $"[INSTALL_DIR]{AppExecutable}", ""),
                    new ExeFileShortcut("Launch Parkitect with Mods", $"[INSTALL_DIR]{AppExecutable}", "--launch") { IconFile = AppIcon } ),
                new InstalledFileAction(AppExecutable, "--silent")
                )
            {
                GUID = guid,
                UI = WUI.WixUI_InstallDir,
                SourceBaseDir = AppBinariesPath,
                OutFileName = "ParkitectNexusSetup" + (Configuration != "Release" ? Configuration : string.Empty),
                OutDir = @"..\..\bin",
                Version = version,
                Description = "An installer for Theme Parkitect.",
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
                },
                // Use CustomUI to skip the license page.
//                CustomUI = new DialogSequence()
//                    .On(NativeDialogs.WelcomeDlg, Buttons.Next, new ShowDialog(NativeDialogs.InstallDirDlg))
//                    .On(NativeDialogs.InstallDirDlg, Buttons.Back, new ShowDialog(NativeDialogs.WelcomeDlg))
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