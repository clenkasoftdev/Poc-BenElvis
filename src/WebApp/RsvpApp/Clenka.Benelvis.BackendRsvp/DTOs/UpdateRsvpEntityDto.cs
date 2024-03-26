
namespace Clenka.Benelvis.BackendRsvp.DTOs
{
    public class UpdateRsvpEntityDto
    {
        public string RowKey { get; set; }
        public string Title { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Attendance { get; set; }
        public int? Seat { get; set; }

    }
}
