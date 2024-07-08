using SalesAnalytics.Core.Entities;

namespace SalesAnalytics.Core.Interface
{
    public interface ICommonResponseService
    {
        CommonResponseModel<T> CreateSuccessResponse<T>(T data, string message);
        CommonResponseModel<T> CreateErrorResponse<T>(string message, int statusCode = 500);
    }
}
