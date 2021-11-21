using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IGenericRepository<TEntity, T, T2> where TEntity : class
    {
        TEntity GetById(T id);
        IList<TEntity> GetAll();
        void Add(TEntity obj);
        bool Remove(T id);
    }
}
