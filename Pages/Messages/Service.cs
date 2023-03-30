using Server_Dotnet.Pages.Auth;
using Server_Dotnet.Pages.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace Server_Dotnet.Pages.Messages
{
    public interface IMessagesService
    {
        public void Send(Message message);
        public void Send(String connectionId, Message message);
        public void SendToAdmins(Messages.Message message);
	}

    public class MessagesService : IMessagesService
    {
        ISocketsService socketService;
        IAuthService authService;
        public MessagesService(ISocketsService socketsService, IAuthService authService) {
            this.socketService = socketsService;
            this.authService = authService;
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

		public void SendToAdmins(Message message)
		{
			List<User_Connections> Admins = authService.users_Connections.FindAll((item) => item.user.Admin == true);

			for (int i = 0; i < Admins.Count; i++)
			{
				Send(Admins[i].connection.id, message);
			}
		}
	}
}
