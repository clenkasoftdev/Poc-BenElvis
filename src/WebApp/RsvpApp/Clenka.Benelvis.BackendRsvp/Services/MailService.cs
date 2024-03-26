using Clenka.Benelvis.BackendRsvp.Models;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Clenka.Benelvis.BackendRsvp.Services
{
    public class MailService : IMailService
    {
        //https://mailtrap.io/blog/asp-net-core-send-email/
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

        public bool SendMail(MailInfo mailInfo)
        {

            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailInfo.EmailToName, mailInfo.EmailTo);
                    emailMessage.To.Add(emailTo);

                    foreach (var item in mailInfo.EmailToCCs)
                    {
                        emailMessage.Cc.Add(new MailboxAddress("CC Receiver",item));
                    }
                    foreach (var item in mailInfo.EmailToBCCs)
                    {
                        emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", item));
                    }

                    emailMessage.Subject = mailInfo.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();

                    if(mailInfo.IsHtml)
                    {
                        emailBodyBuilder.HtmlBody = mailInfo.EmailBody;
                    }
                    else
                    {
                        emailBodyBuilder.TextBody = mailInfo.EmailBody;
                    }


                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

        public async Task<bool> SendMailAsync(MailInfo mailInfo)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailInfo.EmailToName, mailInfo.EmailTo);
                    emailMessage.To.Add(emailTo);

                    foreach (var item in mailInfo.EmailToCCs)
                    {
                        emailMessage.Cc.Add(new MailboxAddress("CC Receiver", item));
                    }
                    foreach (var item in mailInfo.EmailToBCCs)
                    {
                        emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", item));
                    }

                    emailMessage.Subject = mailInfo.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();

                    if (mailInfo.IsHtml)
                    {
                        emailBodyBuilder.HtmlBody = mailInfo.EmailBody;
                    }
                    else
                    {
                        emailBodyBuilder.TextBody = mailInfo.EmailBody;
                    }


                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }
    }
}
