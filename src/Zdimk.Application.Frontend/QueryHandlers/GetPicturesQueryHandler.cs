using System;
using System.Collections;
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
    public class GetPicturesQueryHandler : IRequestHandler<GetPicturesQuery, IEnumerable<PictureDto>>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public GetPicturesQueryHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<IEnumerable<PictureDto>> Handle(GetPicturesQuery request, CancellationToken cancellationToken)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetPicturesUrl);
            AuthenticationHeaderValue authHeader = await _localStorage.GetAuthHeaderValueAsync();

            return await _httpClient.PostAsJsonAsync<GetPicturesQuery, IEnumerable<PictureDto>>(requestUrl, request,
                authHeader);
        }
    }
}