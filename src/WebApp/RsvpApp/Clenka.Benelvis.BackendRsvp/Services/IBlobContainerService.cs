using Clenka.Benelvis.BackendRsvp.Models;
using System.Threading.Tasks;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public interface IBlobContainerService
    {
        Task<string> UploadRsvpBlobAsync(string filePath, RsvpEntity data);
        Task<string> UploadRsvpBlobAsync(RsvpEntity data, byte[] qrImage = null);
        Task<bool> DeleteRsvpBlobAsync(RsvpEntity data);
        Task<Stream> DownloadRsvpBlobAsync(RsvpEntity data);
        Task<string> GetBlobUrl(RsvpEntity data);
    }
}
