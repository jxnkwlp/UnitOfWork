using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Passingwind.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

		int SaveChanges(bool ensureAutoHistory = false);

		Task<int> SaveChangesAsync(bool ensureAutoHistory = false, CancellationToken cancellationToken = default(CancellationToken));

		int ExecuteSqlCommand(string sql, params object[] parameters);

		IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;

		//IDbContextTransaction BeginTransaction();

		/// <summary>
		/// Uses TrakGrap Api to attach disconnected entities
		/// </summary>
		/// <param name="rootEntity"> Root entity</param>
		/// <param name="callback">Delegate to convert Object's State properities to Entities entry state.</param>
		void TrackGraph(object rootEntity, Action<EntityEntryGraphNode> callback);

	}

	public interface IUnitOfWork<TContext> : IUnitOfWork
	{
		TContext DbContext { get; }

		Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks);

	}
}