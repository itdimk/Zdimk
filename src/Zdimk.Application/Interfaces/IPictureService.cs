using System;
using System.IO;
using System.Threading.Tasks;

namespace Zdimk.Application.Interfaces
{
    public interface IPictureService
    {
        Task SaveToContentFolderAsync(Stream source, string uniqueFileName);    
        string GetPictureUrl(string uniqueFileName);
    }
}