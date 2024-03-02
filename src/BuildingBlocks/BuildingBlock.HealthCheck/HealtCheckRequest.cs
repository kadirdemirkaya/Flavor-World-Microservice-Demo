using System.Net.Http.Headers;

namespace BuildingBlock.HealthCheck
{
    public static class HealtCheckRequest
    {
        public static async Task<bool> CheckUrl(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("/health");
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }
    }
}
