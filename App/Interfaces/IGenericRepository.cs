using MVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MVCApp.Interfaces
{
    public interface IGenericRepository
    {
        IQueryable<T> ReturnAllAsQueryable<T>() where T : class;

        //IEnumerable<T> ReturnList<T>(Func<T, bool> filter) where T : class;

        Task<T> ReturnFirstOrDefault<T>(Expression<Func<T, bool>> filter) where T : class;

        void UpdateObjectByID<T>(T obj, Func<T, bool> filter) where T : class;

        Task<T> AddObject<T>(T obj) where T : class;

        void DeleteObj<T>(T obj) where T : class;

        //IQueryable<T> ReturnQueryableList<T>(Func<T, bool> filter) where T : class;

        //IQueryable<T> ReturnQueryableFirstOrDefault<T>(Func<T, bool> filter) where T : class;

        

    }
}