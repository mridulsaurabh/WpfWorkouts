using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure
{
    public class RequestMessage
    {
        public RequestMessage()
        {
            this.UrlBuilder = new URLBuilder();
        }

        public Guid RequestId { get; set; }
        public HttpMethod Verb { get; set; }
        public URLBuilder UrlBuilder { get; set; }
    }

    public class RequestMessage<TInput> : RequestMessage
    {
        public RequestMessage()
        {
            this.UrlBuilder = new URLBuilder();
            this.Settings = new JsonSerializerSettings();
        }

        public TInput Content { get; set; }
        public JsonSerializerSettings Settings { get; set; }
    }
}
