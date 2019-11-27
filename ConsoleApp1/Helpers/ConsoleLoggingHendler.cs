using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Just4Fun.ConsoleApp1.Tests
{
    public class ConsoleLoggingHandler : DelegatingHandler
    {
        public ConsoleLoggingHandler(HttpMessageHandler httpMessageHandler) : base(httpMessageHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                Console.WriteLine($"Request: {request}\nRequest Body: {requestBody}");
            }
            catch
            {
                Console.WriteLine($"Request: {request}");
            }

            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {response}\nResponse Body: {responseBody}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get response: {ex}");
                throw ex;
            }
        }
    }
}
