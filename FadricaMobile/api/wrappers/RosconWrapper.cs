using FadricaMobile.api.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FadricaMobile.api.wrappers
{
    class RosconWrapper
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string rosconUrl = "api/roscones/";

        public RosconWrapper()
        {
            client.BaseAddress = new Uri(Constants.BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Roscon>> GetAllRosconesAsync(int? year)
        {
            if (!year.HasValue)
                year = DateTime.Now.Year;
            HttpResponseMessage response = await client.GetAsync($"{rosconUrl}?token={Constants.Token}&year={year}");
            List<Roscon> roscones = null;

            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                roscones = JsonConvert.DeserializeObject<List<Roscon>>(resp);
            }

            return roscones;
        }

        public async Task<bool> CreateRoscon(Roscon roscon)
        {
            string json = JsonConvert.SerializeObject(roscon, Formatting.Indented);
            var content = new StringContent(json.ToLower(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync($"{rosconUrl}?token={Constants.Token}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<Roscon> UpdateRoscon(Roscon roscon)
        {
            string resp;
            string json = JsonConvert.SerializeObject(roscon, Formatting.Indented);
            var content = new StringContent(json.ToLower(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"{rosconUrl}{roscon.Id}/?token={Constants.Token}", content);

            response.EnsureSuccessStatusCode();

            resp = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Roscon>(resp);
        }
    }
}
