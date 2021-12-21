using DDOX.API.Core.Models.Partition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
    public interface IPartitionSevice
    {
        Task<bool> CreatePartition(PartitionCore partitionCore);
        Task<bool> CheckPartitionPathDuplication(string partitionPath);
        Task<PartitionCore> GetPartitionById(int id);
        Task<List<PartitionCore>> GetAllPartitions();
        Task<bool> UpdatePartition(int id, PartitionCore partitionCore);
    }
}
