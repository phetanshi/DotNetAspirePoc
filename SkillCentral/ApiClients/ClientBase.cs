using SkillCentral.Dtos;

namespace SkillCentral.ApiClients
{
    public abstract class ClientBase(HttpClient http, ILogger logger)
    {
        protected HttpClient Http { get; } = http;
        protected ILogger Logger { get; } = logger;

        protected async Task<List<T>> GetListAsync<T>(string uri) where T : class
        {
            var response = await Http.GetAsync(uri);
            var data = await response.Content.ReadFromJsonAsync<ApiResponse<List<T>>>();
            return data?.Payload ?? new List<T>();
        }

        protected async Task<T> GetAsync<T>(string uri) where T : class
        {
            var response = await http.GetAsync(uri);
            var data = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
            return data?.Payload ?? default;
        }

        protected async Task<T> ReadPostResponseAsync<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
                if (!result.IsSuccess)
                    throw new Exception("Something went wrong while creating a new skill");
                return result.Payload;
            }
            else
            {
                var result = (await response?.Content?.ReadFromJsonAsync<ApiResponse<T>>())?.Message ?? $"Something went wrong while creating of type {typeof(T).Name}. API call returned error code.";
                throw new Exception(result);
            }
        }
    }
}
