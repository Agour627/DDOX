using DDOX.API.Core.Models.File;
using DDOX.API.Core.Models.Folder;
using DDOX.API.Core.Models.FolderCategory;
using DDOX.API.Core.Models.FolderIndices;
using DDOX.API.Infrastructure.Enums;
using DDOX.API.Infrastructure.Models;
using System.Collections.Generic;

namespace DDOX.API.Test.DataGenerator
{
    public class GenerateFolderTests
    {
        public static IEnumerable<object[]> GenerateGetFolderByIdReturnFolderCases()
        {
            yield return new object[]
            {
                1,//folderId
                new FolderCore()
                {
                    Id=1,
                    Files=new List<FileCore>(),
                    FolderIndices= new List<FolderIndicesCore>(),
                    FolderCategories=new List<FolderCategoriesCore>()
                },// expected 
                new Folder(){ Id=1 }//input Folder
            };
        }
        public static IEnumerable<object[]> GenerateGetFolderByIdReturnNullCases()
        {
            yield return new object[]
            {
                 -1,
                 null
            };
        }
        public static IEnumerable<object[]> GenerateCreateFolderCases()
        {
            yield return new object[]
            {

                new Folder(){
                    Id=100,
                    Name="test",
                    PartitionId=2,
                    ParentId=null,
                    FolderType= FolderType.Folder,
                    IsActive =true,
                    FolderCategories = new List<FolderCategories>(),
                    FolderIndices =new List<FolderIndices>(),
                    SubFolders = new List<Folder>(),
                    Files = new List<File>(),
                    ParentFolder = null,
                    CreatedBy =1,
                    UpdatedBy = 1,
                },
                new FolderCore(){
                    Name="test",
                    PartitionId=2,
                    ParentId=null,
                    FolderType= FolderType.Folder,
                    IsActive =true,
                    FolderCategories = new List<FolderCategoriesCore>(),
                    FolderIndices =new List<FolderIndicesCore>(),
                    SubFolders = new List<FolderCore>(),
                    Files = new List<FileCore>(),
                    CreatedBy =1,
                    UpdatedBy = 1,
                    SeperatorLevel = 0
                },
                 new FolderCore(){
                    Id=100,
                    Name="test",
                    PartitionId=2,
                    ParentId=null,
                    FolderType= FolderType.Folder,
                    IsActive =true,
                    FolderCategories = new List<FolderCategoriesCore>(),
                    FolderIndices =new List<FolderIndicesCore>(),
                    SubFolders = new List<FolderCore>(),
                    Files = new List<FileCore>(),
                    CreatedBy =1,
                    UpdatedBy = 1,
                    SeperatorLevel = 0
                },

            };
        }
        public static IEnumerable<object[]> GenerateUpdateFolderCases()
        {
            yield return new object[]
            {
                1,
                new Folder(){Id=1},
                new FolderCore(){Id=1 },
                new List<Folder>(){ new Folder() {Id=1 } }
            };
        }
        public static IEnumerable<object[]> GenerateGetFoldersInPartitionCases()
        {
            yield return new object[]
            {
                1,
                new List<Folder>()
            };

            yield return new object[]
            {
                0,
                new List<Folder>()
            };

        }
    }
}
