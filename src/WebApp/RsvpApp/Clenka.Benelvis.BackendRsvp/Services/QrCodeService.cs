

using Clenka.Benelvis.BackendRsvp.DTOs;
using Clenka.Benelvis.BackendRsvp.Models;
using IronBarCode;
using System.Drawing;
using System.Drawing.Imaging;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public class QrCodeService<T> : IQrCodeService<T> where T : class
    {
        private readonly ILogger<QrCodeService<T>> _logger;

        public QrCodeService(ILogger<QrCodeService<T>> logger)
        {
            _logger = logger;
        }

        public async Task<string> DecodeQrCodeStreamAsync(string filelocation)
        {
            try
            {
                BarcodeResults readResult = await BarcodeReader.ReadAsync(filelocation, new
                BarcodeReaderOptions()
                { ExpectBarcodeTypes = BarcodeEncoding.QRCode });

                if (readResult != null)
                {
                    _logger.LogInformation("Qr Code decoded");
                    return readResult.First().Value;
                }
                else
                {
                    _logger.LogInformation("Qr Code not decoded");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decoding QR Code");
                return null;
            }
            

        }

        public Task<string> GenerateQrCodeAsync(T data)
        {
            try
            {
                _logger.LogInformation("GenerateQrCodeAsync");
                RsvpEntity rsvpEntity = data as RsvpEntity;
                var rs = QRCodeWriter.CreateQrCode(data.ToString(), 300, QRCodeWriter.QrErrorCorrectionLevel.Medium).SaveAsJpeg($"{rsvpEntity.RowKey}.jpg");
                string path = $"QRCodes/{rsvpEntity.RowKey}.jpg";
                var res = rs.SaveAsWindowsBitmap(path);

                //var stream = BitmapToByteArray(rs.ToBitmap());
                //return Task.FromResult(stream);

                if (res.BinaryValue != null)
                {
                    _logger.LogInformation("Qr Code generated");
                    return Task.FromResult("Ok");
                    
                }
                else
                {
                    _logger.LogInformation("Qr Code not generated");
                    return Task.FromResult("Nok");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating QR Code");
                return Task.FromResult("Nok");
            }

        }

        private byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
