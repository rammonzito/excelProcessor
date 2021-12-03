using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel
{
    public class Sender
    {
        public async Task<T> SendAsync<T>(string endpoint, object content, HttpMethod method, string token = null)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            var request = new HttpRequestMessage(method, $"{endpoint}")
            {
                Content = new StringContent(JsonConvert.SerializeObject(content ?? new { }, settings), Encoding.UTF8, "application/json"),
            };

            using (var client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                using (var response = await client.SendAsync(request))
                {
                    var stringJson = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<T>(stringJson);
                    }
                    else
                    {
                        throw new NotFoundException();
                    }
                }
            }
        }
    }
}
