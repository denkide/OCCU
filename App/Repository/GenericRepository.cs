using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCApp.Data;
using MVCApp.Interfaces;
//using COREApi.Web.Models;

namespace CoreAPI.Repository.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private MVCAppContext _context;

        public GenericRepository(MVCAppContext context){ _context = context; }

        public void UpdateObjectByID<T>(T obj, Func<T, bool> filter)
            where T : class
        {
            DbSet<T> _currentEntity;
            _currentEntity = _context.Set<T>();
            try
            {
                var currentObj = _currentEntity.Where(filter).FirstOrDefault();
                _context.Entry(currentObj).CurrentValues.SetValues(obj);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> AddObject<T>(T obj)
            where T : class
        {
            DbSet<T> currentEntity;
            currentEntity = _context.Set<T>();
            try
            {
                var retVal = await Task<T>.Factory.StartNew(() => currentEntity.Add(obj).Entity);
                _context.SaveChanges();

                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<T> ReturnFirstOrDefault<T>(Expression<Func<T, bool>> filter)
            where T : class
        {
            DbSet<T> currentEntity = _context.Set<T>() as DbSet<T>;
            CancellationToken token = new CancellationToken();
            return await currentEntity.SingleOrDefaultAsync(filter,token);  
        }

        public void DeleteObj<T>(T obj) where T : class
        {
            DbSet<T> currentEntity = _context.Set<T>() as DbSet<T>;
            currentEntity.Remove(obj);
            _context.SaveChanges();
        }
        public IQueryable<T> ReturnAllAsQueryable<T>() 
            where T : class
        {
            DbSet<T> _currentEntity;
            _currentEntity = _context.Set<T>();

            try
            {
                return from t in _currentEntity select t;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
