﻿namespace ContactService.Models
{
    public partial class ContactInfo
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public virtual Person Person { get; set; }

        public Guid ContactInfoTypeId { get; set; }
        public virtual ContactInfoType ContactInfoType { get; set; }

        public string Value { get; set; }
    }
}
