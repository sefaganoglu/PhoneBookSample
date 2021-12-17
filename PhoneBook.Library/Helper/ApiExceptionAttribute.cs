using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PhoneBook.Library.Dto;
using PhoneBook.Library.Services;

namespace PhoneBook.Library.Helper
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {

            if (context.Exception != null)
            {
                var nLogService = (INLogService)context.HttpContext.RequestServices.GetService(typeof(INLogService));
                var message = new ResDto<dynamic> { FunctionStatus = false, Data = null, Message = "Technic Error -" + context.Exception.Message + "\r\n" + (context.Exception?.StackTrace ?? "") };

                var expDto = new ResSaveExceptionDto();
                expDto.Url = context.HttpContext?.Request?.Host.Value;
                expDto.ContentType = context.HttpContext.Request?.ContentType;
                expDto.MethodType = context.HttpContext.Request?.Path.Value;
                expDto.Request = string.Empty;
                expDto.RequestTime = "0:00";
                expDto.Response = message.Message;
                nLogService.SaveException(expDto);
                context.Result = new ContentResult { Content = message.Message, StatusCode = (int)System.Net.HttpStatusCode.UnprocessableEntity };
            }
        }
    }
}
