using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Zdimk.Application.Implementations.Configuration;
using Zdimk.Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Zdimk.Application.Implementations
{
    public class PictureService : IPictureService
    {
        private const int SmallImageWidth = 512;

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
            string bigOutputFilePath = Path.Combine("wwwroot", _options.PictureFolderName, pictureId + fileExtension);
            string smallOutputFilePath =
                Path.Combine("wwwroot", _options.PictureFolderName, pictureId + "-small" + ".jpg");

            Image img = Image.Load(source);
            img.Mutate(x => x.Resize(SmallImageWidth, (int) (512.0 * img.Height / img.Width)));
            img.Save(smallOutputFilePath, new JpegEncoder { Quality = 70 });

            source.Position = 0;
            using (Stream output = File.OpenWrite(bigOutputFilePath))
                await source.CopyToAsync(output);
        }

        public string GetBigPictureUrl(Guid pictureId, string fileExtension)
        {
            FixFileExtension(ref fileExtension);
            return _baseUrl + "/" + _options.PictureFolderName + "/" + pictureId + fileExtension;
        }

        public string GetSmallPictureUrl(Guid pictureId, string fileExtension)
        {
            FixFileExtension(ref fileExtension);
            return _baseUrl + "/" + _options.PictureFolderName + "/" + pictureId + "-small" + ".jpg";
        }

        public void DeletePicture(Guid pictureId, string fileExtension)
        {
            string bigPictureFile = Path.Combine("wwwroot", _options.PictureFolderName, pictureId + fileExtension);
            string smallPictureFile =
                Path.Combine("wwwroot", _options.PictureFolderName, pictureId + "-small" + ".jpg");
            
            File.Delete(smallPictureFile);
            File.Delete(bigPictureFile);
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