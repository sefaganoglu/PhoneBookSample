using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library.Dto.Response
{
    public class ResContactInfoReportDto
    {
        public string? Location { get; set; }
        public int PersonCount { get; set; }
        public int PhoneCount { get; set; }

    }
}
