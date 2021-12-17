using PhoneBook.Library.Dto.Response;

namespace PhoneBook.Library.Services
{
    public class NLogService : INLogService
    {
        public bool SaveException(ResSaveExceptionDto resSaveExceptionDto)
        {
            var logger = NLog.LogManager.GetLogger("exception");

            var message = $"-----------------------------{resSaveExceptionDto.RequestTime}----------------------------------------------\r\n ";
            message += "curl --location --request " + resSaveExceptionDto.MethodType + " \"" + resSaveExceptionDto.Url + "\" \r\n  ";
            message += "--header \"Content-Type: " + resSaveExceptionDto.ContentType + "\" \r\n";

            if (resSaveExceptionDto.MethodType == "POST")
            {
                message += "--data '" + resSaveExceptionDto.Request + "' \r\n";
            }



            message += "\r\n ------------------------------Reponse Data--------------------------------------------- \r\n";

            message += resSaveExceptionDto.Response;


            message += "\r\n --------------------------------------------------------------------------- \r\n";

            logger.Info(message);

            return true;
        }
    }
}
