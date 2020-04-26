using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Zdimk.Application.Interfaces;
using Zdimk.Services.Configuration;

namespace Zdimk.Services
{
    public class PictureService : IPictureService
    {
        private readonly string _baseUrl;
        private readonly PictureServiceOptions _options;

        public PictureService(IHttpContextAccessor httpContext, IOptions<PictureServiceOptions> options)
        {
            _baseUrl = GetBaseUrl(httpContext);
            _options = options.Value;
        }

        public async Task SaveToContentFolderAsync(Stream source, string uniqueFileName)
        {
            string outputFilePath = Path.Combine(_options.PictureFolderName, uniqueFileName);

            using (Stream output = File.OpenWrite(outputFilePath))
                await source.CopyToAsync(output);
        }

        public string GetPictureUrl(string uniqueFileName)
        {
            return Path.Combine(_baseUrl, _options.PictureFolderName, uniqueFileName);
        }

        private string GetBaseUrl(IHttpContextAccessor contextAccessor)
        {
            var request = contextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}/";
        }
    }
}