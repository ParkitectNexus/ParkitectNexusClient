using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Utilities;
using CommandLine;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data;
using System.Reflection;

namespace ParkitectNexus.Client.Linux
{
    public class ArgumentService
    {
        private Socket socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        public bool IsServer{ get; private set; }
        private IQueueableTaskManager _queueTaskManager;
        private ILogger _logger;
        public ArgumentService(bool isClient,string[] args,ILogger logger,IQueueableTaskManager queuableTaskManager)
        {
            this._logger = logger;
            this._queueTaskManager = queuableTaskManager;
            
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            if (isClient)
            {
               
                try{
                    
                    socket.Connect(localEndPoint);
                    using (NetworkStream sr = new NetworkStream(socket))
                    {
                        using (StreamWriter writer = new StreamWriter(sr))
                        {
                            writer.WriteLine(args.Length); 
                            for(int x = 0; x < args.Length; x++)
                            {
                                writer.WriteLine(args[x]); 
                            }
                           
                        }
                    }
                    IsServer = false;
                }
                catch(SocketException)
                {
                    socket.Bind(localEndPoint);
                    socket.Listen(5000);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    socket.BeginAccept(new AsyncCallback(OnAccept), socket);
                    IsServer = true;
                }

            }
            else
            {
                socket.Bind(localEndPoint);
                socket.Listen(5000);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.BeginAccept(new AsyncCallback(OnAccept), socket);
                IsServer = true;
            }
        }

        private void OnAccept(IAsyncResult ar)
        {
            // Get the socket that handles the client request.
            System.Net.Sockets.Socket listener = (System.Net.Sockets.Socket)ar.AsyncState;
            System.Net.Sockets.Socket handler = listener.EndAccept(ar);
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
                    ProcessArguments(args);

                 }
            }
            socket.BeginAccept(new AsyncCallback(OnAccept), socket);

        }

        public void ProcessArguments(string[] arguments)
        {
            var options = new AppCommandLineOptions();
            Parser.Default.ParseArguments(arguments, options);
            if (options.Url != null)
            {
                NexusUrl url;
                if(NexusUrl.TryParse(options.Url,out url))
                {
                    var attribute = url.Data.GetType().GetCustomAttribute<UrlActionTaskAttribute>();
                    if (attribute?.TaskType != null && typeof(UrlQueueableTask).IsAssignableFrom(attribute.TaskType))
                    {
                        var task = ObjectFactory.Container.GetInstance(attribute.TaskType) as UrlQueueableTask;
                        task.Data = url.Data;
                     
                        _queueTaskManager.Add (task);                      
                    }

                }
            }
        }


    }
}

