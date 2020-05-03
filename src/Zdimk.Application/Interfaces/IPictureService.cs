using System;
using System.IO;
using System.Threading.Tasks;

namespace Zdimk.Application.Interfaces
{
    public interface IPictureService
    {
        Task SaveToContentFolderAsync(Stream source, Guid pictureId, string fileExtension);    
        string GetPictureUrl(Guid pictureId, string fileExtension);
    }
}