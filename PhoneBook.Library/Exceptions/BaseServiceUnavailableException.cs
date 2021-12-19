using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Library.Exceptions
{
    public class BaseServiceUnavailableException : BaseException
    {
        public BaseServiceUnavailableException() : base("ServiceUnavailable")
        {

        }

        public BaseServiceUnavailableException(string errorCode, string errorMessage) : base("ServiceUnavailable", errorCode, errorMessage)
        {

        }
    }
}
