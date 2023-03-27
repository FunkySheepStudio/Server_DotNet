using Server_Dotnet.Pages.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace Server_Dotnet.Pages.Messages
{
    public interface IMessagesService
    {
        public void Send(Message message);
        public void Send(String connectionId, Message message);
    }

    public class MessagesService : IMessagesService
    {
        ISocketsService socketService;
        public MessagesService(ISocketsService socketsService) {
            this.socketService = socketsService;
        }

        public void Send(Message message)
        {
            if (message.ConnectionId != null)
            {
                this.Send(message.ConnectionId, message);
            }
        }

        public void Send(String connectionId, Message message)
        {
            Connection? connection = socketService.connections.Find(item => item.id == connectionId);
            if (connection != null)
            {
                var dataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message.ToJSON()));
                connection.socket.SendAsync(dataToSend, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
