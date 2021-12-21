using AutoMapper;
using DDOX.API.Core.Models.Partition;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services
{
    public class PartitionSevice : IPartitionSevice
    {
        public readonly IGenericRepository<Partition> _partitionRepository;
        public readonly IMapper _mapper;
        public readonly IUnitOfWork _unitOfWork;
        public PartitionSevice(
                            IGenericRepository<Partition> partitionRepository,
                            IMapper mapper,
                            IUnitOfWork unitOfWork)
        {
            _partitionRepository = partitionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckPartitionPathDuplication(string partitionPath)
        {
            var isExists = await _partitionRepository.FindByCondition(e => e.Path.ToLower() == partitionPath.ToLower());
            return isExists.Count > 0;
        }

        public async Task<bool> CreatePartition(PartitionCore partitionCore)
        {
            if (!Directory.Exists(partitionCore.Path))
            {
                Log.ForContext("Info", new { Path = partitionCore.Path }, true)
                    .Information($"This Partition Path : {partitionCore.Path } , Is Not Exists");
                return false;
            }
            var partitionToAdd = _mapper.Map<Partition>(partitionCore);
            var addedPartition = await _partitionRepository.Add(partitionToAdd);
            var isAdded = await _unitOfWork.Save();
            if (isAdded)
                Log.ForContext("Info", new { EntityId = addedPartition.Id }, true)
                   .Information($"User : {addedPartition.CreatedBy} Successfully Added Partition {addedPartition.Id} ");
            return isAdded;
        }

        public async Task<List<PartitionCore>> GetAllPartitions()
        {
            var fetchedPartitions = await _partitionRepository.GetAll();
            if (fetchedPartitions.LongCount() == 0)
            {
                Log.Information($"Data Base doesn't contain partitions");
                return null;
            }
            return _mapper.Map<List<PartitionCore>>(fetchedPartitions);
        }

        public async Task<PartitionCore> GetPartitionById(int id)
        {
            var fetchedPartition = await _partitionRepository.GetById(id);
            if (fetchedPartition is null)
            {
                Log.ForContext("Info", new { EntityId = id }, true)
                   .Information($"Database Doesn't Contain Partition : {id}");
                return null;
            }
            return _mapper.Map<PartitionCore>(fetchedPartition);
        }

        public async Task<bool> UpdatePartition(int id, PartitionCore partitionCore)
        {
            var oldPartition = await _partitionRepository.GetById(id);
            var partitionToUpdate = _mapper.Map(partitionCore, oldPartition);
            await _partitionRepository.Update(partitionToUpdate);

            var isUpdated = await _unitOfWork.Save();
            if (isUpdated)
                Log.ForContext("Info", new { EntityId = id }, true)
                   .Information($"User : {partitionCore.UpdatedBy} Successfully Updated Partition : {id}");
            return isUpdated;
        }
    }
}
