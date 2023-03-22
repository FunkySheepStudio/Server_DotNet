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
        private readonly Server_Dotnet.Pages.Users.Database userDb = new Server_Dotnet.Pages.Users.Database();
        private readonly Database authDb = new Database();

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
            string? connection = HttpContext.Request.Query["connection"];

            if (k1 == null || key == null || sig == null || connection == null) {
                return "{\"status\": \"ERROR\", \"reason\": \"Unable to login\"}";
            }

            var bytesSig = Encoders.Hex.DecodeData(sig);

            NBitcoin.Crypto.ECDSASignature signature = new NBitcoin.Crypto.ECDSASignature(bytesSig);
            NBitcoin.PubKey pubKey = new NBitcoin.PubKey(key);

            bool result = LNURL.LNAuthRequest.VerifyChallenge(signature, pubKey, Encoders.Hex.DecodeData(k1));
            
            if (result)
            {
                userDb.Add(new Server_Dotnet.Pages.Users.User(pubKey.ToString()));
                userDb.SaveChanges();

                authDb.Add(new Connection(k1, pubKey.ToString()));
                authDb.SaveChanges();

                _hubContext.Clients.Client(connection).SendAsync("ReceiveResponse", true);
                return "{\"status\": \"OK\"}";
            } else
            {
                _hubContext.Clients.Client(connection).SendAsync("ReceiveResponse", false);
                return "{\"status\": \"ERROR\", \"reason\": \"Unable to login\"}";
            }
        }
    }
}
