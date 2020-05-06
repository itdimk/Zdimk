using System;
using System.IO;
using System.Security.Policy;
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

        public async Task SaveToContentFolderAsync(Stream source, Guid pictureId, string fileExtension)
        {
            FixFileExtension(ref fileExtension);
            string outputFilePath = Path.Combine("wwwroot", _options.PictureFolderName, pictureId + fileExtension);

            using (FileStream output = new FileStream(outputFilePath, FileMode.Create))// File.OpenWrite(outputFilePath))
                await source.CopyToAsync(output);
        }

        public string GetPictureUrl(Guid pictureId, string fileExtension)
        {
            FixFileExtension(ref fileExtension);
            return  _baseUrl + "/" + _options.PictureFolderName + "/" + pictureId + fileExtension;
        }

        private string GetBaseUrl(IHttpContextAccessor contextAccessor)
        {
            var request = contextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}/";
        }

        private void FixFileExtension(ref string fileExtension)
        {
            if (fileExtension[0] != '.')
                fileExtension = fileExtension.Insert(0, ".");
        }
    }
}