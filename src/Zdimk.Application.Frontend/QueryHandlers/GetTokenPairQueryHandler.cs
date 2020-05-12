using System;
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
    public class GetTokenPairQueryHandler : IRequestHandler<GetTokenPairQuery, JwtTokenPair>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public GetTokenPairQueryHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        
        public async Task<JwtTokenPair> Handle(GetTokenPairQuery request, CancellationToken cancellationToken)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetTokenPairUrl); // TODO: postjsonasync add out parameter
            return await _httpClient.PostAsJsonAsync<GetTokenPairQuery, JwtTokenPair>(requestUrl, request);
        }
    }
}