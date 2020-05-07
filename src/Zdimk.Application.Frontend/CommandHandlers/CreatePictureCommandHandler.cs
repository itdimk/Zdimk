using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Http;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Application.Frontend.Constants;
using Zdimk.Application.Frontend.Extensions;

namespace Zdimk.Application.Frontend.CommandHandlers
{
    public class CreatePictureCommandHandler : IRequestHandler<CreatePictureCommand, PictureDto>
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public CreatePictureCommandHandler(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<PictureDto> Handle(CreatePictureCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Uri requestUrl = new Uri(_httpClient.BaseAddress, ApiConstants.CreateImage);
            AuthenticationHeaderValue authHeader = await _localStorage.GetAuthHeaderValueAsync();

            var multipart = await CreateMultipartFormDataContent(request, cancellationToken);

            HttpRequestMessage requestMessage = CreateRequestMessage(requestUrl, multipart, null);
            HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);

            responseMessage.EnsureSuccessStatusCode();

            return await DeserializeResponseAsync(responseMessage);
        }

        private async Task<MultipartFormDataContent> CreateMultipartFormDataContent(CreatePictureCommand request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            byte[] fileBytes = await GetBytesAsync(request.PictureFile, cancellationToken);

            return new MultipartFormDataContent
            {
                {
                    new StringContent(request.AlbumId.ToString()), nameof(request.AlbumId).ToLower()
                },
                {
                    new StringContent(request.Name), nameof(request.Name).ToLower()
                },
                {
                    new StringContent(request.Description), nameof(request.Description).ToLower()
                },
                {
                    new ByteArrayContent(fileBytes, 0, fileBytes.Length), nameof(request.PictureFile),
                    request.PictureFile.Name
                }
            };
        }

        private async Task<byte[]> GetBytesAsync(IFormFile file, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (file.Length > int.MaxValue)
                throw new NotSupportedException($"File size is bigger then {int.MaxValue} bytes");

            byte[] bytes = new byte[file.Length];
            using (var stream = new MemoryStream(bytes))
                await file.CopyToAsync(stream);

            return bytes;
        }

        private HttpRequestMessage CreateRequestMessage(Uri requestUrl, MultipartContent multipart,
            AuthenticationHeaderValue authHeaderValue)
        {
            if (authHeaderValue == null)
                throw new ArgumentNullException(nameof(authHeaderValue));

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            requestMessage.Headers.Authorization = authHeaderValue;
            requestMessage.Content = multipart;

            return requestMessage;
        }

        private async Task<PictureDto> DeserializeResponseAsync(HttpResponseMessage response)
        {
            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PictureDto>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}