using DDOX.API.Core.Models.Restriction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
    public interface IRestrictionService
    {
        Task<List<RestrictionCore>> GetAllRestrictions();
    }
}
