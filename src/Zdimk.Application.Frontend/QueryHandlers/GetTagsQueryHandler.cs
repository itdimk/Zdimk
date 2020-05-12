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
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<TagDto>>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStore;

        public GetTagsQueryHandler(HttpClient httpClient, ILocalStorageService localStore)
        {
            _httpClient = httpClient;
            _localStore = localStore;
        }
        
        public async Task<IEnumerable<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetTagsUrl);
            AuthenticationHeaderValue authHeader = await _localStore.GetAuthHeaderValueAsync();

            return await _httpClient.PostAsJsonAsync<GetTagsQuery, IEnumerable<TagDto>>(requestUrl, request,
                authHeader);
        }
    }
}