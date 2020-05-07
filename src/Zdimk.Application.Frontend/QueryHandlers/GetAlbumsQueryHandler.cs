using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public GetAlbumsQueryHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<IEnumerable<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetAlbumsUrl);
            AuthenticationHeaderValue authHeader = await _localStorage.GetAuthHeaderValueAsync();

            return await _httpClient.PostAsJsonAsync<GetAlbumsQuery, ICollection<AlbumDto>>(requestUrl, request,
                authHeader);
        }
    }
}