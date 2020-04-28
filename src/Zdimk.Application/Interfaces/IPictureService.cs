using System;
using System.IO;
using System.Threading.Tasks;

namespace Zdimk.Application.Interfaces
{
    public interface IPictureService
    {
        Task SaveToContentFolderAsync(Stream source, string pictureId, string fileExtension);    
        string GetPictureUrl(string pictureId, string fileExtension);
    }
}