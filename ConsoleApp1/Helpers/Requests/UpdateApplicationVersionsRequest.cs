using System;
using System.Net.Http;
using BritishCarAuctions.ApiClient;
using BritishCarAuctions.ApiClient.Data;
using BritishCarAuctions.ApiClient.Requests;
using BritishCarAuctions.DealerProApp.Api.Data;
using BritishCarAuctions.DealerProApp.Api.Requests;
using Newtonsoft.Json;

namespace Just4Fun.ConsoleApp1.Helpers.Requests
{
    public class UpdateApplicationVersionsRequest : ApiRequest<VoidResponse>, IResolvableRequest
    {
        public UpdateApplicationVersionsRequest(AppVersions appVersions)
        {
            HttpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                Content = new StringContent(JsonConvert.SerializeObject(appVersions), System.Text.Encoding.UTF8, "application/json")
            };
        }

        string IResolvableRequest.LinkKey => "AppVersion";

        public string DiscoveryApiLinkKey => DiscoveryApiKeys.DealerProDiscoveryApiKey;

        public void SetLink(Link link)
        {
            HttpRequest.RequestUri = new Uri(link.Href.Replace("{OSName}", string.Empty));
        }
    }
}