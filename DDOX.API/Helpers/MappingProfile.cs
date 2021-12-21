using AutoMapper;
using DDOX.API.Core.Models.Category;
using DDOX.API.Core.Models.Extension;
using DDOX.API.Core.Models.File;
using DDOX.API.Core.Models.Folder;
using DDOX.API.Core.Models.FolderCategory;
using DDOX.API.Core.Models.FolderIndices;
using DDOX.API.Core.Models.Index;
using DDOX.API.Core.Models.IndexRestriction;
using DDOX.API.Core.Models.Partition;
using DDOX.API.Dtos.Index;
using DDOX.API.Dtos.IndexRestection;
using DDOX.API.Dtos.Partiton;
using DDOX.API.Infrastructure.Enums;
using DDOX.API.Dtos.Category;
using DDOX.API.Dtos.Folder;
using DDOX.API.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDOX.API.Dtos.Extension;
using DDOX.API.Dtos.Partition;
using DDOX.API.Core.Models.Restriction;
using DDOX.API.Dtos.Restriction;
using DDOX.API.Dtos.File;
using DDOX.API.Dtos.FileIndicesCore;
using DDOX.API.Core.Models.FileIndices;
using DDOX.API.Dtos.FolderIndices;
using DDOX.API.Dtos.FolderCategories;
using DDOX.API.Dtos.FileIndices;
using DDOX.API.Dtos.CategoryIndices;
using DDOX.API.Core.Models.Page;
using DDOX.API.Dtos.Page;
using DDOX.API.Dtos.PageIndices;
using DDOX.API.Core.Models;
using System;
using Index = DDOX.API.Infrastructure.Models.Index;

namespace DDOX.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Folder
            CreateMap<Folder, FolderCore>();
            CreateMap<FolderCore, Folder>()
                .ForMember(d => d.SubFolders, opt => opt.Ignore());
            CreateMap<FolderToCreateDto, FolderCore>()
                .AfterMap((s, d) => d.IsActive = true)
                .AfterMap((s, d) => d.CreatedAt = DateTime.Now);
            CreateMap<FolderToUpdateDto, FolderCore>()
                .AfterMap((s, d) => d.UpdatedAt = DateTime.Now);
            CreateMap<FolderCore, FolderToDisplayDto>();


            //FolderCategory
            CreateMap<FolderCategoriesDto, FolderCategoriesCore>().ReverseMap();
            CreateMap<FolderCategoriesToAddDto, FolderCategoriesCore>();
            CreateMap<FolderCategoriesCore, FolderCategories>().ReverseMap();

            //FolderIndices
            CreateMap<FolderIndicesDto, FolderIndicesCore>().ReverseMap();
            CreateMap<FolderIndicesToCreateDto, FolderIndicesCore>();
            CreateMap<FolderIndicesCore, FolderIndices>().ReverseMap();

            //File
            CreateMap<File, FileCore>().ReverseMap();
            CreateMap<FileCore, FileToDisplayDto>();
            CreateMap<FileToCreateDto, FileCore>()
                .AfterMap((s, d) => d.IsActive = true)
                .AfterMap((s, d) => d.CreatedAt = DateTime.Now);
            CreateMap<FileToUpdateDto, FileCore>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .AfterMap((s, d) => d.UpdatedAt = DateTime.Now);
            CreateMap<FileCore, FolderCore>()
                .ForMember(d => d.Name, opt => opt.Ignore());

            //Extension
            CreateMap<Extension, ExtensionCore>().ReverseMap();
            CreateMap<ExtensionCore, ExtensionToDisplayDto>();

            //Partition
            CreateMap<Partition, PartitionCore>().ReverseMap();
            CreateMap<PartitionToCreateDto, PartitionCore>()
                .AfterMap((s, d) => d.IsEncrypted = true)
                .AfterMap((s, d) => d.CreatedAt = DateTime.Now);
            CreateMap<PartitionCore, PartitionToDisplayDto>();
            CreateMap<PartitionToUpdateDto, PartitionCore>()
                .AfterMap((s, d) => d.UpdatedAt = DateTime.Now);


            //Index
            CreateMap<Index, IndexCore>().ReverseMap();
            CreateMap<IndexCore, IndexToDisplayDto>();
            CreateMap<IndexToCreateDto, IndexCore>()
                .AfterMap((s, d) => d.IsActive = true)
                .AfterMap((s, d) => d.CreatedAt = DateTime.Now);
            CreateMap<IndexToUpdateDto, IndexCore>()
                .AfterMap((s, d) => d.UpdatedAt = DateTime.Now);

            //IndexRestriction
            CreateMap<IndexRestrictionToAddDto, IndexRestrictionCore>();
            CreateMap<IndexRestrictionDto, IndexRestrictionCore>().ReverseMap();
            CreateMap<IndexRestrictionCore, IndexRestrictions>().ReverseMap();


            //Category

            CreateMap<Category, CategoryCore>().ReverseMap();
            CreateMap<CategoryToCreateDto, CategoryCore>()
                .AfterMap((s, d) => d.IsActive = true)
                .AfterMap((s, d) => d.CreatedAt = DateTime.Now);
            CreateMap<CategoryToUpdateDto, CategoryCore>()
                .AfterMap((s, d) => d.UpdatedAt = DateTime.Now);
            CreateMap<CategoryCore, CategoryToDisplayDto>();

            //CategoryIndices
            CreateMap<CategoryIndicesCore, CategoryIndices>().ReverseMap();
            CreateMap<CategoryIndicesDto, CategoryIndicesCore>().ReverseMap();
            CreateMap<CategoryIndicesToAddDto, CategoryIndicesCore>();

            //Resteriction
            CreateMap<Restriction, RestrictionCore>().ReverseMap();
            CreateMap<RestrictionCore, RestrictionToDisplayDto>();

            //FileIndices
            CreateMap<FileIndices, FileIndicesCore>().ReverseMap();
            CreateMap<FileIndicesCore, FileIndicesDto>().ReverseMap();
            CreateMap<FileIndicesToAddDto, FileIndicesCore>();

            //Page
            CreateMap<PageCore, Page>().ReverseMap();
            CreateMap<PageToAddDto, PageCore>();
            CreateMap<PageDto, PageCore>().ReverseMap();


            //PageIndices
            CreateMap<PageIndicesCore, PageIndices>().ReverseMap();
            CreateMap<PageIndicesToAddDto, PageIndicesCore>();
            CreateMap<PageIndicesDto, PageIndicesCore>().ReverseMap();







        }
    }
}
