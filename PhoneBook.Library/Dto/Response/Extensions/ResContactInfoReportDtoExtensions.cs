using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library.Dto.Response.Extensions
{
    public static class ResContactInfoReportDtoExtensions
    {
        public static string ToCsvFormat(this List<ResContactInfoReportDto> resContactInfoReportDto)
        {
            string result = "Location;Person Count;Phone Count";

            foreach (var item in resContactInfoReportDto)
            {
                result += Environment.NewLine;
                result += $"{ item.Location };{ item.PersonCount };{ item.PhoneCount }";
            }

            return result;
        }
    }
}
