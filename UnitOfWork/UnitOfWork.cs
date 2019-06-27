using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Passingwind.UnitOfWork
{
	public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext
	{
		private TContext _context;
		private Dictionary<Type, object> repositories = new Dictionary<Type, object>();

		public TContext DbContext => _context;

		public UnitOfWork(TContext context)
		{
			this._context = context;
		}

		public int ExecuteSqlCommand(string sql, params object[] parameters)
		{
			return _context.Database.ExecuteSqlCommand(sql, parameters);
		}

		public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
		{
			return _context.Set<TEntity>().FromSql(sql, parameters);
		}

		public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
		{
			var type = typeof(TEntity);

			if (!repositories.ContainsKey(type))
				repositories[type] = new GenericRepository<TEntity>(_context);

			return repositories[type] as IRepository<TEntity>;
		}

		public int SaveChanges(bool ensureAutoHistory = false)
		{
			if (ensureAutoHistory)
			{
				_context.EnsureAutoHistory();
			}

			return _context.SaveChanges();
		}

		public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks)
		{
			using (var ts = new TransactionScope())
			{
				var count = 0;
				foreach (var unitOfWork in unitOfWorks)
				{
					count += await unitOfWork.SaveChangesAsync(ensureAutoHistory);
				}

				ts.Complete();

				return count;
			}
		}

		public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (ensureAutoHistory)
			{
				_context.EnsureAutoHistory();
			}

			return await _context.SaveChangesAsync(cancellationToken);
		}


		public void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback)
		{
			_context.ChangeTracker.TrackGraph(rootEntity, callback);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}


		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// clear repositories
					if (repositories != null)
					{
						repositories.Clear();
					}

					// dispose the db context.
					_context.Dispose();
				}
			}

			disposed = true;
		}
	}
}