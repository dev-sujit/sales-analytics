using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesAnalytics.Application.Services;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interface;

namespace SalesAnalytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesAnalyticsController : ControllerBase
    {
        private readonly SaleRecordService _saleRecordService;
        private readonly ICommonResponseService _commonResponseService;

        public SalesAnalyticsController(SaleRecordService saleRecordService, ICommonResponseService commonResponseService)
        {
            _saleRecordService = saleRecordService;
            _commonResponseService = commonResponseService;
        }

        [HttpGet("gettotalsales")]
        public async Task<ActionResult<CommonResponseModel<List<SaleRecord>>>> GetTotalSalesAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var sales = await _saleRecordService.GetTotalSalesAsync(startDate, endDate);
                var commonResponse = _commonResponseService.CreateSuccessResponse(sales, "Total Sales fetched successfully.");
                return Ok(commonResponse);
            }
            catch (Exception ex) 
            {
                var errorResponse = _commonResponseService.CreateErrorResponse<List<SaleRecord>>($"An error occurred while fetching Total Sales:{ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
            
        }

        [HttpGet("trends/{interval}")]
        public async Task<ActionResult<CommonResponseModel<List<SalesTrendDTO>>>> GetSalesTrendsAsync(TrendInterval interval)
        {
            try
            {
                var salesTrends = await _saleRecordService.GetSalesTrendsAsync(interval);
                var commonResponse = _commonResponseService.CreateSuccessResponse(salesTrends, "Trends fetched successfully.");
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = _commonResponseService.CreateErrorResponse<List<SaleRecord>>($"An error occurred while fetching  Trends:{ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
        }

        [HttpGet("topproducts")]
        public async Task<ActionResult<CommonResponseModel<List<SaleRecord>>>> GetTopProductsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var sales = await _saleRecordService.GetTopProductsAsync(startDate, endDate);
                var commonResponse = _commonResponseService.CreateSuccessResponse(sales, "Top Products fetched successfully.");
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = _commonResponseService.CreateErrorResponse<List<SaleRecord>>($"An error occurred while fetching Top Products: {ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
        }

        [HttpGet("getsalesbyregion")]
        public async Task<ActionResult<CommonResponseModel<List<SaleRecord>>>> GetSalesByRegionAsync(string region)
        {
            try
            {
                var sales = await _saleRecordService.GetSalesByRegionAsync(region);
                var commonResponse = _commonResponseService.CreateSuccessResponse(sales, "Sales by Region fetched successfully.");
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = _commonResponseService.CreateErrorResponse<List<SaleRecord>>($"An error occurred while fetching Sales by Region: {ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
        }
    }
}
