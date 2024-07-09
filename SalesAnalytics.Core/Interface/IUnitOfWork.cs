namespace SalesAnalytics.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISaleRecordRepository SaleRecordRepository { get; }
        Task CompleteAsync();
        Task<int> CommitAsync();
        void Rollback();
    }
}
