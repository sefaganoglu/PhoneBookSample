namespace ContactService.Models
{
    public partial class ContactInfo
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Location { get; set; }
    }
}
