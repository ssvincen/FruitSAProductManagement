using FruitSA.Web.Helper;
using FruitSA.Web.Models.Account;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

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
            var deserializedResponse = await HandleResponseAsync<T>(response, url);
            return deserializedResponse;
        }

        public async Task<T> PutAsync<T>(string relativeUrl, object data, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);
            var url = BuildUrl(relativeUrl);
            var response = await _httpClient.PutAsJsonAsync(url, data);
            var deserializedResponse = await HandleResponseAsync<T>(response, url);
            return deserializedResponse;
        }

        public async Task<T> DeleteAsync<T>(string relativeUrl, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);
            var url = BuildUrl(relativeUrl);
            var response = await _httpClient.DeleteAsync(url);
            var deserializedResponse = await HandleResponseAsync<T>(response, url);
            return deserializedResponse;
        }

        public async Task<T> GetAsync<T>(string relativeUrl, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);
            var url = BuildUrl(relativeUrl);
            var response = await _httpClient.GetAsync(url);
            var deserializedResponse = await HandleResponseAsync<T>(response, url);
            return deserializedResponse;
        }

        public async Task<T> PostMultipartAsync<T>(string relativeUrl, MultipartFormDataContent content, bool withAuth = true)
        {
            ApplyJwtIfRequired(withAuth);
            var url = BuildUrl(relativeUrl);
            var response = await _httpClient.PostAsync(url, content);
            var deserializedResponse = await HandleResponseAsync<T>(response, url);
            return deserializedResponse;
        }


        private static async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, string url)
        {
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<T>(content, options);
                if (result == null)
                {
                    throw new InvalidOperationException("The response content could not be deserialized.");
                }

                return result;
            }

            // Handle 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (typeof(T) == typeof(JwtResponse))
                {
                    // Create a JwtResponse object even if failed, to return a consistent type
                    var errorResponse = new JwtResponse
                    {
                        Success = false,
                        Message = content,
                        Token = null,
                        Expiration = DateTime.MinValue
                    };
                    return (T)(object)errorResponse;
                }
                else if (typeof(T) == typeof(ApiResponse))
                {
                    return (T)(object)new ApiResponse
                    {
                        Success = false,
                        Message = "Unauthorized access. Please login again."
                    };
                }
            }

            // Handle other API error responses and attempt to deserialize to T
            try
            {
                var errorResult = JsonSerializer.Deserialize<T>(content, options);
                if (errorResult != null)
                {
                    return errorResult;
                }
            }
            catch
            {
                // Fallback to basic ApiResponse if deserialization fails
            }

            if (typeof(T) == typeof(ApiResponse))
            {
                return (T)(object)new ApiResponse
                {
                    Success = false,
                    Message = content
                };
            }

            throw new HttpRequestException($"Request to '{url}' failed with status code {(int)response.StatusCode}: {content}");
        }



        public void StoreToken(string token)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("JWToken", token);
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
