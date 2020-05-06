using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorInputFile;
using Microsoft.Extensions.Options;
using Zdimk.BlazorApp.Abstractions;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Commands;
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

        public async Task<PictureDto> UploadPicture(CreatePictureCommand command)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.CreateImage);

            string token = await _localStore.GetItemAsync<string>(_securityOptions.AccessTokenName);
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);

            MultipartFormDataContent form = new MultipartFormDataContent();

            byte[] fileBytes;

            using (MemoryStream stream = await command.PictureFile.ReadAllAsync())
            {
                fileBytes = stream.ToArray();
            }

            form.Add(new StringContent(command.AlbumId.ToString()), nameof(command.AlbumId).ToLower());
            form.Add(new StringContent(command.Name), nameof(command.Name).ToLower());
            form.Add(new StringContent(command.Description + "111"), nameof(command.Description).ToLower());
            form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), nameof(command.PictureFile),
                command.PictureFile.Name);

            HttpRequestMessage request = CreateRequestMessage(requestUrl, form, authHeader);
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<PictureDto>(await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        

        public async Task<bool> CreateAlbum(CreateAlbumCommand command)
        {
            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.CreateAlbum);

            string token = await _localStore.GetItemAsync<string>(_securityOptions.AccessTokenName);
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", token);

            AlbumDto result = await _httpClient.PostAsJsonAsync<CreateAlbumCommand, AlbumDto>(requestUrl, command,
                authHeader);

            return result != null;
        }

        private static HttpRequestMessage CreateRequestMessage(Uri requestUrl, MultipartContent multipart,
            AuthenticationHeaderValue authHeaderValue = null)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            if (authHeaderValue != null)
                requestMessage.Headers.Authorization = authHeaderValue;

            requestMessage.Content = multipart;

            return requestMessage;
        }
    }
}