using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MediatR;
using Zdimk.Abstractions.Commands;
using Zdimk.Application.Frontend.Constants;
using Zdimk.Application.Frontend.Extensions;

namespace Zdimk.Application.Frontend.CommandHandlers
{
    public class ActivateRefreshTokenCommandHandler : IRequestHandler<ActivateRefreshTokenCommand>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ActivateRefreshTokenCommandHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<Unit> Handle(ActivateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var authHeader = await _localStorage.GetAuthHeaderValueAsync();
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.ActivateRefreshToken);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(requestUrl, request, authHeader);

            response.EnsureSuccessStatusCode();
            return Unit.Value;
        }
    }
}