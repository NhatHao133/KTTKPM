using ASC.DataAccess.Interfaces;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.DataAccess
{
	public class UnitOfWork : IUniOfWork
	{
		private Dictionary<string, object> _repositories;
		private DbContext _dbContext;
		public UnitOfWork(DbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public int CommitTransaction()
		{
			return _dbContext.SaveChanges();
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				_dbContext.Dispose();
			}
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		public IRespository<T> Respository<T>() where T : BaseEntity
		{
			if (_repositories == null)
				_repositories = new Dictionary<string, object>();
			var type = typeof(T).Name;
			if (_repositories.ContainsKey(type))
				return (IRespository<T>) _repositories[type];
			var repostitoryType = typeof(Repository<>);
			var repostitoryInstance = Activator.CreateInstance(repostitoryType.MakeGenericType(typeof(T)), _dbContext);
			_repositories.Add(type,repostitoryInstance);
			return (IRespository<T>)_repositories[type];
		}
	}
}
