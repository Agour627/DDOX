using AutoMapper;
using DDOX.API.Core.Models.Page;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services
{
    public class PageService : IPageService
    {
        private readonly IGenericRepository<Page> _pageRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PageService(IGenericRepository<Page> pageRepository,
                           IMapper mapper,
                           IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        public async Task<PageCore> SavePage(PageCore pageCore)
        {
            var pageToAdd = _mapper.Map<Page>(pageCore);
            var savedPage = await _pageRepository.Add(pageToAdd);
            var isAdded = await _unitOfWork.Save();
            if (!isAdded)
                return null;

            Log.ForContext("Info", new { EntityId = savedPage.Id, PageNumber = savedPage.PageNumber, FileId = savedPage.FileId }, true)
               .Information($"The Page Number: {savedPage.PageNumber} in file: {savedPage.FileId} have been saved successfully with id: {savedPage.Id}");
            return _mapper.Map<PageCore>(savedPage);
        }

        public async Task<List<PageCore>> GetPagesByFileId(int fileId)
        {
            var fetchedPages = await _pageRepository.FindByCondition(e => e.FileId == fileId, 
                                                              relation => relation.Include(e => e.PageIndices));

            if (fetchedPages.Count == 0)
            {
                Log.ForContext("Info", new { FileId = fileId }, true)
                    .Information($"The file: {fileId} doesn't have saved Pages");
                return null;
            }
            var pagesCore = _mapper.Map<List<PageCore>>(fetchedPages);
            return pagesCore;
        }

        public async Task<bool> UpdatePage(int id, PageCore pageCore)
        {
            var fetchedPage = await _pageRepository.FindByCondition(page => page.Id ==  id,
                                                                    page => page.Include(p => p.PageIndices));
            var pageToUpdate = _mapper.Map(pageCore, fetchedPage.FirstOrDefault());
            await _pageRepository.Update(pageToUpdate);

            var isUpdated = await _unitOfWork.Save();
            if (isUpdated)
                Log.ForContext("Info", new { EntityId = id, PageNumber = pageCore.PageNumber, FileId = pageCore.FileId }, true)
                    .Information($"User sucessfully update page: {id} in file: {pageCore.FileId}");
            return isUpdated;
        }
    }
}
