using Clenka.Benelvis.BackendRsvp.Models;
using Clenka.Benelvis.BackendRsvp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Clenka.Benelvis.BackendRsvp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailService _mailService;
        //injecting the IMailService into the constructor
        public MailController(IMailService _MailService)
        {
            _mailService = _MailService;
        }

        [HttpPost]
        [Route("SendMail")]
        public bool SendMail(MailInfo mailInfo)
        {
            return _mailService.SendMail(mailInfo);
        }
    }
}
