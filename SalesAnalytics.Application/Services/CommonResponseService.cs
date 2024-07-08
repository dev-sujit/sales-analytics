using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interface;

namespace SalesAnalytics.Application.Services
{
    public class CommonResponseService : ICommonResponseService
    {
        public CommonResponseModel<T> CreateSuccessResponse<T>(T data, string message)
        {
            return new CommonResponseModel<T>
            {
                StatusCode = 200,
                Error = false,
                Data = data,
                Message = message
            };
        }

        public CommonResponseModel<T> CreateErrorResponse<T>(string message, int statusCode = 500)
        {
            return new CommonResponseModel<T>
            {
                StatusCode = statusCode,
                Error = true,
                Data = default,
                Message = message
            };
        }
    }
}
