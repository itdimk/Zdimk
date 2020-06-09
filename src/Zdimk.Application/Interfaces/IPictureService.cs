using System;
using System.IO;
using System.Threading.Tasks;

namespace Zdimk.Application.Interfaces
{
    public interface IPictureService
    {
        Task SaveToContentFolderAsync(Stream source, Guid pictureId, string fileExtension);    
        string GetBigPictureUrl(Guid pictureId, string fileExtension);
        string GetSmallPictureUrl(Guid pictureId, string fileExtension);
        void DeletePicture(Guid pictureId, string fileExtension);
    }
}