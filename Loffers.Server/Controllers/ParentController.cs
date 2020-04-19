using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Loffers.Server.Controllers
{
    public class ParentController : ControllerBase
    {
        public class HttpResult
        {
            public HttpResult(object result, bool isError = false, List<Error> errors = null, string token = null)
            {
                this.Result = result;
                this.IsError = isError;
                this.Errors = errors;
                this.Token = token;
            }

            public bool IsError { get; set; }
            public object Result { get; set; }
            public List<Error> Errors { get; set; }
            public string Token { get; set; }


            public class ErrorCodes
            {
                public static readonly string INVALIDCREDENTIALS = "InvalidCredentials";
                public static readonly string GENERALERROR = "GeneralError";
                public static readonly string USERDOESNTEXIST = "UserNotFound";
                public static readonly string NOTFOUNDERROR = "NotFound";
            }

            internal static List<Error> SingleError(string code, string errorDescription)
            {
                return new List<Error>() { new Error() { Code = code, Description = errorDescription } };
            }
        }

        public class Error : IdentityError
        {

        }
    }
}
