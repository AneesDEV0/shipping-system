using DataAccess.Context;
using DataAccess.Contracts;
using DataAccess.Exceptions;
using DataAccess.Models;
using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TableRepository<T> : ITableRepository<T> where T : BaseTable
    {
        protected readonly ShippingContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly ILogger<ITableRepository<T>> _logger;

        public TableRepository(ShippingContext context, ILogger<ITableRepository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                return _dbSet.Where(e=>e.CurrentState >0 ).ToList();

            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error retrieving all records", _logger);
            }
        }

        public T? GetById(Guid id)
        {
            try
            {
                return _dbSet.Where(a => a.Id == id).AsNoTracking().FirstOrDefault();

            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error retrieving record by ID", _logger);
            }
        }

        public bool Add(T entity)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;
                _dbSet.Add(entity);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error adding new record", _logger);
            }
        }

        public bool Add(T entity, out Guid id)
        {
            try
            {
                entity.CreatedDate = DateTime.Now;

                _dbSet.Add(entity);
                _context.SaveChanges();
                id = entity.Id;
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error adding new record", _logger);
            }
        }



        public bool Update(T entity)
        {
            try
            {
                var dbData = GetById(entity.Id);
                entity.CreatedDate = dbData.CreatedDate;
                entity.CreatedBy = dbData.CreatedBy;
                entity.UpdatedDate = DateTime.Now;
                entity.CurrentState = dbData.CurrentState;
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error updating record", _logger);
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                var data = GetById(entity.Id);
                if (data != null)
                {
                    _dbSet.Remove(entity);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error deleting record", _logger);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public bool ChangeStatus(Guid id, Guid userId, int status = 1)
        {
            try
            {
                var entity = GetById(id);
                if (entity != null)
                {
                    entity.CurrentState = status;
                    entity.UpdatedBy = id;
                    entity.UpdatedDate = DateTime.Now;
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "Error changing status", _logger);
            }


        }
        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            try
            {
                return _dbSet.Where(filter).AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        // Method to get a list of records based on a filter
        public async Task<List<T>> GetList(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.Where(filter).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }

        public async Task<List<TResult>> GetList<TResult>(
    Expression<Func<T, bool>>? filter = null,
    Expression<Func<T, TResult>>? selector = null,
    Expression<Func<T, object>>? orderBy = null,
    bool isDescending = false,
    params Expression<Func<T, object>>[] includers)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsQueryable();

                // Apply includes
                foreach (var include in includers)
                    query = query.Include(include);

                // Apply filter
                if (filter != null)
                    query = query.Where(filter);

                // Apply ordering
                if (orderBy != null)
                    query = isDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);

                query = query.AsNoTracking();

                // Apply projection
                if (selector != null)
                    return await query.Select(selector).ToListAsync();

                return await query.Cast<TResult>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger); // Or your custom exception
            }
        }

        public async Task<PagedResult<TResult>> GetPagedList<TResult>(
  int pageNumber,
  int pageSize,
  Expression<Func<T, bool>>? filter = null,
  Expression<Func<T, TResult>>? selector = null,
  Expression<Func<T, object>>? orderBy = null,
  bool isDescending = false,
  params Expression<Func<T, object>>[] includers)
        {
            try
            {
                IQueryable<T> query = _dbSet.AsQueryable();

                // Apply includes
                foreach (var include in includers)
                    query = query.Include(include);

                // Apply filter
                if (filter != null)
                    query = query.Where(filter);

                // Total count before pagination
                int totalCount = await query.CountAsync();

                // Apply ordering
                if (orderBy != null)
                {
                    query = isDescending
                        ? query.OrderByDescending(orderBy)
                        : query.OrderBy(orderBy);
                }

                query = query.AsNoTracking();

                // Apply paging
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);

                // Apply projection
                List<TResult> items;
                if (selector != null)
                    items = await query.Select(selector).ToListAsync();
                else
                    items = await query.Cast<TResult>().ToListAsync();

                // Calculate total pages
                int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return new PagedResult<TResult>
                {
                    Items = items,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, "", _logger);
            }
        }




    }
}
