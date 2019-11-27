using BritishCarAuctions.ApiClient.OAuth;
using Newtonsoft.Json;

namespace Just4Fun.ConsoleApp1.Helpers
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        public OAuthToken Token => new OAuthToken { Type = TokenType, Value = AccessToken };
    }
}
