using System;
using System.IO;
using System.Diagnostics;
using Gtk;
using ParkitectNexus.Data.Web;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.GTK
{
    public partial class ParkitectUpdate : Gtk.Dialog
    {
        private UpdateInfo _updateinfo;

        public static void MigrateMods(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (!parkitect.IsInstalled)
                return;

            var oldPath = parkitect.Paths.NativeMods;

            if (!Directory.Exists(oldPath))
                return;

            foreach (var directory in Directory.GetDirectories(oldPath))
            {
                var target = System.IO.Path.Combine(parkitect.Paths.Mods, System.IO.Path.GetFileName(directory));

                if (!File.Exists(System.IO.Path.Combine(directory, "mod.json")) || Directory.Exists(target))
                    continue;

                Directory.Move(directory, target);
            }
        }

        public ParkitectUpdate (UpdateInfo updateInfo,ClientSettings settings,CommandLineOptions options)
        {

            // Store download url so it can be downloaded after the update.
            if (!string.IsNullOrEmpty(options.DownloadUrl))
                settings.DownloadOnNextRun = options.DownloadUrl;
            else
                settings.BootOnNextRun = !options.Silent;
            settings.Save();

            this.Build ();

            this._updateinfo = updateInfo;
            if (updateInfo != null)
            {
                this.updateInfo.Text = "A required update for the ParkitectNexus Client needs to be installed. Without this update you won't be able to install blueprints, savegames or mods trough this application. The update should take less than a minute.\n Would you like to install it now?\n\nYou are currently running v"+   Assembly.GetExecutingAssembly().GetName().Version +". The newest version is v" + updateInfo.Version + " ParkitectNexus Client";
            }
        }

        /// <summary>
        ///     Checks for available updates.
        /// </summary>
        /// <returns>Information about the available update.</returns>
        public static UpdateInfo CheckForUpdates(IParkitectNexusWebsite website)
        {
            #if DEBUG
                return null;
            #endif
           
            return null;
        }

        /// <summary>
        /// on cancle the update utility will send a cancel response for the dialog
        /// </summary>
        /// <returns><c>true</c> if this instance cancel sender e; otherwise, <c>false</c>.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void Cancel (object sender, EventArgs e)
        {
            this.Respond(ResponseType.Cancel);
        }

        /// <summary>
        /// Proceeds to update the client.
        /// </summary>
        protected void ProceedToUpdate (object sender, EventArgs e)
        {
            #if DEBUG
            #else
            try
            {
                var targetFolder = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "Downloads");

                if(!Directory.Exists(targetFolder))
                    targetFolder = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "Documents");

                if(!Directory.Exists(targetFolder))
                    targetFolder = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal));

                if(!Directory.Exists(targetFolder))
                {
                    Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Failed installing the update! Please try again later.");
                    if (errorDialog.Run () == (int)Gtk.ResponseType.Ok) {
                        this.Respond(ResponseType.Cancel);
                    }
                }

                var tempPath = System.IO.Path.Combine(targetFolder, $"parkitectnexus-client-{_updateinfo.Version}-{tempRandom(6)}.dmg");

                using (var webClient = new ParkitectNexusWebClient())
                {
                    webClient.DownloadFile(_updateinfo.DownloadUrl, tempPath);

                    Process.Start(new ProcessStartInfo(
                        "hdiutil",
                        "attach \"" + tempPath + "\"")
                        {UseShellExecute = false});
                }

                this.Destroy();
            }
            catch
            {
                Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Failed installing the update! Please try again later.");
                if (errorDialog.Run () == (int)Gtk.ResponseType.Ok) {
                    this.Respond(ResponseType.Cancel);
                }
            }
            #endif

        }

        private static string tempRandom(int length)
        {
            var eligable = Enumerable.Range(0, 36).Select(n => n < 10 ? (char)(n + '0') : (char)('a' + n - 10)).ToArray();
            var random = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(n => eligable[random.Next(eligable.Length)]));
        }

    }
}

