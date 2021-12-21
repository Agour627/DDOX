using AutoMapper;
using DDOX.API.Core.Models.Index;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Enums;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services
{
    public class IndexService : IIndexService
    {
        private readonly IGenericRepository<Index> _indexRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public IndexService (IGenericRepository<Index> indexRepository,
                             IMapper mapper,
                             IUnitOfWork unitOfWork)
        {
            _indexRepository = indexRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateIndex(IndexCore indexCore)
        {
            var indexToAdd = _mapper.Map<Index>(indexCore);
            var createdIndex = await _indexRepository.Add(indexToAdd);
            var isAdded = await _unitOfWork.Save();
            if (isAdded)
                Log.ForContext("Info", new { EntityId = createdIndex.Id }, true)
                    .Information($"User:{indexCore.CreatedBy} Successfully  Added Index :{createdIndex.Id}");
            return isAdded;
        }

        public async Task<List<IndexCore>> GetPartitionIndices(int partitionId)
        {
            var fetchedIndices = await _indexRepository.FindByCondition(index => index.PartitionId == partitionId && index.IsActive);
            if (fetchedIndices.Count == 0)
            {
                Log.ForContext("Info", new { EntityId = partitionId }, true)
                   .Information($"Partition :{partitionId} Doesn't Contain Indicies");
                return null;
            }
            return _mapper.Map<List<IndexCore>>(fetchedIndices.ToList());
        }

        public async Task<IndexCore> GetIndexById(int id)
        {
            var fetchedIndex = await _indexRepository.FindByCondition(index => index.Id == id,
                                                                      index => index.Include(x => x.IndexRestrictions));
            if (fetchedIndex.Count == 0)
            {
                Log.ForContext("Info", new { EntityId = id }, true)
                    .Information($"Database Doesn't Contain Index: {id}");
                return null;
            }
            return _mapper.Map<IndexCore>(fetchedIndex.FirstOrDefault());
        }

        public async Task<bool> UpdateIndex(int id, IndexCore indexCore)
        {
            var oldIndex = _indexRepository.FindByCondition(e => e.Id == id, w => w.Include(d => d.IndexRestrictions)).Result;
            var indexToUpdate = _mapper.Map(indexCore, oldIndex.FirstOrDefault());
            await _indexRepository.Update(indexToUpdate);
          
            var isUpdated = await _unitOfWork.Save();
            if (isUpdated)
                Log.ForContext("Info", new { EntityId = id }, true)
                    .Information($"User: {indexCore.UpdatedBy} Successfully  Updated Index :{id}");
            return isUpdated;
        }

        public async Task<List<IndexCore>> GetAllIndices()
        {
            var fetchedIndices = await _indexRepository.GetAll();

            if (fetchedIndices.LongCount() == 0)
            {
                Log.Information($"Database Doesn't Contain Categories");
                return null;
            }
            return _mapper.Map<List<IndexCore>>(fetchedIndices);
        }
    }
}
