﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Domains;

namespace DataAccess.Contracts
{
    public interface ITableRepository
        <T> where T : BaseTable
    {
        IEnumerable<T> GetAll();
        T? GetById(Guid id);
        bool Add(T entity);
        bool Add(T entity, out Guid id);

        bool Update(T entity);
        bool Delete(T entity);
        bool ChangeStatus(Guid id , Guid userId, int stutus = 1);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        Task<List<T>> GetList(Expression<Func<T, bool>> filter);
        Task<List<TResult>> GetList<TResult>(
          Expression<Func<T, bool>>? filter = null,
          Expression<Func<T, TResult>>? selector = null,
          Expression<Func<T, object>>? orderBy = null,
          bool isDescending = false,
          params Expression<Func<T, object>>[] includers);
        Task<PagedResult<TResult>> GetPagedList<TResult>(
   int pageNumber,
   int pageSize,
   Expression<Func<T, bool>>? filter = null,
   Expression<Func<T, TResult>>? selector = null,
   Expression<Func<T, object>>? orderBy = null,
   bool isDescending = false,
   params Expression<Func<T, object>>[] includers);




    }

}
