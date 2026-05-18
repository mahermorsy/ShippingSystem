using DataAccessLayer.Data;
using DataAccessLayer.Exceptions;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseTable
    {
        private readonly ShippingContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(ShippingContext context, ILogger<GenericRepository<T>> Logger)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _logger = Logger;
        }
        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception EX)
            {
                throw new DataAccessException(EX, "Error To Get Entities List", _logger);
            }
        }
        public async Task<T?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbSet.FindAsync(id).AsTask();
            }
            catch (Exception EX)
            {
                throw new DataAccessException(EX, "Error To Get Row by Id from Entity Table", _logger);
            }
        }
        public async Task<bool> AddAsync(T entity, Guid USerID)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = USerID;
                entity.CurrentState = 0;

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception EX)
            {
                throw new DataAccessException(EX, "Error To Add Row To Entity Table", _logger);
            }
        }
        public async Task<(bool Success, Guid EntityId)> AddAsyncGetID(T entity, Guid userId)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = userId;
                entity.CurrentState = 0;

                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                return (true, entity.Id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error To Add Row To Entity Table", _logger);
            }
        }
        public Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    return _context.SaveChangesAsync();
                }
            }
            catch (Exception EX)
            {
                throw new DataAccessException(EX, "Error To Delete Row From Entity Table", _logger);
            }

            return Task.CompletedTask;
        }
        public async Task UpdateAsync(T entity, Guid USerID)
        {
            try
            {
                var entDb = await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);

                if (entDb == null)
                {
                    return;
                }

                entity.CreatedDate = entDb.CreatedDate;
                entity.CreatedBy = entDb.CreatedBy;
                entity.UpdatedBy = USerID;
                entity.UpdatedDate = DateTime.Now;
                entity.CurrentState = entDb.CurrentState;

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception EX)
            {
                throw new DataAccessException(EX, "Error To Update Row From Entity Table", _logger);
            }
        }
        public async Task<int> ChangeStatus(Guid id, Guid USerID, int Status = 1)
        {
            try
            {
                var entity = await   _dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new DataAccessException(
                        new KeyNotFoundException($"Entity with id {id} was not found."),
                        "Error changing entity status",
                        _logger);
                }

                entity.CurrentState = Status;
                entity.UpdatedBy = USerID;
                entity.UpdatedDate = DateTime.Now;

                _context.Entry(entity).State = EntityState.Modified;
                return await _context.SaveChangesAsync();
            }
            catch (Exception EX)
            {
                lock (_logger)
                {
                    _logger.LogError(EX, $"Error changing status for entity with id {id}");
                }
                throw new DataAccessException(EX, "", _logger);
            }

        }
        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> Filter)
        {
            try
            {
                return await _dbSet.AsNoTracking().Where(Filter).FirstOrDefaultAsync();   
            }
            catch (Exception EX)
            {
                throw new DataAccessException(new Exception("No entity found matching the filter criteria."), "Error To Get Row by Filter from Entity Table", _logger);
            }
        }
        public async Task<List<T>> GetList(Expression<Func<T, bool>> Filter)
        {
            try
            {
                return await _dbSet.AsNoTracking().Where(Filter).ToListAsync();
            }
            catch (Exception EX)
            {
                throw new DataAccessException(new Exception("No entity found matching the filter criteria."), "Error To Get Row by Filter from Entity Table", _logger);
            }
        }
        public async Task<List<Tresult>> GetList<Tresult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, Tresult>> selector = null,
            Expression<Func<T, object>> orderBy = null,
            bool isDescending = false,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                if (includes != null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = isDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
                }

                if (selector != null)
                {
                    return await query.Select(selector).ToListAsync();
                }

                return await query.Cast<Tresult>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error To Get List by Filter from Entity Table", _logger);
            }
        }
        public async Task<Tresult> GetByIdAsync<Tresult>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, Tresult>> selector = null,
            Expression<Func<T, object>> orderBy = null,
            bool isDescending = false,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                if (includes != null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = isDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
                }

                if (selector != null)
                {
                    return await query.Select(selector).FirstOrDefaultAsync();
                }

                return await query.Cast<Tresult>().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error To Get List by Filter from Entity Table", _logger);
            }
        }

        public async Task<PagedResult<Tresult>> GetPagedList<Tresult>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, Tresult>> selector,
            int pageNumber = 1,
            int pageSize = 10,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false,
            params Expression<Func<T, object>>[] includes)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsNoTracking();

                if (includes != null && includes.Any())
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = isDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
                }

                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }

                if (pageSize < 1)
                {
                    pageSize = 10;
                }

                var totalCount = await query.CountAsync();
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(selector)
                    .ToListAsync();

                return new PagedResult<Tresult>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error To Get Paged List by Filter from Entity Table", _logger);
            }
        }

        public async Task<bool> UpdateFieldsync(Guid id, Action<T> updateAction)
        {
            try

            {
                // Get entity by id
                var entity = _dbSet.FirstOrDefault(e => e.Id == id);

                // If entity not found
                if (entity == null)
                    return false;

                // Apply updates from outside
                updateAction(entity);

                // Mark entity as modified
                _context.Entry(entity).State = EntityState.Modified;

                // Save changes
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
