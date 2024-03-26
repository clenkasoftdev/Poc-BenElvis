using Azure;
using Azure.Data.Tables;

namespace Clenka.Benelvis.BackendRsvp.Models
{
    public class RsvpEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Attendance { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string LinkChoice { get; set; }
        public string OtherText { get; set; }
        public string Message { get; set; }
        public int? Seat { get; set; }
        public bool EmailSent { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string Ip { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
