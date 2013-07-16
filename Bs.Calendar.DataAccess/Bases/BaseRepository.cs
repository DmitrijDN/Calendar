﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Bs.Calendar.Models.Bases;

namespace Bs.Calendar.DataAccess.Bases
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntityId
    {
        private readonly CalendarContext _context;

        protected BaseRepository(CalendarContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public IQueryable<T> Load()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> Load(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Delete(T entity)
        {
            //_context.Set<T>().Attach(entity);
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void Save(T entity)
        {
            if (entity.Id == 0)
            {
                _context.Set<T>().Add(entity);
            }
            else
            {
                _context.Entry(entity).State = System.Data.EntityState.Modified;
            }

            _context.SaveChanges();
        }
    }
}