using Microsoft.AspNetCore.Mvc;

namespace Server_Dotnet.Pages.Messages
{
    public class MessageController : ControllerBase
    {
        IMessagesService messagesService;
        public MessageController(IMessagesService messagesService) {
            this.messagesService = messagesService;
        }
        public void Add(Message message)
        {

        }
    }
}
