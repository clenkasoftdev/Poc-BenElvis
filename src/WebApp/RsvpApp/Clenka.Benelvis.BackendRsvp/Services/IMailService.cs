using Clenka.Benelvis.BackendRsvp.Models;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(MailInfo mailInfo);
        bool SendMail(MailInfo mailInfo);
    }
}

