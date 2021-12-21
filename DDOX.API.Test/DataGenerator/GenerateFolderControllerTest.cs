using DDOX.API.Core.Models.Folder;
using DDOX.API.Dtos.Folder;
using DDOX.API.Dtos.FolderIndices;
using DDOX.API.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Test.DataGenerator
{
    public class GenerateFolderControllerTest
    {
        public static IEnumerable<object[]> GenerateCreateFolderCases()
        {
            yield return new object[]{
               new FolderToCreateDto()
               {
                   Name="Folder1",
                   CreatedBy=1,
                   FolderType=FolderType.Folder,
                   FolderCategories =new List<FolderCategoriesToAddDto>(),
                   FolderIndices = new List<FolderIndicesToCreateDto>(),
                   ParentId =2,
                   PartitionId=2
               },
               new FolderCore()
               {
                   Id=1,
                   FolderType=FolderType.Folder
               }
            };

            yield return new object[]{
               new FolderToCreateDto()
               {
                   Name="Folder1",
                   CreatedBy=1,
                   FolderType=FolderType.Folder,
                   FolderCategories =new List<FolderCategoriesToAddDto>(),
                   FolderIndices = new List<FolderIndicesToCreateDto>(),
                   ParentId =1 ,
                   PartitionId=2
               },
               new FolderCore()
               {
                   Id=1,
                   FolderType=FolderType.Folder
               }
            };

        }
    }
}
