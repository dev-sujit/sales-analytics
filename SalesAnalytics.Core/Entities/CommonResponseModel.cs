namespace SalesAnalytics.Core.Entities
{
    public class CommonResponseModel<T>
    {
        public int StatusCode { get; set; }
        public bool Error { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public CommonResponseModel()
        {
        }

        public CommonResponseModel(int statusCode, bool error, T data, string message)
        {
            StatusCode = statusCode;
            Error = error;
            Data = data;
            Message = message;
        }
    }

}
