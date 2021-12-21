using AutoMapper;
using DDOX.API.Core.Models.Restriction;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services
{
    public class RestrictionService : IRestrictionService
    {

        public readonly IGenericRepository<Restriction> _restrictionRepository;
        public readonly IMapper _mapper;
        public RestrictionService(
                                  IGenericRepository<Restriction> restrictionRepository,
                                  IMapper mapper)
                               
        {
            _restrictionRepository = restrictionRepository;
            _mapper = mapper;
        }

        public async Task<List<RestrictionCore>> GetAllRestrictions()
        {
            var restictions = await _restrictionRepository.GetAll();
            if (restictions.LongCount() == 0)
            {
                Log.Information($"Database Doesn't Contain Restrictions");
                return null;
            }
            return _mapper.Map<List<RestrictionCore>>(restictions);
        }
    }
}
