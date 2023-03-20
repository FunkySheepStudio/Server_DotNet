using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NBitcoin.DataEncoders;
using Server_Dotnet.Pages.Auth;

namespace Server_Dotnet.Api.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHubContext<AuthHub> _hubContext;

        public AuthController(IHubContext<AuthHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public string Get()
        {
            string? k1 = HttpContext.Request.Query["k1"];
            string? key = HttpContext.Request.Query["key"];
            string? sig = HttpContext.Request.Query["sig"];

            var bytesSig = Encoders.Hex.DecodeData(sig);

            NBitcoin.Crypto.ECDSASignature signature = new NBitcoin.Crypto.ECDSASignature(bytesSig);
            NBitcoin.PubKey pubKey = new NBitcoin.PubKey(key);

            bool result = LNURL.LNAuthRequest.VerifyChallenge(signature, pubKey, Encoders.Hex.DecodeData(k1));
            
            if (result)
            {
                _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Home page loaded at: {DateTime.Now}");
                Console.WriteLine(key);
                return "{\"status\": \"OK\"}";
            } else
            {
                return "{\"status\": \"ERROR\", \"reason\": \"Unable to login\"}";
            }
        }
    }
}
