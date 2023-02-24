﻿using ApiCatalago.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ApiCatalago.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApiCatalagoDbContext _context;

        public Repository(ApiCatalagoDbContext context)
        {
            _context = context;
        }
        public T GetById(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsNoTracking();
        }


        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
             _context.Set<T>().Remove(entity);
        }

       
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
        }
    }
}
