using DDOX.API.Core.Models.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services.Interfaces
{
    public interface IPageService
    {
        Task<PageCore> SavePage(PageCore pageCore);
        Task<bool> UpdatePage(int id, PageCore pageCore);
        Task<List<PageCore>> GetPagesByFileId(int fileId);
    }
}
