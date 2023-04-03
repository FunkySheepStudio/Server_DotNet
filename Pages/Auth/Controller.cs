using Microsoft.AspNetCore.Mvc;
using NBitcoin.DataEncoders;
using Server_Dotnet.Pages.Auth;
using Server_Dotnet.Pages.Messages;
using Server_Dotnet.Pages.Sockets;
using Server_Dotnet.Pages.Users;

namespace Server_Dotnet.Auth
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
		ISocketsService _socketService;
		IMessagesService _messagesService;
        IAuthService _authService;

        private readonly Server_Dotnet.Pages.Users.Database userDb = new ();

        public AuthController(ISocketsService socketService, IMessagesService messagesService, IAuthService authService)
        {
			this._socketService = socketService;
			this._messagesService = messagesService;
			this._authService = authService;
		}

        [HttpGet]
        public string Get()
        {
            string? k1 = HttpContext.Request.Query["k1"];
            string? key = HttpContext.Request.Query["key"];
            string? sig = HttpContext.Request.Query["sig"];

            if (k1 == null || key == null || sig == null) {
                return "{\"status\": \"ERROR\", \"reason\": \"Unable to login\"}";
            }

            var bytesSig = Encoders.Hex.DecodeData(sig);

            NBitcoin.Crypto.ECDSASignature signature = new NBitcoin.Crypto.ECDSASignature(bytesSig);
            NBitcoin.PubKey pubKey = new NBitcoin.PubKey(key);

            bool result = LNURL.LNAuthRequest.VerifyChallenge(signature, pubKey, Encoders.Hex.DecodeData(k1));

			Connection? connection = _socketService.connections.Find((item) => item.id == k1);

			if (result && connection != null)
            {
				User? user = userDb.Find<User>(pubKey.ToString());

				if (user == null) {
                    user = new Server_Dotnet.Pages.Users.User(pubKey.ToString(), true);
					userDb.Add(user);
                    userDb.SaveChanges();
                }

				Message message = new Message("Auth", "AuthOk", k1);
                this._messagesService.Send(message);
				this._messagesService.SendToAdmins(message);

				_authService.users_Connections.Add(new User_Connections(user, connection));

				return "{\"status\": \"OK\"}";
            } else
            {
                Message message = new Message("Auth", "AuthNOk", k1);
                this._messagesService.Send(message);
                return "{\"status\": \"ERROR\", \"reason\": \"Unable to login\"}";
            }
        }
    }
}
