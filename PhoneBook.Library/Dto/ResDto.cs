namespace PhoneBook.Library.Dto
{
    public class ResDto<T>
    {
        public int TotalCount { get; set; }
        public bool FunctionStatus { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public T Data { get; set; }
        [NonSerialized]
        public Pagination Pagination;
    }

    public static class ResponseCreator<T>
    {
        public static ResDto<T> Response(bool functionStatus, string errorCode, string message, T data)
        {
            return new ResDto<T>
            {
                Data = data,
                Message = message,
                ErrorCode = errorCode,
                FunctionStatus = functionStatus
            };
        }

    }


    public class Pagination
    {
        public Pagination(int countData, int? page, int countShow = 1)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)countData / (decimal)countShow);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            CountData = countData;
            CurrentPage = currentPage;
            CountShow = countShow;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int CountData { get; private set; }
        public int CountShow { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}
