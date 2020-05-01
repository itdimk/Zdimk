using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zdimk.BlazorApp.Extensions
{
    public static class HttpClientExtensions
    {
        public static async void PostAsJsonAsync<TRequest>(this HttpClient client, Uri requestUrl,
            TRequest requestModel,
            AuthenticationHeaderValue authHeaderValue = null)
            where TRequest : class
        {
            HttpRequestMessage request = await CreateRequestMessage(requestUrl, requestModel, authHeaderValue);
            await client.SendAsync(request);
        }

        public static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(this HttpClient client, Uri requestUrl,
            TRequest requestModel, AuthenticationHeaderValue authHeaderValue = null)
            where TRequest : class
            where TResponse : class
        {
            HttpRequestMessage request = await CreateRequestMessage(requestUrl, requestModel, authHeaderValue);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK) return null;

            await using (Stream responseStream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<TResponse>(responseStream,
                    new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
            }
        }

        private static async Task<HttpRequestMessage> CreateRequestMessage<TRequest>(Uri requestUrl,
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