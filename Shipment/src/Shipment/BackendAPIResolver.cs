using System.Web.Mvc;
using Newtonsoft.Json;

namespace Shipment
{
    public class BackendAPIResolver<T>
    {
        public async Task<T> GetRequest(string URL)
        {
            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, errors) =>
                            {
                                return true;
                            };

            using (var client = new HttpClient(handler))
            {
                var backendAPIPortNumber = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Port"];



                client.BaseAddress = new Uri($"https://localhost:{backendAPIPortNumber}{URL}");
                var response = await client.GetAsync($"");
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync();
                T outputObject = JsonConvert.DeserializeObject<T>(stringResult);

                return outputObject;
            }
        }

        public async Task<HttpResponseMessage> PostRequest(string objectName, string URL, T objectToSend)
        {
            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, errors) =>
                            {
                                return true;
                            };

            using (var client = new HttpClient(handler))
            {
                var backendAPIPortNumber = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Port"];
                client.BaseAddress = new Uri($"https://localhost:{backendAPIPortNumber}{URL}");

                HttpResponseMessage response = await client.PutAsJsonAsync(objectName, objectToSend);
                response.EnsureSuccessStatusCode();

                return response;
            }
        }
    }

}