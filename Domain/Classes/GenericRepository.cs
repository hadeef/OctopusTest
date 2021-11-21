using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class GenericRepository<TEntity, Tstring, Tdate> : IGenericRepository<TEntity, Tstring, Tdate> where TEntity : class
    {
        protected IList<TEntity> _entities;

        public abstract TEntity GetById(Tstring id);
        public IList<TEntity> GetAll()
        {
            return _entities;
        }

        public void Add(TEntity obj)
        {
            _entities.Add(obj);
        }
  
        public abstract bool Remove(Tstring id);

    }
}
