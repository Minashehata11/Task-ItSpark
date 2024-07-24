using SchoolSystemStak.DAL.Models;

namespace SchoolSystemTask.BAL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
        public void Add<T>(T entity) where T : class;

    }
}
