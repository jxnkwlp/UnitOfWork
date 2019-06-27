using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Passingwind.UnitOfWork
{
	public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private DbContext _context;

		public DbSet<TEntity> DbSet
		{
			get
			{
				return _context.Set<TEntity>();
			}
		}

		public GenericRepository(DbContext context)
		{
			_context = context;
		}

		public TEntity Get(params object[] keyValues)
		{
			return DbSet.Find(keyValues);
		}

		public async Task<TEntity> GetAsync(params object[] keyValues)
		{
			return await DbSet.FindAsync(keyValues);
		}

		public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
		{
			var q = DbSet.AsQueryable();
			if (include != null)
				q = include(q);
			if (predicate != null)
				q = q.Where(predicate);
			if (orderBy != null)
				q = orderBy(q);

			return q.FirstOrDefault();
		}

		public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
		{
			var q = DbSet.AsQueryable();
			if (include != null)
				q = include(q);
			if (predicate != null)
				q = q.Where(predicate);
			if (orderBy != null)
				q = orderBy(q);

			return await q.FirstOrDefaultAsync();
		}

		public IQueryable<TEntity> GetQueryable()
		{
			return DbSet;
		}

		public IQueryable<TEntity> GetQueryableIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
		{
			var q = DbSet.AsQueryable();

			if (propertySelectors != null)
				foreach (var include in propertySelectors)
				{
					q = q.Include(include);
				}

			return q;
		}

		public TEntity Insert(TEntity entity)
		{
			DbSet.Add(entity);

			return entity;
		}

		public async Task<TEntity> InsertAsync(TEntity entity)
		{
			await DbSet.AddAsync(entity);

			return entity;
		}

		public void Update(TEntity entity)
		{
			DbSet.Update(entity);

		}

		public Task UpdateAsync(TEntity entity)
		{
			DbSet.Update(entity);
			return Task.CompletedTask;
		}

		public void Delete(TEntity entity)
		{
			DbSet.Remove(entity);
		}

		public Task DeleteAsync(TEntity entity)
		{
			DbSet.Remove(entity);
			return Task.CompletedTask;
		}

		public void Delete(Expression<Func<TEntity, bool>> predicate)
		{
			DbSet.RemoveRange(DbSet.Where(predicate));
		}

		public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
		{
			DbSet.RemoveRange(DbSet.Where(predicate));

			return Task.CompletedTask;
		}

		public int Count()
		{
			return DbSet.Count();
		}

		public async Task<int> CountAsync()
		{
			return await DbSet.CountAsync();
		}

		public int Count(Expression<Func<TEntity, bool>> predicate)
		{
			return DbSet.Count(predicate);
		}

		public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await DbSet.CountAsync(predicate);
		}

		public long LongCount()
		{
			return DbSet.LongCount();
		}

		public async Task<long> LongCountAsync()
		{
			return await DbSet.LongCountAsync();
		}

		public long LongCount(Expression<Func<TEntity, bool>> predicate)
		{
			return DbSet.LongCount(predicate);
		}

		public async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await DbSet.LongCountAsync(predicate);
		}
	}
}