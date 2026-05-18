using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Domains.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShippingContext _DBContext;
        private readonly ConcurrentDictionary<Type, object> _repositories = new ConcurrentDictionary<Type, object>();
        private IDbContextTransaction? _transaction; 
        private readonly ILoggerFactory _logger;


        public UnitOfWork(ShippingContext DBContext, ILoggerFactory logger)
        {
            _DBContext = DBContext;
            _logger = logger;
        }
        public IGenericRepository<T> Repository<T>() where T : BaseTable
        {
            return (IGenericRepository<T>)_repositories.GetOrAdd(
                typeof(T),_=> new GenericRepository<T>(_DBContext,_logger.CreateLogger<GenericRepository<T>>()));
        
        }
        /*public IGenericRepository<T> Repository<T>() where T : BaseTable
        {
            return (IGenericRepository<T>)_repositories.GetOrAdd(typeof(T), (type) =>
              {
                  var repositoryType = typeof(GenericRepository<>).MakeGenericType(type);
                  return Activator.CreateInstance(repositoryType, _DBContext, _logger) ?? throw new InvalidOperationException($"Could not create repository for type {type.FullName}");
              });
        }*/
        public async Task BeginTransactionAsync()
        {
            _transaction = await _DBContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _DBContext.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
           if(_transaction != null) 
                await _transaction.DisposeAsync();
            await _DBContext.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _DBContext.SaveChangesAsync();
        }
    }
}
