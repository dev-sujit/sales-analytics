using Microsoft.AspNetCore.Mvc;
using Moq;
using SalesAnalytics.Application.Services;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interface;
using SalesAnalytics.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesAnalytics.Tests
{
    public class SaleAnalyticsServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISaleRecordRepository> _saleRecordRepositoryMock;
        private readonly SaleRecordService _saleRecordService;
        private readonly Mock<ICommonResponseService> _commonResponseServiceMock;

        public SaleAnalyticsServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _saleRecordRepositoryMock = new Mock<ISaleRecordRepository>();
            _saleRecordService = new SaleRecordService(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(u => u.SaleRecordRepository).Returns(_saleRecordRepositoryMock.Object);

            _saleRecordService = new SaleRecordService(_unitOfWorkMock.Object);

            _commonResponseServiceMock = new Mock<ICommonResponseService>();
        }

        [Fact]
        public async Task GetTotalSalesAsync_ReturnsOkResultWithSales()
        {
            // Arrange
            var startDate = DateTime.Now.AddMonths(-1);
            var endDate = DateTime.Now;
            var sales = new List<SaleRecord> { new SaleRecord { Id = 1, Product = "Test Product" } };

            // Mocking the service method
            _saleRecordRepositoryMock.Setup(s => s.GetSalesWithinDateRangeAsync(startDate, endDate)).ReturnsAsync(sales);

            // Mocking the response from CommonResponseService
            _commonResponseServiceMock.Setup(c => c.CreateSuccessResponse(
                It.IsAny<List<SaleRecord>>(), // Use It.IsAny<T>() to match any list of SaleRecord
                "Total Sales fetched successfully."
            )).Returns(new CommonResponseModel<List<SaleRecord>>
            {
                StatusCode = 200,
                Data = sales
            });

            // Act
            var result = await _saleRecordService.GetTotalSalesAsync(startDate, endDate);

            // Assert
            Assert.Equal(sales.Sum(s => s.Price), result); // Adjust based on your schema
        }

        [Fact]
        public async Task GetSalesTrendsAsync_ReturnsOkResultWithTrends()
        {
            // Arrange
            var interval = TrendInterval.Monthly;
            var startDate = DateTime.Today.AddMonths(-5); // Adjust based on the interval logic in GetSalesTrendsAsync method
            var endDate = DateTime.Now;
            var salesTrends = new List<SalesTrendDTO>
            {
                new SalesTrendDTO { Date = DateTime.Today, Product = "plastic1", SalesAmount = 100, NumberOfSales = 75 }
            };

            // Mocking the repository method
            _unitOfWorkMock.Setup(u => u.SaleRecordRepository.GetSalesTrendsAsync(startDate, endDate))
                           .ReturnsAsync(salesTrends);

            // Mocking the response from CommonResponseService
            _commonResponseServiceMock.Setup(c => c.CreateSuccessResponse(
                It.IsAny<List<SalesTrendDTO>>(), // Use It.IsAny<T>() to match any list of SalesTrendDTO
                "Trends fetched successfully."
            )).Returns(new CommonResponseModel<List<SalesTrendDTO>>
            {
                StatusCode = 200,
                Data = salesTrends
            });

            // Act
            var result = await _saleRecordService.GetSalesTrendsAsync(interval);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(salesTrends.Count, result.Count); // Verify if the returned trends count matches the expected trends count

            // Optional: Verify individual properties if needed
            Assert.Equal(salesTrends[0].Date, result[0].Date);
            Assert.Equal(salesTrends[0].Product, result[0].Product);
            Assert.Equal(salesTrends[0].SalesAmount, result[0].SalesAmount);
            Assert.Equal(salesTrends[0].NumberOfSales, result[0].NumberOfSales);
        }

        [Fact]
        public async Task GetSalesTrendsAsync_ReturnsOkResultWithSalesTrends()
        {
            var startDate = DateTime.Today.AddMonths(-5); // Adjust based on the interval logic in GetSalesTrendsAsync method
            var endDate = DateTime.Now;

            // Arrange
            var interval = TrendInterval.Daily; // Adjust as needed
            var salesTrends = new List<SalesTrendDTO>
            {
                new SalesTrendDTO { Date = DateTime.Now.Date, Product = "Test Product", SalesAmount = 1000, NumberOfSales = 10 }
            };

            // Mocking the service method
            _unitOfWorkMock.Setup(s => s.SaleRecordRepository.GetSalesTrendsAsync(startDate, endDate)).ReturnsAsync(salesTrends);

            // Mocking the response from CommonResponseService
            _commonResponseServiceMock.Setup(c => c.CreateSuccessResponse(
                It.IsAny<List<SalesTrendDTO>>(), // Use It.IsAny<T>() to match any list of SalesTrendDTO
                "Trends fetched successfully."
            )).Returns(new CommonResponseModel<List<SalesTrendDTO>>
            {
                StatusCode = 200,
                Data = salesTrends
            });

            // Act
            var result = await _saleRecordService.GetSalesTrendsAsync(interval);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var commonResponse = Assert.IsType<CommonResponseModel<List<SalesTrendDTO>>>(okResult.Value);

            Assert.Equal(200, commonResponse.StatusCode);
            Assert.Equal(salesTrends, commonResponse.Data);
        }

        [Fact]
        public async Task GetSalesByRegionAsync_ReturnsSalesList()
        {
            // Arrange
            var region = "TestRegion";
            var expectedSales = new List<SaleRecord> { new SaleRecord { Id = 1, Product = "Test Product" } };

            // Mocking repository method
            _unitOfWorkMock.Setup(u => u.SaleRecordRepository.GetSalesByRegionAsync(region)).ReturnsAsync(expectedSales);

            // Act
            var result = await _saleRecordService.GetSalesByRegionAsync(region);

            // Assert
            Assert.Equal(expectedSales, result);
        }
    }
}
