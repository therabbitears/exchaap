using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Loffers.Server.Controllers
{
    public class ParentController : ApiController
    {
        public string UserId
        {
            get
            {
                string nameIdentifier = ClaimTypes.NameIdentifier;
                var claimUsers = User as ClaimsPrincipal;
                var userId = claimUsers.Claims.FirstOrDefault(c => c.Type == nameIdentifier);
                if (userId != null)
                {
                    return userId.Value;
                }

                return string.Empty;
            }
        }

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

        public class Error
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }
    }
}
