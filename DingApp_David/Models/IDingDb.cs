using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DingApp_David.Models
{
    public interface IDingDb : IDisposable
    {
        IQueryable<T> Query<T>() where T : class;
        T Add<T>(T entity) where T : class;
    }
}
