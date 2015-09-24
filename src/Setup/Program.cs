// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using System.Runtime.InteropServices;
using WixSharp;
using Assembly = System.Reflection.Assembly;
using File = WixSharp.File;

namespace WixSharpSetup
{
    internal class Program
    {
        private const string AppIcon = @"..\..\images\nexus.ico";
        private const string AppName = @"ParkitectNexus Client";
        private const string AppExecutable = @"ParkitectNexusClient.exe";
        private const string AppBinariesPath = @"..\..\bin\";

        private static void Main()
        {
            var assembly = Assembly.LoadFrom(AppBinariesPath + AppExecutable);
            var assemblyName = assembly.GetName();
            var project = new ManagedProject(AppName,
                new Dir(@"%ProgramFiles%\" + AppName,
                    new File(AppExecutable,
                        new FileShortcut(AppName, @"%ProgramMenu%\" + AppName) {IconFile = AppIcon}
                        ),
                    new File(@"CommandLine.dll"),
                    new File(@"Newtonsoft.Json.dll"),
                    new File(@"Octokit.dll"),
                    new File(@"ParkitectModLauncher.dll"),
                    new File(@"ParkitectNexus.Data.dll"),
                    new File(@"Parkitectnexus.ModLoader.dll")
                    ),
                new Dir(@"%ProgramMenu%\" + AppName,
                    new ExeFileShortcut("Uninstall " + AppName, "[System64Folder]msiexec.exe", "/x [ProductCode]")),
                new InstalledFileAction(AppExecutable, "-s")
                )
            {
                GUID = new Guid(((GuidAttribute) assembly.GetCustomAttributes(typeof (GuidAttribute), true)[0]).Value),
                UI = WUI.WixUI_InstallDir,
                SourceBaseDir = AppBinariesPath,
                OutFileName = "ParkitectNexusSetup",
                OutDir = @"..\..\bin",
                Version = assemblyName.Version,
                Description = "An installer for Theme Parkitect.",
                MajorUpgradeStrategy = MajorUpgradeStrategy.Default,
                LicenceFile = null,
                BannerImage = Directory.GetCurrentDirectory() + @"\..\..\images\dialog_banner.png",
                BackgroundImage = Directory.GetCurrentDirectory() + @"\..\..\images\dialog_bg.png",
                ControlPanelInfo =
                {
                    NoRepair = true,
                    HelpLink = "https://parkitectnexus.com",
                    ProductIcon = AppIcon,
                    Manufacturer = "ParkitectNexus, Tim Potze"
                },
                CustomUI = new DialogSequence()
                    // Skip license
                    .On(NativeDialogs.WelcomeDlg, Buttons.Next, new ShowDialog(NativeDialogs.InstallDirDlg))
                    .On(NativeDialogs.InstallDirDlg, Buttons.Back, new ShowDialog(NativeDialogs.WelcomeDlg))
            };

            project.MajorUpgradeStrategy.NewerProductInstalledErrorMessage = "A newer version of " + AppName +
                                                                             " has already been installed.";
            project.MajorUpgradeStrategy.UpgradeVersions = VersionRange.ThisAndOlder;
            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            project.BuildMsi();
        }
    }
}