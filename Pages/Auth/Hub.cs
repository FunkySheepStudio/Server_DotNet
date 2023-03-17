using Microsoft.AspNetCore.SignalR;

namespace Server_Dotnet.Pages.Auth
{
    //[Route("hub/[controller]")]
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine(Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string id)
        {
            Console.WriteLine(id);
            await Clients.All.SendAsync("ReceiveMessage", id);
        }
    }
}
