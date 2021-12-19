using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PhoneBook.Library.Exceptions;

namespace PhoneBook.Library.Middlewares
{
    public class ServiceExceptionInterceptor : ExceptionFilterAttribute, IAsyncExceptionFilter
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            context.ExceptionHandled = true;

            switch (context.Exception)
            {
                case BaseBadRequestException businessException:
                    {
                        context.HttpContext.Response.StatusCode = 400;
                        context.Result = new JsonResult(new ExceptionHttpResponse()
                        {
                            ErrorType = businessException.ErrorType,
                            ErrorCode = businessException.ErrorCode,
                            ErrorMessage = businessException.ErrorMessage
                        });
                        break;
                    }
                case BaseServiceUnavailableException businessException:
                    {
                        context.HttpContext.Response.StatusCode = 503;
                        context.Result = new JsonResult(new ExceptionHttpResponse()
                        {
                            ErrorType = businessException.ErrorType,
                            ErrorCode = businessException.ErrorCode,
                            ErrorMessage = businessException.ErrorMessage
                        });
                        break;
                    }
                case BaseInternalServerErrorException businessException:
                    {
                        context.HttpContext.Response.StatusCode = 500;
                        context.Result = new JsonResult(new ExceptionHttpResponse()
                        {
                            ErrorType = businessException.ErrorType,
                            ErrorCode = businessException.ErrorCode,
                            ErrorMessage = businessException.ErrorMessage
                        });
                        break;
                    }
                case DbUpdateException dbUpdateException:
                    {
                        context.HttpContext.Response.StatusCode = 500;
                        context.Result = new JsonResult(new ExceptionHttpResponse()
                        {
                            ErrorType = "InternalServerError",
                            ErrorCode = "InternalServerError",
                            ErrorMessage = "Internal Server Error"
                        });
                        break;
                    }
                case System.ComponentModel.DataAnnotations.ValidationException validationException:
                    {
                        context.HttpContext.Response.StatusCode = 400;
                        if (validationException.ValidationAttribute != null)
                            context.Result = new JsonResult(new ExceptionHttpResponse()
                            {
                                ErrorType = "ValidationError",
                                ErrorCode = "ValidationError",
                                Fields = new Dictionary<string, string>()
                                {
                                    {
                                        (validationException.ValidationAttribute.ErrorMessageResourceName ?? validationException.ValidationResult.MemberNames.FirstOrDefault()) ?? "Unknown",
                                        validationException.ValidationAttribute.ErrorMessage
                                    }
                                }
                            });
                        break;
                    }
                default:
                    {
                        context.HttpContext.Response.StatusCode = 500;
                        context.Result = new JsonResult(new ExceptionHttpResponse()
                        {
                            ErrorType = "InternalServerError",
                            ErrorCode = "InternalServerError",
                            ErrorMessage = "Internal Server Error"
                        });
                        break;
                    }
            }

            return Task.CompletedTask;
        }
    }

    public class ExceptionHttpResponse
    {
        public string ErrorType { get; init; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, string> Fields { get; set; }
    }
}
