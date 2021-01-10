using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using People.Models.Common;
using People.Models.Settings;
using Utilities;

namespace People.Api.Controllers
{

    public class HomeController : Controller
    {
        private WSettings _wSettings;

        public HomeController(
            IOptions<WSettings> wSettings
        )
        {
            _wSettings = wSettings.Value;
        }

        public ContentResult Index()
        {
            LoggerStatic.Logger.Debug("Api Index");

            var indexPage = new IndexPage
            {
                ServiceName = "Api",
            };

            var text = SerializerJson.SerializeObjectToJsonString(indexPage);
            
            return new ContentResult
            {
                ContentType = "application/json",
                Content = text,
                StatusCode = (int)HttpStatusCode.OK
            };

        }
       
    }
}
