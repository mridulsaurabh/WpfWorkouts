using Infrastructure.Utility;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using ServiceClient.Infrastructure;
using ServiceClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure
{
    public class ServiceBus 
    {
        private ServiceContext _serviceContext;
        private ISettingsProvider _SettingsProvider;

        public ServiceBus(ServiceContext serviceContext)
        {
            this._serviceContext = serviceContext;
            this._SettingsProvider = null;
            this.Authenticate();
        }
        
        private void Authenticate()
        {
            try
            {
                if (_serviceContext == null)
                {
                    _serviceContext = new ServiceContext(); // use the overloaded constructor to instantiate the service context.
                    //settingProvier.GetValue<ServiceContext>(SessionConstants.ServiceContext);
                }

                AuthenticationContext authenticationContext = new AuthenticationContext(_serviceContext.Authority);
                AuthenticationResult authenticationResult = authenticationContext.AcquireToken(_serviceContext.AppIdURI, _serviceContext.ClientID, _serviceContext.RedirectURI); // SessionService.GetValue<AuthenticationResult>(SessionConstants.AuthenticationResult);

                if (authenticationResult == null)
                {
                    authenticationResult = authenticationContext.AcquireToken(_serviceContext.AppIdURI, _serviceContext.ClientID, _serviceContext.RedirectURI);
                    //settingProvier.SetValue<AuthenticationResult>(SessionConstants.AuthenticationResult, result);
                }
                else
                {
                    if (authenticationResult.ExpiresOn.LocalDateTime.TimeOfDay < System.DateTime.Now.TimeOfDay)
                    {
                        authenticationResult = authenticationContext.AcquireToken(_serviceContext.AppIdURI, _serviceContext.ClientID, _serviceContext.RedirectURI);
                        //settingProvier.SetValue<AuthenticationResult>(SessionConstants.AuthenticationResult, result);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("user is not authorized to use this proxy.");
            }
        }

        public ResponseMessage<TOutput> Invoke<TOutput>(RequestMessage request)
        {
            Task<HttpResponseMessage> response = null;
            ResponseMessage<TOutput> output = new ResponseMessage<TOutput>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (_serviceContext == null)
                    {
                        return null;
                    }

                    string requestContent = string.Empty;
                    HttpContent httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(request.Verb, string.Format("{0}{1}", _serviceContext.ResourceURL, request.UrlBuilder.GetUrl()));
                    // skip the autorization for now
                    httpRequestMessage.Headers.Add("IgnoreAuthValue", "true");
                    //var accessToken = "JWTaccessToken";//settingProvier.GetValue<ServiceContext>(SessionConstants.ServiceContext);.
                    //httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpRequestMessage.Content = request.Verb == HttpMethod.Get ? null : httpContent;
                    response = httpClient.SendAsync(httpRequestMessage);
                    var responseContent = response.Result.Content.ReadAsStringAsync();
                    output.Content = JsonConvert.DeserializeObject<TOutput>(responseContent.Result);
                }
            }
            catch (HttpRequestException ex)
            {
                output = new ResponseMessage<TOutput>();
                output.StatusCode = response.Result.StatusCode;
                output.Errors = new List<Error>() 
                {
                   new Error(ex.Message, request.Verb.ToString()) 
                };
            }

            return output;
        }

        public ResponseMessage<TOutput> Invoke<TInput, TOutput>(RequestMessage<TInput> request)
        {
            Task<HttpResponseMessage> response = null;
            ResponseMessage<TOutput> output = new ResponseMessage<TOutput>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (this._serviceContext == null)
                    {
                        return null;
                    }

                    string requestContent = string.Empty;
                    HttpContent httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(request.Verb, string.Format("{0}{1}", _serviceContext.ResourceURL, request.UrlBuilder.GetUrl()));
                    // skip the autorization for now
                    httpRequestMessage.Headers.Add("IgnoreAuthValue", "true");
                    //var accessToken = "JWTaccessToken";//settingProvier.GetValue<ServiceContext>(SessionConstants.ServiceContext);.
                    //httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpRequestMessage.Content = request.Verb == HttpMethod.Get ? null : httpContent;
                    response = httpClient.SendAsync(httpRequestMessage);
                    var responseContent = response.Result.Content.ReadAsStringAsync();
                    output.Content = JsonConvert.DeserializeObject<TOutput>(responseContent.Result);
                }
            }
            catch (HttpRequestException ex)
            {
                output = new ResponseMessage<TOutput>();
                output.StatusCode = response.Result.StatusCode;
                output.Errors = new List<Error>() 
                {
                   new Error(ex.Message, request.Verb.ToString()) 
                };
            }

            return output;
        }      

        public async Task<ResponseMessage<TOutput>> InvokeAsync<TOutput>(RequestMessage request)
        {
            HttpResponseMessage response = null;
            ResponseMessage<TOutput> output = new ResponseMessage<TOutput>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (this._serviceContext == null)
                    {
                        return null;
                    }

                    string requestContent = string.Empty;
                    HttpContent httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, request.UrlBuilder.GetUrl());
                    // skip the autorization for now
                    httpRequestMessage.Headers.Add("IgnoreAuthValue", "true");
                    //var accessToken = "JWTaccessToken";//settingProvier.GetValue<ServiceContext>(SessionConstants.ServiceContext);.
                    //httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken); httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    response = await httpClient.SendAsync(httpRequestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    output.Content = JsonConvert.DeserializeObject<TOutput>(responseContent);
                }
            }
            catch (Exception ex)
            {
                output = new ResponseMessage<TOutput>();
                output.StatusCode = response.StatusCode;
                output.Errors = new List<Error>() 
                 {
                     new Error(ex.Message, request.Verb.ToString()) 
                 };
            }

            return output;

        }

        public async Task<ResponseMessage<TOutput>> InvokeAsync<TInput, TOutput>(RequestMessage<TInput> request)
        {
            HttpResponseMessage response = null;
            ResponseMessage<TOutput> output = new ResponseMessage<TOutput>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (_serviceContext == null)
                    {
                        return null;
                    }

                    string requestContent = string.Empty;
                    HttpContent httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");
                    HttpRequestMessage httpRequestMessage = new HttpRequestMessage(request.Verb, string.Format("{0}{1}", _serviceContext.ResourceURL, request.UrlBuilder.GetUrl()));
                    // skip the autorization for now
                    httpRequestMessage.Headers.Add("IgnoreAuthValue", "true");
                    //var accessToken = "JWTaccessToken";//settingProvier.GetValue<ServiceContext>(SessionConstants.ServiceContext);.
                    //httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpRequestMessage.Content = request.Verb == HttpMethod.Get ? null : httpContent;
                    response = await httpClient.SendAsync(httpRequestMessage);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    output.Content = JsonConvert.DeserializeObject<TOutput>(responseContent);
                }
            }
            catch (Exception ex)
            {
                output = new ResponseMessage<TOutput>();
                output.StatusCode = response.StatusCode;
                output.Errors = new List<Error>() 
                 {
                     new Error(ex.Message, request.Verb.ToString()) 
                 };
            }

            return output;
        }
    }
}
