using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Server_Dotnet.Pages.Auth
{
    public class IndexModel : PageModel
    {
        public Uri? uri;

        public void OnGet()
        {
            string url = HttpContext.Request.IsHttps ? "https://" : "http://";
            url += HttpContext.Request.Host + "/api/auth/?tag=login&k1=";

            Random rnd = new Random();
            var k1 = new Byte[32];
            rnd.NextBytes(k1);

            var hexArray = Array.ConvertAll(k1, x => x.ToString("X2"));
            var hexStr = String.Concat(hexArray);

            uri = new Uri(url + hexStr);
            uri = LNURL.LNURL.EncodeUri(uri, "login", true);
        }
    }
}
