using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs.User;
using Newtonsoft.Json;

namespace Boilerplate.Api.IntegrationTests.Helpers
{
    public static class HttpHelper
    {
        public static async Task<T> DeserializeContent<T>(this HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        public static StringContent GetStringContent<T>(this T obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        public static void UpdateBearerToken(this HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }
    }
}
