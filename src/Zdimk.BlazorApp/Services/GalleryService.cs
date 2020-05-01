using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Queries;
using Zdimk.BlazorApp.Extensions;
using Zdimk.BlazorApp.Services.Configuration;

namespace Zdimk.BlazorApp.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStore;
        private readonly SecurityOptions _securityOptions;

        public GalleryService(HttpClient httpClient, ILocalStorageService localStore, IOptions<SecurityOptions> options)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStore = localStore ?? throw new ArgumentNullException(nameof(localStore));
            _securityOptions = options.Value;
        }

        public async Task<ICollection<AlbumDto>> GetAlbumsAsync(GetAlbumsQuery query)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetAlbumsUrl);
 
            string token = await _localStore.GetItemAsync<string>(_securityOptions.AccessTokenName);
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.PostAsJsonAsync<GetAlbumsQuery, ICollection<AlbumDto>>(requestUrl, query,
                authHeader);
        }

        public async Task<ICollection<PictureDto>> GetPicturesAsync(GetPicturesQuery query)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.GetPicturesUrl);

            string token = await _localStore.GetItemAsync<string>(_securityOptions.AccessTokenName);
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);

            return await _httpClient.PostAsJsonAsync<GetPicturesQuery, ICollection<PictureDto>>(requestUrl, query,
                authHeader);
        }
    }
}