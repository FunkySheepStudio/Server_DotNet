using System.Text.Json;
using System.Text.Json.Nodes;

namespace Server_Dotnet.Pages.Messages
{
    [Serializable]
    public class Message
    {
        JsonObject root = new JsonObject();

        public Message(string Controller = "", string Method = "", string ConnectionId = "")
        {
            root["Controller"] = Controller;
            root["Method"] = Method;
            root["ConnectionId"] = ConnectionId;
        }

        public Message(JsonDocument jsonDocument, string ConnectionId = "")
        {
            root = (JsonObject)JsonObject.Parse(jsonDocument.RootElement.ToString());
            root["ConnectionId"] = ConnectionId;
        }

		public Message(JsonDocument jsonDocument)
		{
            root = (JsonObject)JsonObject.Parse(jsonDocument.RootElement.ToString());
		}

		public string GetString(string property)
        {
            return root[property].ToString();
        }

        public void SetString(string property, string value)
        {
            root[property] = value;
        }

        public string ToJSON()
        {
            return root.ToString();
        }
    }
}
