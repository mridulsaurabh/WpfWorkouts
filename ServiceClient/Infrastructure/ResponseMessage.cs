using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure
{
    public class ResponseMessage<TOutput>
    {
        public HttpStatusCode StatusCode { get; set; }
        public List<Error> Errors { get; set; }
        public TOutput Content { get; set; }
        public string ResponseString { get; set; }

    }
}
