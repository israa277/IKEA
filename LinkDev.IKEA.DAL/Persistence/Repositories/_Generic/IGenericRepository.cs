﻿using LinkDev.IKEA.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.DAL.Persistence.Repositories._Generic
{
    public interface IGenericRepository<T> where T : ModelBase
    {
        T? Get(int id);
        IEnumerable<T> GetAll(bool withAsNoTracking = true);
        IQueryable<T> GetAllAsIQueryable();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

