using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Repository.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Save();
    }
}
