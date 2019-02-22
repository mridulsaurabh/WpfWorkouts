using Infrastructure.Utility;
using ServiceClient.Entities;
using ServiceClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure
{
    public class Proxy : IProxy
    {
        private ServiceContext _serviceContext;
        public Proxy()
        {

        }

        public async Task<LateReasonCodes> GetLateReasonCodes()
        {
            RequestMessage reqMessage = new RequestMessage() { RequestId = new Guid(), Verb = HttpMethod.Get };
            reqMessage.UrlBuilder.AddRelativeUrl(SessionConstants.LateReasonCodesLink);
            var serviceBus = new ServiceBus(this._serviceContext);
            var output = await serviceBus.InvokeAsync<LateReasonCodes>(reqMessage);
            return output.Content;
        }

        public async Task<AdjustmentReasonCodes> GetAdjustmentReasonCodes()
        {
            RequestMessage<AdjustmentReasonCodes> reqMessage = new RequestMessage<AdjustmentReasonCodes>();
            var serviceBus = new ServiceBus(this._serviceContext);
            var output = await serviceBus.InvokeAsync<AdjustmentReasonCodes, AdjustmentReasonCodes>(reqMessage);
            return output.Content;
        }

        public async Task<RejectionReasonCodes> GetRejectionReasonCodes()
        {
            RequestMessage<RejectionReasonCodes> reqMessage = new RequestMessage<RejectionReasonCodes>();
            var serviceBus = new ServiceBus(this._serviceContext);
            var output = await serviceBus.InvokeAsync<RejectionReasonCodes, RejectionReasonCodes>(reqMessage);
            return output.Content;
        }

        public async Task<LaborCategories> GetLateCategories()
        {
            RequestMessage<LaborCategories> reqMessage = new RequestMessage<LaborCategories>();
            var serviceBus = new ServiceBus(this._serviceContext);
            var output = await serviceBus.InvokeAsync<LaborCategories, LaborCategories>(reqMessage);
            return output.Content;
        }

        public async Task<TimeZones> GetTimeZones()
        {
            RequestMessage<TimeZones> reqMessage = new RequestMessage<TimeZones>();
            reqMessage.UrlBuilder.AddResource("v-mrverm");
            reqMessage.UrlBuilder.AddQueryParameter("SessionConstantId", "0");
            var serviceBus = new ServiceBus(this._serviceContext);
            var output = await serviceBus.InvokeAsync<TimeZones, TimeZones>(reqMessage);
            return output.Content;
        }
    }
}
