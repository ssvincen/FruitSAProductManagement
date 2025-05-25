using FruitSA.Web.Helper;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace FruitSA.Web.Services
{
    public class ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> options)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly AppSettings _appSettings = options.Value;


        public async Task<T> PostAsync<T>(string relativeUrl, object data, bool withAuth = false)
        {
            ApplyJwtIfRequired(withAuth);

            var url = BuildUrl(relativeUrl);
            var response = await _httpClient.PostAsJsonAsync(url, data);
            var errorContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadFromJsonAsync<T>();
            return responseContent;
        }

        public async Task<T> GetAsync<T>(string relativeUrl, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);

            var response = await _httpClient.GetAsync(BuildUrl(relativeUrl));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> DeleteAsync<T>(string relativeUrl, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);

            var response = await _httpClient.DeleteAsync(BuildUrl(relativeUrl));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T> PutAsync<T>(string relativeUrl, object data, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);
            var response = await _httpClient.PutAsJsonAsync(BuildUrl(relativeUrl), data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }


        public async Task<T> PostMultipartAsync<T>(string relativeUrl, MultipartFormDataContent content, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);

            var url = BuildUrl(relativeUrl);
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Upload failed: {response.StatusCode} - {error}");
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }


        public async Task<T> PatchAsync<T>(string relativeUrl, object data, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);
            var response = await _httpClient.PatchAsJsonAsync(BuildUrl(relativeUrl), data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public void StoreToken(string token)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("JWToken", token);
        }

        public string GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
        }

        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("JWToken");
        }

        private void ApplyJwtIfRequired(bool withAuth)
        {
            if (withAuth)
            {
                var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        private string BuildUrl(string endpoint)
        {
            return _appSettings.ApiBaseUrl.TrimEnd('/') + "/" + endpoint.TrimStart('/');
        }
    }
}
