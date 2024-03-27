using IronBarCode;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public interface IQrCodeService<T> where T : class
    {
        Task<string> GenerateQrCodeAsync(T data);

        public Task<string> DecodeQrCodeStreamAsync(string filelocation);
    }
}
