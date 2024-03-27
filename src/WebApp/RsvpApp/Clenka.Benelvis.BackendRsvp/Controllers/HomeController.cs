using AutoMapper;
using Clenka.Benelvis.BackendRsvp.DTOs;
using Clenka.Benelvis.BackendRsvp.Models;
using Clenka.Benelvis.BackendRsvp.Services;
using Clenka.Benelvis.BackendRsvp.Services.PDFService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MimeKit.Cryptography;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Diagnostics;
using System.IO;

namespace Clenka.Benelvis.BackendRsvp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITableStorageService<RsvpEntity> _tableService;
        private readonly IBlobContainerService _blobContainerService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQrCodeService<RsvpEntity> _qrCoderService;
        string username = "";
        //private readonly IInvitationDocument _invitationDocument;
        public HomeController(ILogger<HomeController> logger, ITableStorageService<RsvpEntity> tableService, IMapper mapper, IHttpContextAccessor httpContextAccessor, IBlobContainerService blobContainerService, IQrCodeService<RsvpEntity> qrCoderService)
        {
            _logger = logger;
            _mapper = mapper;
            _tableService = tableService ?? throw new ArgumentNullException(nameof(tableService));
            _httpContextAccessor = httpContextAccessor;
            _blobContainerService = blobContainerService;
            _qrCoderService = qrCoderService;
            //    _invitationDocument = invitationDocument;
        }



        public async Task<IActionResult> Index()
        {
            if(!IsAllowed())
                return RedirectToAction(nameof(Login));
            _logger.LogInformation("Getting Rsvps");
            var result = await _tableService.GetAllAsync();
             _logger.LogInformation("Got RSVPs");
            var r = result.OrderByDescending(x => x.Timestamp).ToList();
            var rsvps = _mapper.Map<IReadOnlyList<RsvpEntityDto>>(r);
            return View(rsvps);
        }


        public async Task<ActionResult> Details(string id)
        {
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));

            if (id == null)
            {
                return NotFound();
            }

            var data = await _tableService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            var details = _mapper.Map<RsvpEntityDto>(data);
            return View(details);
        }


        public async Task<ActionResult> Edit(string id)
        {
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));
            if (id == null)
            {
                return NotFound();
            }

            var data = await _tableService.GetByIdAsync(id);
            var rSVPEntityDto = _mapper.Map<UpdateRsvpEntityDto>(data);
            if (rSVPEntityDto == null)
            {
                return NotFound();
            }
            return View(rSVPEntityDto);
        }

        // POST: RsvpController/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, [Bind("RowKey,Title,Fname,Lname,Attendance,Seat")] UpdateRsvpEntityDto rSVPEntityDto)
        {
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));
            if (ModelState.IsValid)
            {
                try
                {
                    var data = await _tableService.GetByIdAsync(id);
                    if (data == null)
                        return NotFound();
                    

                    data.Attendance = rSVPEntityDto.Attendance.ToString();
                    data.Title = rSVPEntityDto.Title.ToString();
                    data.Seat = rSVPEntityDto.Seat;

                    data.LastUpdated = DateTime.UtcNow;
                    await _tableService.UpdateAsync(data);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;

                }
                return RedirectToAction(nameof(Index));
            }
            return View(rSVPEntityDto);
        }

        // GET: RsvpController/Delete/5

        public async Task<ActionResult> Delete(string id)
        {
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));
            if (id == null)
            {
                return NotFound();
            }

            var data = await _tableService.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            var res = _mapper.Map<RsvpEntityDto>(data);
            res.Id = data.RowKey;
            return View(res);
        }

        // POST: RsvpController/Delete/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));
            var rsvp = await _tableService.GetByIdAsync(id);
            if (rsvp != null)
            {
                await _tableService.DeleteASync(id);
            }

            return RedirectToAction(nameof(Index));

        }


        public async Task<ActionResult> GenerateInvitation(string id)
        {
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));
            if (id == null)
            {
                return NotFound();
            }

            var data = await _tableService.GetByIdAsync(id);

           
            if (data == null)
            {
                return NotFound();
            }
         

            var qrcodeImage = await _qrCoderService.GenerateQrCodeAsync(data);

            var result = await _blobContainerService.UploadRsvpBlobAsync(data);
            if (result != "Ok" )
            {
                return RedirectToAction(nameof(ErrorOccured));
            }

            return RedirectToAction(nameof(Success));

            //string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), "filename");
            //string path = Server.MapPath("~/Uploads/");
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            //postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));

            //InvitationModel invitationModel = new InvitationModel()
            //{
            //    Id = data.RowKey,
            //    Title = data.Title,
            //    FirstName = data.Fname,
            //    LastName = data.Lname,
            //    Seat = data.Seat,
            //    Email = data.Email,
            //    Attendance = data.Attendance,
            //    InvitationText = "You are cordially invited to the wedding of Bernice and Elvis, scheduled to take place on Saturday August 17.2024 in Essen, Germany.",
            //    InvitationTitle = "Wedding Invitation",
            //    CreatedBy = "Clenkasoft",
            //    Remarks = "We shall not be sending any print invitations. Your invitation has been recorded in our system. Just come along with the downloaded version of the invitation in your phone.",
            //    IssueDate = DateTime.UtcNow,
            //};

            //var  _invitationDocument = new InvitationDocument(invitationModel);
            //byte[] pdf = null;
            //try
            //{
            //    QuestPDF.Settings.License = LicenseType.Community;
            //    pdf = _invitationDocument.GeneratePdf();
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogError(ex, "Error generating PDF");

            //}

            //return File(pdf, "application/pdf",$"Wedding_Invitation_{data.Lname}.pdf");
        }


        public async Task<ActionResult> SendInvitation(string id)
        {
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));
            if (id == null)
            {
                return NotFound();
            }

            var data = await _tableService.GetByIdAsync(id);
           
            var rSVPEntityDto = _mapper.Map<RsvpEntityDto>(data);
            if (rSVPEntityDto == null)
            {
                return NotFound();
            }
            return View(rSVPEntityDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ErrorOccured()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("UserName,Password")] User user)
        {

            if (ModelState.IsValid)
            {
      
                var userFound = ApplicationUsers.Users.FirstOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
                if (userFound == null)
                    return  RedirectToAction(nameof(Unauthorized));
                //HttpContext.Session.SetString("UserName",userFound.UserName);
                _httpContextAccessor?.HttpContext?.Session.SetString("UserName", userFound.UserName);
                return RedirectToAction(nameof(Index));
         
            }
            return View(user);
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> Preview(string id)
        {
            // Read file from blobstorage
            if (!IsAllowed())
                return RedirectToAction(nameof(Login));
            if (id == null)
            {
                return NotFound();
            }

            var data = await _tableService.GetByIdAsync(id);

            //var dec = await _qrCoderService.DecodeQrCodeStreamAsync($"QRCODES/{data.RowKey}.jpg");

            var result = await _blobContainerService.DownloadRsvpBlobAsync(data);
            if(result == null)
            {
                return RedirectToAction(nameof(ErrorOccured));
            }
            return File(result, "application/pdf");
            // return File(result, "application/pdf", $"Wedding_Invitation_{data.Lname}.pdf");

        }
        private bool IsAllowed()
        {
            username = _httpContextAccessor?.HttpContext?.Session.GetString("UserName");
            if (!string.IsNullOrEmpty(username))
                return true;
            return false;
        }


    }
}
