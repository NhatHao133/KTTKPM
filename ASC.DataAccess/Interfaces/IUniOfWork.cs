using ASC.Model.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.DataAccess.Interfaces;

namespace ASC.DataAccess.Interfaces
{
    public interface IUniOfWork : IDisposable
    {
        IRespository<T> Respository<T>() where T : BaseEntity;
        public int CommitTransaction();
    }
}
