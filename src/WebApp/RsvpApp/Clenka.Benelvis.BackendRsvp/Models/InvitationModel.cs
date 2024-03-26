namespace Clenka.Benelvis.BackendRsvp.Models
{
    public class InvitationModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Seat { get; set; }
        public DateTime IssueDate { get; set; }
        public string Remarks { get; set; }
        public string Attendance { get; set; }

        public string InvitationTitle{ get; set; }
        public string InvitationText { get; set; }

        public string Email { get; set; }

        public string CreatedBy { get; set; }

    }

}
