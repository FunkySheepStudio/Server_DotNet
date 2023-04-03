using Server_Dotnet.Pages.Sockets;

namespace Server_Dotnet.Pages.Auth
{
	public class User_Connections
	{
		public Users.User user;
		public Connection connection;

		public User_Connections(Users.User user, Connection connection)
		{
			this.user = user;
			this.connection = connection;
		}
	}

	public interface IAuthService
	{
		public List<User_Connections> users_Connections { get; set; }
	}

	public class AuthService : IAuthService
	{
		List<User_Connections> _users_Connections = new();
		List<User_Connections> IAuthService.users_Connections { get => _users_Connections; set => _users_Connections = value; }

		public static Uri GetAuthUri(string pathBase, string SocketID)
        {
            string url = pathBase + "/auth/?tag=login&k1=";

            Uri uri = new Uri(url + SocketID);

			Console.WriteLine(uri.ToString());

            return LNURL.LNURL.EncodeUri(uri, "login", true);
        }
	}
}
