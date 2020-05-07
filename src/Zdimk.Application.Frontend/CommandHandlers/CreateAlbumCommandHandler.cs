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
    public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, AlbumDto>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CreateAlbumCommandHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<AlbumDto> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.CreateAlbum);
            var authHeader = await _localStorage.GetAuthHeaderValueAsync();

            return await _httpClient.PostAsJsonAsync<CreateAlbumCommand, AlbumDto>(requestUrl, request,
                authHeader);
        }
    }
}