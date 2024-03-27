
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Clenka.Benelvis.BackendRsvp.Constants;
using Clenka.Benelvis.BackendRsvp.Models;
using Clenka.Benelvis.BackendRsvp.Services.PDFService;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public class BlobContainerService : IBlobContainerService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BlobContainerService> _logger;

        public BlobContainerService(IConfiguration configuration, ILogger<BlobContainerService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _blobContainerClient = GetBlobClientAsync(GlobalConstants.RSVPBLOBCONTAINERNAME).Result;
        }

        private async Task<BlobContainerClient> GetBlobClientAsync(string blobName)
        {
            var x = GlobalConstants.RSVPENTITYCONNECTIONSTRINGNAME;
            var connectionString = _configuration.GetConnectionString(GlobalConstants.RSVPENTITYCONNECTIONSTRINGNAME);
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(blobName);
            await blobContainerClient.CreateIfNotExistsAsync();
            return blobContainerClient;
        }
        public Task<bool> DeleteRsvpBlobAsync(RsvpEntity data)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadRsvpBlobAsync(RsvpEntity data)
        {
            try
            {
                string blobName = $"{data.RowKey}.pdf";
                var blobClient = _blobContainerClient.GetBlobClient(blobName);
                var s = blobClient.Uri.ToString();
                var response = blobClient.Download();
                return Task.FromResult(response.Value.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file from blob");
                return Task.FromResult<Stream>(null);
            }

        }

        public Task<string> GetBlobUrl(RsvpEntity data)
        {
            try
            {
                string blobName = $"{data.RowKey}.jpg";
                var blobClient = _blobContainerClient.GetBlobClient(blobName);
                var s = blobClient.Uri.ToString();
                return Task.FromResult(s);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file from blob");
                return Task.FromResult("error");
            }
        }

        public Task<string> UploadRsvpBlobAsync(string filePath, RsvpEntity data)
        {
            throw new NotImplementedException();
        }
        
        public Task<string> UploadRsvpBlobAsync(RsvpEntity data, byte[] qrImage = null)
        {
            string retval = "error";
            InvitationModel invitationModel = new InvitationModel()
            {
                Id = data.RowKey,
                Title = data.Title,
                FirstName = data.Fname,
                LastName = data.Lname,
                Seat = data.Seat,
                Email = data.Email,
                Attendance = data.Attendance,
                InvitationText = "You are cordially invited to the wedding of Bernice and Elvis, scheduled to take place on Saturday August 17.2024 in Essen, Germany.",
                InvitationTitle = "Wedding Invitation",
                CreatedBy = "Clenkasoft",
                Remarks = "We shall not be sending any print invitations. Your invitation has been recorded in our system. Just come along with the downloaded version of the invitation in your phone.",
                IssueDate = DateTime.UtcNow,
            };

            var _invitationDocument = new InvitationDocument(invitationModel);
           
            try
            {
                QuestPDF.Settings.License = LicenseType.Community;
                byte[] pdf = _invitationDocument.GeneratePdf();


                var blobClient = _blobContainerClient.GetBlobClient($"{invitationModel.Id}.pdf");

                using (var stream = new MemoryStream(pdf))
                {

                    var res = blobClient.Upload(stream, true);

                    if (res.GetRawResponse().Status == 201)
                    {
                        retval = "Ok";
                    }
                }

                //if(qrImage != null)
                //{
                //    var qrBlobClient = _blobContainerClient.GetBlobClient($"{invitationModel.Id}.jpg");
                //    using (var stream = new MemoryStream(qrImage))
                //    {
                //        var res = qrBlobClient.Upload(stream, true);
                //        if (res.GetRawResponse().Status == 201)
                //        {
                //            retval = "Ok";
                //        }
                //    }
                //}   
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file to blob");
                return Task.FromResult("Error");
            }

            return Task.FromResult(retval);
        }


    }
}
