using System.Collections.Generic;

namespace Xaminals.Infra.Results
{
    public class HttpResult<T> : IHttpResult
    {
        public bool IsError { get; set; }
        public string Token { get; set; }
        public T Result { get; set; }
        public List<Error> Errors { get; set; }

        public class Error
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }
    }
}
