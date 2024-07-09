using Moq;
using SalesAnalytics.Application.Services;
using SalesAnalytics.Core.Entities;
using SalesAnalytics.Core.Interfaces;

namespace SalesAnalytics.Tests
{
    public class SaleRecordServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ISaleRecordRepository> _saleRecordRepositoryMock;
        private readonly SaleRecordService _saleRecordService;

        public SaleRecordServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _saleRecordRepositoryMock = new Mock<ISaleRecordRepository>();
            _saleRecordService = new SaleRecordService(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(u => u.SaleRecordRepository).Returns(_saleRecordRepositoryMock.Object);

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

        [Fact]
        public async Task GetAllSaleRecordsAsync_ReturnsSaleRecords()
        {
            // Arrange
            var saleRecords = new List<SaleRecord>();
            _unitOfWorkMock.Setup(u => u.SaleRecordRepository.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(saleRecords);

            // Act
            var result = await _saleRecordService.GetAllSaleRecordsAsync(1, 10);

            // Assert
            Assert.Equal(saleRecords, result);
        }

        [Fact]
        public async Task AddSaleRecordAsync_CallsRepositoryAdd()
        {
            // Arrange
            var saleRecord = new SaleRecord
            { 
                Id = 1,
                Date = DateTime.Now,
                Product = "Test Product",
                Quantity = 1,
                Price = 200,
                NumberOfSales = 100,
                Region = "Random"
            };

            // Act
            await _saleRecordService.AddSaleRecordAsync(saleRecord);

            // Assert
            _unitOfWorkMock.Verify(u => u.SaleRecordRepository.AddAsync(saleRecord), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSaleRecordAsync_CallsRepositoryUpdate()
        {
            // Arrange
            var saleRecord = new SaleRecord
            {
                Id = 1,
                Date = DateTime.Now,
                Product = "Test Product 1",
                Quantity = 1,
                Price = 200,
                NumberOfSales = 100,
                Region = "Random"
            };

            // Act
            await _saleRecordService.UpdateSaleRecordAsync(saleRecord);

            // Assert
            _unitOfWorkMock.Verify(u => u.SaleRecordRepository.UpdateAsync(saleRecord), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteSaleRecordAsync_CallsRepositoryDelete()
        {
            // Arrange
            var saleRecordId = 1;

            // Act
            await _saleRecordService.DeleteSaleRecordAsync(saleRecordId);

            // Assert
            _unitOfWorkMock.Verify(u => u.SaleRecordRepository.DeleteAsync(saleRecordId), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}
