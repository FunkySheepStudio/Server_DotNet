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
    }

    public class SocketsService : ISocketsService
    {
        List<Connection> _connections = new List<Connection>();
        List<Connection> ISocketsService.connections { get => _connections; set => _connections = value; }
    }
}
