using Microsoft.AspNetCore.Mvc;
using PhoneBook.Library.Helper;
using PhoneBook.Library.Infrastructure.Validation;

namespace PhoneBook.Library
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExceptionAttribute]
    [ModelStateValidationActionFilter]
    public class BaseController : ControllerBase
    {

    }
}