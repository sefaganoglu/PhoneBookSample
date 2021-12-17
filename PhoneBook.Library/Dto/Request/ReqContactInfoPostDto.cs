using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library.Dto.Request
{
    public class ReqContactInfoPostDto
    {
        public Guid PersonId { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }
    }
}
