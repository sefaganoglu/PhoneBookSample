namespace ContactService.Models
{
    public partial class ContactInfoType
    {
        public ContactInfoType()
        {
            ContactInfos = new HashSet<ContactInfo>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ContactInfo> ContactInfos { get; set; }
    }
}
