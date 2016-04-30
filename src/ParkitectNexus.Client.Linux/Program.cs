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
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static App app;
        public static bool closed;


        [STAThread]
        public static void Main(string[] args)
        {
            String socketPath = Path.GetTempPath() + "/parkitect_nexus.socket";
            bool isHost = false;
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
                    using (NetworkStream sr = new NetworkStream(socket))
                    {
                        using (StreamWriter writer = new StreamWriter(sr))
                        {
                            writer.WriteLine(args.Length);
                            for (int x = 0; x < args.Length; x++)
                            {
                                writer.WriteLine(args[x]);
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
                ObjectFactory.SetUpContainer(registry);


                // Create the form and run its message loop. If arguments were specified, process them within the
                // form.
                var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();
                app = presenterFactory.InstantiatePresenter<App>();
                if (!app.Initialize(ToolkitType.Gtk))
                    return;


                if (args.Any())
                {
                    ProcessArgs(args);
                }

                app.Run();
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
            closed = true;
            socket.Close();
        }

        public static void OnAccept(IAsyncResult ar)
        {
            //release the listening thread and process the args
            allDone.Set();
            if (!closed)
            {
                Socket listener = (Socket) ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                using (NetworkStream stream = new NetworkStream(handler))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        int numberArguments = int.Parse(reader.ReadLine());
                        string[] args = new string[numberArguments];
                        for (int x = 0; x < numberArguments; x++)
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
            for (int x = 0; x < args.Length; x++)
            {
                if (args[x].Contains("parkitectnexus://"))
                {
                    options.Url = args[x];
                }
            }


            if (args.Any())
            {
                Parser.Default.ParseArguments(args, options);

                if (options.Url != null)
                {
                    NexusUrl url;
                    if (NexusUrl.TryParse(options.Url, out url))
                        app.HandleUrl(url);
                }
            }
        }

        public static bool CreateSocket(Socket s, UnixEndPoint end)
        {
            try
            {
                //listen for a connection and then rebind the accept
                var listeningThread = new Thread(delegate()
                {
                    s.Bind(end);
                    s.Listen(10);

                    while (s.IsBound)
                    {
                        allDone.Reset();
                        if (closed)
                            break;
                        //bind accept and listen for arguments
                        s.BeginAccept(OnAccept, s);
                        //end the thread when nothing is connected

                        allDone.WaitOne();
                    }
                });
                listeningThread.Start();

                return true;
            }
            catch (SocketException e)
            {
                throw e;
            }
        }
    }
}