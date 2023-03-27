using System.Net.WebSockets;

namespace Server_Dotnet.Pages.Sockets
{
    public class Connection
    {
        public string id;
        public WebSocket socket;

        public Connection(string id, WebSocket socket)
        {
            this.id = id;
            this.socket = socket;
        }
    }

    public interface ISocketsService
    {
        public List<Connection> connections { get; set; }
        void Start();
        void Send();
    }

    public class SocketsService : ISocketsService
    {
        List<Connection> _connections = new List<Connection>();
        List<Connection> ISocketsService.connections { get => _connections; set => _connections = value; }

        public SocketsService()
        {
            Console.WriteLine("Start");
        }


        public void Send()
        {
            Console.WriteLine("Send");
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
