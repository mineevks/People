using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace People.Api.Helpers
{
    public static class ControllersHelper
    {

        public static ActionResult ReturnContentResult(string response)
        {

            return new ContentResult
            {
                ContentType = "application/json",
                Content = response
            };
        }


        



    }
}
