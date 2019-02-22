using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure
{
    public class URLBuilder
    {
        private IDictionary<string, string> _resourceList;
        private IDictionary<string, string> _queryParameters;
        private string _absoluteUrl;

        public URLBuilder()
        {
            this._resourceList = new Dictionary<string, string>(); // could be someother type incase of having duplicate resources.
            this._queryParameters = new Dictionary<string, string>();
            this._absoluteUrl = string.Empty;
        }

        public string AbsoluteURL
        {
            get { return this._absoluteUrl; }
            set { this._absoluteUrl = value; }
        }
        public void AddResource(string resource)
        {
            this._resourceList.Add(resource, string.Empty);
        }
        public void AddResource(string resource, string value)
        {
            this._resourceList.Add(resource, value);
        }
        public void AddQueryParameter(string parameterName, string paramterValue)
        {
            this._queryParameters.Add(parameterName, paramterValue);
        }
        public void AddRelativeUrl(string url)
        {
            this._absoluteUrl = url;
        }
        public string GetUrl()
        {
            if (!string.IsNullOrEmpty(this._absoluteUrl))
            {
                return this._absoluteUrl;
            }

            foreach (var item in this._resourceList)
            {
                this._absoluteUrl = string.Format("{0}/{1}", this._absoluteUrl, item.Key);
                if (!string.IsNullOrEmpty(item.Value))
                {
                    this._absoluteUrl = string.Format("{0}/{1}", this._absoluteUrl, item.Value);
                }
            }

            string query = string.Empty;
            foreach (var item in this._queryParameters)
            {
                query = string.Concat(query, string.Format("&{0}={1}", item.Key, WebUtility.UrlEncode(item.Value)));
            }

            this._absoluteUrl = this._absoluteUrl.Trim('/');
            if (!string.IsNullOrEmpty(this._absoluteUrl) && !string.IsNullOrEmpty(query))
            {
                this._absoluteUrl = string.Format("{0}/?{1}", this._absoluteUrl.Trim('/'), query.Trim('&'));
            }
            return this._absoluteUrl;
        }
    }
}
