using Microsoft.EntityFrameworkCore;
using SchoolSystemStak.DAL.Context;
using SchoolSystemStak.DAL.Models;
using SchoolSystemTask.BAL.Interfaces;
using SchoolSystemTask.BAL.Specefication;

namespace SchoolSystemTask.BAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ShcoolDbContext _shcoolDbContext;

        public GenericRepository(ShcoolDbContext shcoolDbContext )
        {
            _shcoolDbContext = shcoolDbContext;
        }

        public void AddEntity(T entity)
        => _shcoolDbContext.Set<T>().Add(entity);

        public void DeleteEntity(T entity)
        => _shcoolDbContext.Set<T>().Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _shcoolDbContext.Set<T>().ToListAsync();

        public async Task<T> GetById(int id)
         => await _shcoolDbContext.Set<T>().FindAsync(id);

        public void UpdateEntity(T entity)
         => _shcoolDbContext.Set<T>().Update(entity);
        public async Task<IReadOnlyList<T>> GetAllAsyncWithSpecification(ISpecefication<T> specs)
        =>
            await ApplySpecification(specs).ToListAsync();



        public async Task<T> GetByIdAsyncWithSpecification(ISpecefication<T> specs)
            =>
         await ApplySpecification(specs).FirstOrDefaultAsync();

        private IQueryable<T> ApplySpecification(ISpecefication<T> spec)
            => SpecefiationEvaluator<T>.GetQuery(_shcoolDbContext.Set<T>(), spec);
    }
}
