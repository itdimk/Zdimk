using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MediatR;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Application.Frontend.Constants;
using Zdimk.Application.Frontend.Extensions;

namespace Zdimk.Application.Frontend.CommandHandlers
{
    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public DeleteAlbumCommandHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.DeleteAlbum);
            var authHeader = await _localStorage.GetAuthHeaderValueAsync();

            var response = await _httpClient.PostAsJsonAsync(requestUrl, request,
                authHeader);

            response.EnsureSuccessStatusCode();
            return Unit.Value;
        }
    }
}