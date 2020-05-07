using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MediatR;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Frontend.Constants;
using Zdimk.Application.Frontend.Extensions;

namespace Zdimk.Application.Frontend.QueryHandlers
{
    public class GetAccessTokenQueryHandler : IRequestHandler<GetAccessTokenQuery, string>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public GetAccessTokenQueryHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<string> Handle(GetAccessTokenQuery request, CancellationToken cancellationToken)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetAccessTokenUrl);
            var authHeader = await _localStorage.GetAuthHeaderValueAsync();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, request, authHeader);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}