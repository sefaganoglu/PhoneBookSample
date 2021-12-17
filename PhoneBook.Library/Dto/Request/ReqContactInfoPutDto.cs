using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library.Dto.Request
{
    public class ReqContactInfoPutDto
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }
    }
}
