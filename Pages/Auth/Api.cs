using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Server_Dotnet.Api.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "{\"status\": \"ok\"}";
        }
    }
}
