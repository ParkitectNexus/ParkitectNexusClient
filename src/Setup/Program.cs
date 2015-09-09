using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WixSharp;
using WixSharp.Forms;

namespace WixSharpSetup
{
    class Program
    {
        private const string AppIcon = @"..\nexus.ico";
        private const string AppName = @"Parkitect Nexus Client";
        private const string AppExecutable = @"ParkitectNexusClient.exe";
        private const string AppBinariesPath = @"..\ParkitectNexusClient\bin\Release\";
        static void Main()
        {
            var assembly = System.Reflection.Assembly.LoadFrom(AppBinariesPath + AppExecutable);
            var assemblyName = assembly.GetName();
            var project = new ManagedProject(AppName,
                new Dir(@"%ProgramFiles%\" + AppName,
                    new File(AppExecutable,
                        new FileShortcut(AppName, "INSTALLDIR"),
                        new FileShortcut("MyApp", @"%ProgramMenu%\" + AppName) {IconFile = AppIcon}
                        ),
                    new File(@"CommandLine.dll")),
                new Dir(@"%ProgramMenu%\" + AppName,
                    new ExeFileShortcut("Uninstall " + AppName, "[System64Folder]msiexec.exe", "/x [ProductCode]")),

                 new InstalledFileAction(AppExecutable, "-s")
                )
            {
                GUID = new Guid(((GuidAttribute) assembly.GetCustomAttributes(typeof (GuidAttribute), true)[0]).Value),
                ManagedUI = new ManagedUI(),
                SourceBaseDir = AppBinariesPath,
                OutDir = "bin",
                Version = assemblyName.Version,
                Description = "An installer for Theme Parkitect.",
                LicenceFile = null,
                MajorUpgradeStrategy = MajorUpgradeStrategy.Default,
                ControlPanelInfo =
                {
                    NoRepair = true,
                    // NoModify = true;
                    HelpLink = "https://parkitectnexus.com",
                    ProductIcon = AppIcon,
                    Manufacturer = "Parkitect Nexus, Tim Potze"
                },
            };

            project.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;

            //custom set of standard UI dialogs
            project.ManagedUI.InstallDialogs.Add(Dialogs.Welcome)
                                            .Add(Dialogs.InstallDir)
                                            .Add(Dialogs.Progress)
                                            .Add(Dialogs.Exit);
            
            project.BuildMsi();
        }
    }
}
