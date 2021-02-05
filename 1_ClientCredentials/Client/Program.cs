using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Console.WriteLine("Hello World!");

            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:6000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResp = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
            });

            if (tokenResp.IsError)
            {
                Console.WriteLine(tokenResp.Error);
                return;
            }

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResp.AccessToken);
            var contentResp = await apiClient.GetAsync("http://localhost:5000/api/order");
            if (contentResp.IsSuccessStatusCode)
            {
                var content = await contentResp.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            else
            {
                Console.WriteLine(contentResp.StatusCode);
            }

            Console.ReadLine();

        }
    }
}
