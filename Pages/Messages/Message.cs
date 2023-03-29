using System.Text.Json;

namespace Server_Dotnet.Pages.Messages
{
    [Serializable]
    public class Message
    {
        public string Controller { get; set; }
        public string Method { get; set; }
        public string ConnectionId { get; set; } = "";

        JsonElement root;

        public Message(string Controller = "", string Method = "")
        {
            this.Controller = Controller;
            this.Method = Method;
        }

        public Message(JsonDocument jsonDocument) {
            root = jsonDocument.RootElement;
            this.Controller = root.GetProperty("Controller").ToString();
            this.Method = root.GetProperty("Method").ToString();
        }

        public string GetString(string property)
        {
            return root.GetProperty(property).ToString();
        }

        public string ToJSON()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
