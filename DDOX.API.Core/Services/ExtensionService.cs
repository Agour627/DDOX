using AutoMapper;
using DDOX.API.Core.Models.Extension;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Core.Services
{
    public class ExtensionService : IExtensionService
    {
        public readonly IGenericRepository<Extension> _extensionRepository;
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;

        public ExtensionService(IGenericRepository<Extension> extensionRepository,
                               IUnitOfWork unitOfWork,
                               IMapper mapper)
        {
            _extensionRepository = extensionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       
        public async Task<List<ExtensionCore>> GetAllExtentions()
        {
            var fetchedExtensions = await _extensionRepository.GetAll();
            if (fetchedExtensions.LongCount() == 0)
            {
                Log.Information($"Data Base doesn't contain Extensions");
                return null;
            }
            return _mapper.Map<List<ExtensionCore>>(fetchedExtensions.OrderBy(e=>e.Id));
        }

        public async Task<ExtensionCore> GetExtensionById(int id)
        {
            var fetchedExtension = await _extensionRepository.GetById(id);
            if (fetchedExtension == null)
            {
                Log.ForContext("Info", new { EntityId = id}, true)
                   .Information($"Data Base doesn't contain Extension: {id}");
                return null;
            }
            return _mapper.Map<ExtensionCore>(fetchedExtension);
        }

        public async Task<ExtensionCore> GetExtensionByFileName(string fileName)
        {
            var extensionName = Path.GetExtension(fileName);
            var fetchedExtension = await _extensionRepository.FindByCondition(e => e.Name == extensionName.ToLower());
            if (fetchedExtension is null || fetchedExtension.Count == 0)
            {
                Log.ForContext("Info", new { Entity= extensionName}, true)
                   .Information($"Data Base doesn't contain extension: {extensionName}");
                return null;
            }
            return _mapper.Map<ExtensionCore>(fetchedExtension.FirstOrDefault());
        }

        public async Task<bool> UpdateExtension(int id, ExtensionCore extensionCore)
        {
            var oldExtension = await _extensionRepository.GetById(id);
            var extensionToUpdate = _mapper.Map(extensionCore, oldExtension);
            await _extensionRepository.Update(extensionToUpdate);

            var isUpdated = await _unitOfWork.Save();
            if(isUpdated)
                Log.ForContext("Info", new { EntityId = id }, true)
                   .Information($"User successfully updated Extension: {id}");
            return isUpdated;
        }
    }
}
