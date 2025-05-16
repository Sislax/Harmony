using System.Net.Http.Json;
using Harmony.UI.Models.DTOs;

namespace Harmony.UI.Services
{
    public class ServerService
    {
        private readonly IHttpClientFactory _factory;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;

        public ServerService(IHttpClientFactory factory, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _factory = factory;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
        }

        public async Task<List<ServerDTO>?> GetServersByUser()
        {
            HttpResponseMessage response = await _factory.CreateClient("HarmonyAPI")
                .GetAsync("api/server/getServersByUser");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ServerDTO>>();
            }

            return null;
        }
    }
}
