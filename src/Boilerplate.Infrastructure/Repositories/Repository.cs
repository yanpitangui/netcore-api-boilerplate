using System;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Domain.Core.Entities;
using Boilerplate.Domain.Core.Interfaces;
using Boilerplate.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        public Repository(ApplicationDbContext dbContext)
        {
            Db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbSet = Db.Set<TEntity>();
        }

        protected ApplicationDbContext Db { get; }

        protected DbSet<TEntity> DbSet { get; }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet.AsNoTracking();
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            return await DbSet
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual TEntity Create(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            DbSet.Update(entity);
            return entity;
        }

        public virtual async Task Delete(Guid id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity != null) DbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing) Db.Dispose();
        }
    }
}
