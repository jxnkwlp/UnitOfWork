using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Passingwind.UnitOfWork
{
	public interface IRepository<TEntity> where TEntity : class
	{
		DbSet<TEntity> DbSet { get; }

		TEntity Get(params object[] keyValues);
		Task<TEntity> GetAsync(params object[] keyValues);

		TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
										   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
										   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

		Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
										   Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
										   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

		IQueryable<TEntity> GetQueryable();
		IQueryable<TEntity> GetQueryableIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

		TEntity Insert(TEntity entity);
		Task<TEntity> InsertAsync(TEntity entity);

		void Update(TEntity entity);
		Task UpdateAsync(TEntity entity);

		void Delete(TEntity entity);
		Task DeleteAsync(TEntity entity);
		void Delete(Expression<Func<TEntity, bool>> predicate);
		Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

		int Count();
		Task<int> CountAsync();
		int Count(Expression<Func<TEntity, bool>> predicate);
		Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
		long LongCount();
		Task<long> LongCountAsync();
		long LongCount(Expression<Func<TEntity, bool>> predicate);
		Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate);

	}
}