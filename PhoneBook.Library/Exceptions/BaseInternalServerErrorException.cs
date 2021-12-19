using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library.Exceptions
{
    public class BaseInternalServerErrorException : BaseException
    {
        public BaseInternalServerErrorException() : base("InternalServerError")
        {

        }

        public BaseInternalServerErrorException(string errorCode, string errorMessage) : base("InternalServerError", errorCode, errorMessage)
        {

        }
    }
}
