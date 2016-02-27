
using Mono.Unix;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Tasks;
using System;
using CommandLine;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Tasks.Prefab;
using System.IO;
using System.Reflection;

namespace ParkitectNexus.Client.Linux
{
    public class ArgumentService
    {
        
        public bool IsServer{ get; private set;}
        private IQueueableTaskManager _queueTaskManager;
        private EndPoint _endPoint;
        private Socket _socket;
        public ArgumentService(bool isClient,string[] args,ILogger logger,IQueueableTaskManager queuableTaskManager)
        {
            this._queueTaskManager = queuableTaskManager;
            _endPoint = new UnixEndPoint("Parkitect_Unix_Port");
            _socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

            if(!System.IO.File.Exists("Parkitect_Unix_Port"))
            {
                    CreateServer();
            }
            else
            {
                try{
                    IsServer = false;
                    _socket.Connect(_endPoint);
                    using (NetworkStream sr = new NetworkStream(_socket))
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
                    _socket.Close();
                }  
                catch
                {
                    System.IO.File.Delete("Parkitect_Unix_Port");
                    CreateServer();
                }
            }
  

        }

        private void CreateServer() 
        {
            try
            {
            IsServer = true;
            _socket.Bind(_endPoint);
            _socket.Listen(10);

            var listeningThread = new Thread(Listen);
            listeningThread.Start();
            }
            catch(System.Net.Sockets.SocketException e)
            {
                throw e;
            }
        }

        private void Listen()
        {
            while (_socket.Connected)
                {
                    _socket.BeginAccept(new System.AsyncCallback(OnAccept),_socket);
                }
            
        }

        private void OnAccept(IAsyncResult ar)
        {
           
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
            _socket.BeginAccept(new AsyncCallback(OnAccept), _socket);

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

        public void Close()
        {
            _socket.Close();
        }

       

    }
}

