namespace Clenka.Benelvis.BackendRsvp.Models
{
    public class MailInfo
    {
        public string EmailTo { get; set; }
        public string EmailToName { get; set; } 
        public List<string> EmailToCCs { get; set; } = new List<string>();
        public List<string> EmailToBCCs { get; set; } = new List<string>();

        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;

        public bool IsHtml { get; set; }
    }
}
