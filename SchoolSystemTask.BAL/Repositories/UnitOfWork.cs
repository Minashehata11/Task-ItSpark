using SchoolSystemStak.DAL.Context;
using SchoolSystemStak.DAL.Models;
using SchoolSystemTask.BAL.Interfaces;
using System.Collections;

namespace SchoolSystemTask.BAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _respositories;
        private readonly ShcoolDbContext _shcoolDbContext;

        public UnitOfWork(ShcoolDbContext shcoolDbContext )
        {
            _respositories = new Hashtable();
            _shcoolDbContext = shcoolDbContext;
        }

        public async Task<int> CompleteAsync()
        => await _shcoolDbContext.SaveChangesAsync();

        public void Add<T>(T entity) where T : class
        {
            _shcoolDbContext.Add(entity);
        }
        public async ValueTask DisposeAsync()
        => await _shcoolDbContext.DisposeAsync();

        public IGenericRepository<Tentity> Repository<Tentity>() where Tentity : BaseEntity
        {

            var type = typeof(Tentity).Name; 
            if (!_respositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<Tentity>(_shcoolDbContext);
                _respositories.Add(type, Repository);
            }
            return (IGenericRepository<Tentity>)_respositories[type];
        }


    }
}
