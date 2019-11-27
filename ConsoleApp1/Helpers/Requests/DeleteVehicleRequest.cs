using System;
using System.Net.Http;
using System.Net.Http.Headers;
using BritishCarAuctions.ApiClient;
using BritishCarAuctions.ApiClient.Data;

namespace Just4Fun.ConsoleApp1.Helpers.Requests
{
    public class DeleteVehicleRequest : ApiRequest<VoidResponse>
    {
        public DeleteVehicleRequest(string baseUrl, string vehicleId)
        {
            HttpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri($"{baseUrl}/api/v1/Vehicle/delete/{vehicleId}", UriKind.Absolute),
                Method = HttpMethod.Delete
            };

            HttpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
        }
    }
}