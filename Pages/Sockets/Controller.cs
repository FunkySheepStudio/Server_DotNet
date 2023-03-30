using Microsoft.AspNetCore.Mvc;
using System.Buffers;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using Server_Dotnet.Pages.Messages;

namespace Server_Dotnet.Pages.Sockets
{
    public class SocketsController : ControllerBase
    {
        ISocketsService socketService;
        IMessagesService messagesService;

        public SocketsController(ISocketsService socketService, IMessagesService messagesService)
        {
            this.socketService = socketService;
            this.messagesService = messagesService;
        }

        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                await OnConnection();
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Listen(Connection connection)
        {
            using IMemoryOwner<byte> memory = MemoryPool<byte>.Shared.Rent(1024 * 4);
            var request = await connection.socket.ReceiveAsync(
                memory.Memory, CancellationToken.None);

            while (request.MessageType != WebSocketMessageType.Close)
            {
				switch (request.MessageType)
                {
                    case WebSocketMessageType.Text:
                        string msg = Encoding.UTF8.GetString(memory.Memory.Span);

                        JsonDocument jsonDocument = JsonDocument.Parse(memory.Memory.Slice(0, request.Count));
                        Messages.Message message = new(jsonDocument);
                        message.ConnectionId = connection.id;
                        break;
                    default:
                        break;
                }

                request = await connection.socket.ReceiveAsync(
                memory.Memory, CancellationToken.None);
            }

            await connection.socket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closing",
                CancellationToken.None);

            OnDisconnection(connection);
        }

		async Task OnConnection()
        {
			var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
			Connection connection = new(GenerateConnectionId(), webSocket);
			this.socketService.connections.Add(connection);

			try
			{
				// Send the generated ID
				Message message = new Message("Socket", "SetId");
				message.ConnectionId = connection.id;
				this.messagesService.Send(message);
				this.messagesService.SendToAdmins(message);

				await Listen(connection);
			}
			catch
			{
				OnDisconnection(connection);
			}
		}

        void OnDisconnection(Connection connection)
        {
            //Console.WriteLine($"Disconnection: {connection.id} ");
            Connection? closedConnection = this.socketService.connections.Find(item => item == connection);

            if (closedConnection != null)
            {
                this.socketService.connections.Remove(closedConnection);
            }
        }

        string GenerateConnectionId()
        {
            Random rnd = new Random();
            var k1 = new Byte[32];
            rnd.NextBytes(k1);

            var hexArray = Array.ConvertAll(k1, x => x.ToString("X2"));
            return String.Concat(hexArray);
        }

        public void GetConnectionID(Messages.Message message)
        {
            Console.WriteLine("execute");
            Connection? connection = this.GetConnection(message.ConnectionId);
            if (connection != null) {
                var dataToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message.ToJSON()));
                connection.socket.SendAsync(dataToSend, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public Connection? GetConnection(String connectionId)
        {
            return this.socketService.connections.Find(connection => connection.id == connectionId);
        }
    }
}
