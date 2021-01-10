using System.ComponentModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace People.Models.Common
{
    public enum ErrorType
    {
        //[Description("OK")]
        [EnumMember(Value = "OK")]
        Ok = 0,

        //[Description("Internal error")]
        [EnumMember(Value = "Internal error")]
        InternalError = 1,

        //[Description("Authentication failed")]
        [EnumMember(Value = "Authentication failed")]
        AuthFailed = 2,

        //[Description("Bad request")]
        [EnumMember(Value = "Bad request")]
        BadRequest = 5,


    }
}
