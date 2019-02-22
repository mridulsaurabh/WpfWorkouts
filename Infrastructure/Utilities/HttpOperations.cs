using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utility
{
    public enum HttpOperationErrors
    {
        InternetNotAvailable,
        InvalidData,
        ServiceUnavailable,
        ErrorRetreivingData,
    }

    public class HttpOperations
    {
        private static object _syncRoot;
        private static HttpOperations httpOperations;
        private EventHandler<HttpOperationsFailedEventArgs> HttpOperationsFailed;
        public HttpOperations()
        {
            _syncRoot = new object();
        }

        public static HttpOperations Instance
        {
            get
            {
                if (HttpOperations.httpOperations == null)
                {
                    lock (_syncRoot)
                    {
                        if (HttpOperations.httpOperations == null)
                        {
                            HttpOperations.httpOperations = new HttpOperations();
                        }
                    }
                }
                return HttpOperations.httpOperations;
            }
        }

        public async Task<string> GetData(string link)
        {
            string response = string.Empty;
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    HttpClientHandler handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    }
                    HttpClient httpClient = new HttpClient(handler);
                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, link);
                    req.Headers.Add(SessionConstants.IgnoreAuthorizationValue, "true");
                    HttpResponseMessage resp = await httpClient.SendAsync(req);
                    response = await resp.Content.ReadAsStringAsync();
                    this.HandleErrors(resp);
                }
                else
                {
                    if (this.HttpOperationsFailed != null)
                    {
                        this.HttpOperationsFailed(this, new HttpOperationsFailedEventArgs()
                        {
                            Error = HttpOperationErrors.InternetNotAvailable,
                            Message = ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<string> PostData(string link, string postData)
        {
            string response = string.Empty;
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    HttpClientHandler handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    }
                    HttpClient httpClient = new HttpClient(handler);
                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, link);
                    req.Headers.Add(SessionConstants.IgnoreAuthorizationValue, "true");
                    req.Content = (HttpContent)new StringContent(postData, Encoding.UTF8, SessionConstants.ApplicationMediaType);
                    HttpResponseMessage resp = await httpClient.SendAsync(req);
                    response = await resp.Content.ReadAsStringAsync();
                    this.HandleErrors(resp);
                }
                else
                {
                    if (this.HttpOperationsFailed != null)
                    {
                        this.HttpOperationsFailed(this, new HttpOperationsFailedEventArgs()
                        {
                            Error = HttpOperationErrors.InternetNotAvailable,
                            Message = ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        private void HandleErrors(HttpResponseMessage resp)
        {
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                if (resp.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    if (null != this.HttpOperationsFailed)
                        this.HttpOperationsFailed((object)this, new HttpOperationsFailedEventArgs()
                        {
                            Error = HttpOperationErrors.ServiceUnavailable
                        });
                }
                else if (resp.StatusCode == HttpStatusCode.InternalServerError)
                    this.HttpOperationsFailed((object)this, new HttpOperationsFailedEventArgs()
                    {
                        Error = HttpOperationErrors.ErrorRetreivingData
                    });
                else if (resp.StatusCode != HttpStatusCode.NoContent)
                    this.HttpOperationsFailed((object)this, new HttpOperationsFailedEventArgs()
                    {
                        Error = HttpOperationErrors.InvalidData
                    });
            }
        }
    }

    public class HttpOperationsFailedEventArgs : EventArgs
    {
        public HttpOperationErrors Error { get; set; }

        public string Message { get; set; }
    }

}
