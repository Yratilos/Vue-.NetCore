using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApi.IUtils;

namespace WebApi.Utils
{
    public class HttpUtil : IHttpUtil
    {

        public async Task<string> Post(string url, object obj)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(obj);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
        }

        public async Task<string> Get(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
        }

        public async Task<string> Put(string url, object obj)
        {
            using (HttpClient client = new HttpClient())
            {
                string jsonData = JsonConvert.SerializeObject(obj); var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
        }

        public async Task<string> Delete(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    return "Error: " + response.StatusCode;
                }
            }
        }
    }
}
