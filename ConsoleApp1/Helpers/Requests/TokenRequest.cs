using System.Collections.Generic;
using System.Net.Http;
using BritishCarAuctions.ApiClient;

namespace Just4Fun.ConsoleApp1.Helpers.Requests
{
    public class TokenRequest : ApiRequest<TokenResponse>
    {
        public TokenRequest(string url, string clientId, string clientSecret, string scope, string grantType, string username, string password)
        {
            HttpRequest = new HttpRequestMessage(HttpMethod.Post, url);

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("client_id", clientId));
            keyValues.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            keyValues.Add(new KeyValuePair<string, string>("grant_type", grantType));
            keyValues.Add(new KeyValuePair<string, string>("scope", scope));
            keyValues.Add(new KeyValuePair<string, string>("username", username));
            keyValues.Add(new KeyValuePair<string, string>("password", password));

            HttpRequest.Content = new FormUrlEncodedContent(keyValues);

            ShouldAddAuthorization = false;
        }
    }
}