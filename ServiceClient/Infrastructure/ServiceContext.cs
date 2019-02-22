using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient
{
    public class ServiceContext
    {
        private string _authority = string.Empty;

        public ServiceContext()
        {

        }

        public ServiceContext(string azureAdInstance, string tenantId, string clientId, Uri redirectUri, string addIdUri, string resourceUrl)
        {
            this.AzureADInstance = azureAdInstance;
            this.TenantID = tenantId;
            this.ClientID = clientId;
            this.RedirectURI = redirectUri;
            this.AppIdURI = addIdUri;
            this.ResourceURL = resourceUrl;
        }

        public string AzureADInstance { get; set; }

        public string TenantID { get; set; }

        //fields required from native application hosted in azure AD
        //and having access to permissions delegated to respective Web API application 
        //hosted in azure AD
        public string ClientID { get; set; }

        public string Authority
        {
            get
            {
                if (!string.IsNullOrEmpty(this.AzureADInstance) && !string.IsNullOrEmpty(this.TenantID))
                {
                    _authority = string.Format(CultureInfo.InvariantCulture, AzureADInstance, TenantID);
                }
                return _authority;
            }
        }

        public Uri RedirectURI { get; set; }

        // fields required from web API application hosted azure AD
        public string AppIdURI { get; set; }

        /// <summary>
        /// Web API Url in real case SignOn Url/
        /// </summary>
        public string ResourceURL { get; set; }

        public ServiceContextScope Scope { get; set; }
    }

    public enum ServiceContextScope
    {
        ULS,
        Profile
    }
}
