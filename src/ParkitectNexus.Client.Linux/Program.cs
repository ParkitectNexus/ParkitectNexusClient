// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using CommandLine;
using Mono.Unix;
using ParkitectNexus.Client.Base;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using Xwt;

namespace ParkitectNexus.Client.Linux
{
    public class Program
    {
        public static ManualResetEvent AllDone = new ManualResetEvent(false);
        public static App App;
        public static bool Closed;


        [STAThread]
        public static void Main(string[] args)
        {
            var socketPath = Path.GetTempPath() + "/parkitect_nexus.socket";
            var endPoint = new UnixEndPoint(socketPath);
            var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

            if (!File.Exists(Path.GetTempPath() + "/parkitect_nexus.socket"))
            {
                if (!CreateSocket(socket, endPoint)) return;
            }
            else
            {
                try
                {
                    socket.Connect(endPoint);
                    using (var sr = new NetworkStream(socket))
                    {
                        using (var writer = new StreamWriter(sr))
                        {
                            writer.WriteLine(args.Length);
                            foreach (var t in args)
                            {
                                writer.WriteLine(t);
                            }
                        }
                    }
                    return;
                }
                catch
                {
                    File.Delete(Path.GetTempPath() + "/parkitect_nexus.socket");
                    if (!CreateSocket(socket, endPoint)) return;
                }
            }


            try
            {
                // Initialize the structure map container.
                var registry = ObjectFactory.ConfigureStructureMap();
                registry.IncludeRegistry(new PresenterRegistry());
                registry.For<IApp>().Singleton().Use<App>();
                ObjectFactory.SetUpContainer(registry);


                // Create the form and run its message loop. If arguments were specified, process them within the
                // form.
                var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();
                App = presenterFactory.InstantiatePresenter<App>();
                if (!App.Initialize(ToolkitType.Gtk))
                    return;


                if (args.Any())
                {
                    ProcessArgs(args);
                }

                App.Run();
            }
            catch (Exception e)
            {
                // Report crash to the server.
                var crashReporterFactory = ObjectFactory.GetInstance<ICrashReporterFactory>();
                crashReporterFactory.Report("global", e);

                // Write the error to the log file.
                var log = ObjectFactory.GetInstance<ILogger>();
                log?.WriteLine("Application crashed!", LogLevel.Fatal);
                log?.WriteException(e);
            }
            Closed = true;
            socket.Close();
        }

        public static void OnAccept(IAsyncResult ar)
        {
            //release the listening thread and process the args
            AllDone.Set();
            if (!Closed)
            {
                var listener = (Socket) ar.AsyncState;
                var handler = listener.EndAccept(ar);
                using (var stream = new NetworkStream(handler))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var numberArguments = int.Parse(reader.ReadLine());
                        var args = new string[numberArguments];
                        for (var x = 0; x < numberArguments; x++)
                        {
                            args[x] = reader.ReadLine();
                        }
                        ProcessArgs(args);
                    }
                }
            }
        }

        //process the arguments passed into the application
        public static void ProcessArgs(string[] args)
        {
            var options = new AppCommandLineOptions();
            Parser.Default.ParseArguments(args, options);
            foreach (var t in args)
            {
                if (t.Contains("parkitectnexus://"))
                {
                    options.Url = t;
                }
            }


            if (args.Any())
            {
                Parser.Default.ParseArguments(args, options);

                if (options.Url != null)
                {
                    NexusUrl url;
                    if (NexusUrl.TryParse(options.Url, out url))
                        App.HandleUrl(url);
                }
            }
        }

        public static bool CreateSocket(Socket s, UnixEndPoint end)
        {
            //listen for a connection and then rebind the accept
            var listeningThread = new Thread(delegate()
            {
                s.Bind(end);
                s.Listen(10);

                while (s.IsBound)
                {
                    AllDone.Reset();
                    if (Closed)
                        break;
                    //bind accept and listen for arguments
                    s.BeginAccept(OnAccept, s);
                    //end the thread when nothing is connected

                    AllDone.WaitOne();
                }
            });
            listeningThread.Start();

            return true;
        }
    }
}