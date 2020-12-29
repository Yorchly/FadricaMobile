using FadricaMobile.api.models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FadricaMobile.api.wrappers
{
    class RosconTypeWrapper
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string tipoRosconUrl = "api/tipo-roscon/";

        public RosconTypeWrapper()
        {
            client.BaseAddress = new Uri(Constants.BaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<RosconType>> GetAllRosconTypeAsync()
        {
            HttpResponseMessage response = await client.GetAsync($"{tipoRosconUrl}?token={Constants.Token}");
            List<RosconType> rosconTypes = null;

            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                rosconTypes = JsonConvert.DeserializeObject<List<RosconType>>(resp);
            }

            return rosconTypes;
        }
    }
}
