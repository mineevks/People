using System.Xml.Serialization;

namespace People.Models.Common
{

    public class ResponseError
    {
        public ResponseError(ErrorType errorType)
        {
            Code = (int)errorType;
            Text = errorType.ToString();
        }

        public int Code { get; set; }

        public string Text { get; set; }
    }

    
    



}