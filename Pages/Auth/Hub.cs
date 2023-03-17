using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Server_Dotnet.Pages.Auth
{
    //[Route("hub/[controller]")]
    public class ChatHub : Hub
    {
        public async Task SendMessage(string id)
        {
            Debug.WriteLine(id);
            await Clients.All.SendAsync("ReceiveMessage", id);
        }
    }
}
