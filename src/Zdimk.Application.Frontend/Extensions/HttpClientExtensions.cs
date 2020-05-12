using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zdimk.Application.Frontend.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync<TRequest>(this HttpClient client, Uri requestUrl,
            TRequest requestModel,
            AuthenticationHeaderValue authHeaderValue = null)
            where TRequest : class
        {
            HttpRequestMessage request = CreateRequestMessage(requestUrl, requestModel, authHeaderValue);
            return await client.SendAsync(request);
        }

        public static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(this HttpClient client, Uri requestUrl,
            TRequest requestModel, AuthenticationHeaderValue authHeaderValue = null)
            where TRequest : class
            where TResponse : class
        {
            HttpRequestMessage request = CreateRequestMessage(requestUrl, requestModel, authHeaderValue);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                if (typeof(TResponse) == typeof(string))
                    return responseString as TResponse;

                return JsonSerializer.Deserialize<TResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
            {
                return null;
            }
        }

        private static HttpRequestMessage CreateRequestMessage<TRequest>(Uri requestUrl,
            TRequest requestModel, AuthenticationHeaderValue authHeaderValue = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (authHeaderValue != null)
                requestMessage.Headers.Authorization = authHeaderValue;

            string content = JsonSerializer.Serialize(requestModel,
                new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            requestMessage.Content = new StringContent(content);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json);

            return requestMessage;
        }
    }
}