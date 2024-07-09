using Moq;
using SalesAnalytics.Application.Services;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interfaces;

namespace SalesAnalytics.Tests
{
    public class SaleRecordServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly SaleRecordService _saleRecordService;

        public SaleRecordServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _saleRecordService = new SaleRecordService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetSaleRecordByIdAsync_ReturnsSaleRecord()
        {
            // Arrange
            var saleRecord = new SaleRecord { Id = 1, Product = "Test Product" };
            _unitOfWorkMock.Setup(u => u.SaleRecordRepository.GetByIdAsync(1)).ReturnsAsync(saleRecord);

            // Act
            var result = await _saleRecordService.GetSaleRecordByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test Product", result.Product);
        }
    }
}
