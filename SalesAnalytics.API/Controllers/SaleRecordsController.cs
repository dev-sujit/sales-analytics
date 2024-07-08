using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesAnalytics.Application.Services;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesAnalytics.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SaleRecordsController : ControllerBase
    {
        private readonly SaleRecordService _saleRecordService;
        private readonly ICommonResponseService _commonResponseService;

        public SaleRecordsController(SaleRecordService saleRecordService, ICommonResponseService commonResponseService)
        {
            _saleRecordService = saleRecordService;
            _commonResponseService = commonResponseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommonResponseModel<SaleRecord>>> GetSaleRecordById(int id)
        {
            try
            {
                var saleRecord = await _saleRecordService.GetSaleRecordByIdAsync(id);
                var commonResponse = _commonResponseService.CreateSuccessResponse(saleRecord, "Sales Record By Id fetched successfully.");
                return Ok(commonResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = _commonResponseService.CreateErrorResponse<List<SaleRecord>>($"An error occurred while fetching Sales Record By Id:{ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleRecord>>> GetAllSaleRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var saleRecords = await _saleRecordService.GetAllSaleRecordsAsync(page, pageSize);
                var commonResponse = _commonResponseService.CreateSuccessResponse(saleRecords, "Sales Record fetched successfully.");
                return Ok(commonResponse);
            }
            catch (Exception ex) {
                var errorResponse = _commonResponseService.CreateErrorResponse<List<SaleRecord>>($"An error occurred while fetching Sales Record:{ex.Message}");
                return StatusCode(errorResponse.StatusCode, errorResponse);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateSaleRecord([FromBody] SaleRecord saleRecord)
        {
            await _saleRecordService.AddSaleRecordAsync(saleRecord);
            return CreatedAtAction(nameof(GetSaleRecordById), new { id = saleRecord.Id }, saleRecord);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSaleRecord(int id, [FromBody] SaleRecord saleRecord)
        {
            if (id != saleRecord.Id)
            {
                return BadRequest();
            }
            await _saleRecordService.UpdateSaleRecordAsync(saleRecord);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleRecord(int id)
        {
            await _saleRecordService.DeleteSaleRecordAsync(id);
            return NoContent();
        }
    }
}
