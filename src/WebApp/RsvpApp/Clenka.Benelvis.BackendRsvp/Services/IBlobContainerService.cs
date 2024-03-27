using Clenka.Benelvis.BackendRsvp.Models;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public interface IBlobContainerService
    {
        Task<string> UploadRsvpBlobAsync(string filePath, RsvpEntity data);
        Task<string> UploadRsvpBlobAsync(RsvpEntity data);
        Task<bool> DeleteRsvpBlobAsync(RsvpEntity data);
        Task<Stream> DownloadRsvpBlobAsync(RsvpEntity data);
    }
}
