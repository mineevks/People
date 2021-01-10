using System;
using System.Net;
using People.Models.Common.JsonApi;


namespace Utilities
{
    public static class ResponseHelper
    {

        public static Response ReturnSuccess()
        {
            return new Response
            {
                /*Data = new Data
                {
                    Type = "success"
                }*/
            };
        }

        

        
        public static Response ReturnInternalServerError(string exceptionMessage)
        {
            string desc;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                desc = exceptionMessage;
            }
            else
            {
                desc = HttpStatusCode.InternalServerError.ToString();
            }

            return new Response
            {
                Error = new Error
                {
                    Code = HttpStatusCode.InternalServerError.ToString(),
                    Desc = desc
                }
            };
        }

        public static Response ReturnBadRequest(string exceptionMessage)
        {
            string desc;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                desc = exceptionMessage;
            }
            else
            {
                desc = HttpStatusCode.BadRequest.ToString();
            }

            return new Response
            {
                Error = new Error
                {
                    Code = HttpStatusCode.BadRequest.ToString(),
                    Desc = desc
                }
            };
        }

    }
}
