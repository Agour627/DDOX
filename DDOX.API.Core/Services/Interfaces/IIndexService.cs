using DDOX.API.Core.Models.Index;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
    public interface IIndexService
    {
        Task<bool> CreateIndex(IndexCore indexCore);
        Task<IndexCore> GetIndexById(int id);
        Task<List<IndexCore>> GetAllIndices();
        Task<List<IndexCore>> GetPartitionIndices(int partitionId);
        Task<bool> UpdateIndex(int id, IndexCore indexCore);
    }
}
