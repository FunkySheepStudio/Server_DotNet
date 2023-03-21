using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Server_Dotnet.Pages.Auth
{
    [Route("/auth/hub")]
    public class AuthHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Random rnd = new Random();
            var k1 = new Byte[32];
            rnd.NextBytes(k1);

            var hexArray = Array.ConvertAll(k1, x => x.ToString("X2"));
            var hexStr = String.Concat(hexArray);

            Clients.Caller.SendAsync("ReceiveToken", hexStr);
            return base.OnConnectedAsync();
        }
        [Authorize("CustomSignalrAuth")]
        public async Task SendMessage(string id)
        {
            Console.WriteLine(id);
            await Clients.All.SendAsync("ReceiveMessage", id);
        }
    }
}
