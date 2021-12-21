using AutoMapper;
using DDOX.API.Core.Models.Folder;
using DDOX.API.Core.Services;
using DDOX.API.Helpers;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Repositories.Interfaces;
using DDOX.API.Test.DataGenerator;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DDOX.API.Test
{
    public class FolderServiceTests
    {

        #region Properties
        private readonly FolderService _folderSerivce;
        private readonly Mock<IGenericRepository<Folder>> _folderRepoMoc = new Mock<IGenericRepository<Folder>>();
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private static IMapper _mapper;
        #endregion

        #region ctro
        public FolderServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
            _folderSerivce = new FolderService(_folderRepoMoc.Object, _mapper, _unitOfWork.Object);
        }
        #endregion

        [Theory]
        [MemberData(nameof(GenerateFolderTests.GenerateGetFolderByIdReturnFolderCases), MemberType = typeof(GenerateFolderTests))]
        public async Task GetByIdAsync_ShouldReturnFolder_WhenFolderExists(int folderId, FolderCore expected, Folder inputFolder)
        {
            //Arrange
            _folderRepoMoc.Setup(e => e.GetById(folderId)).ReturnsAsync(inputFolder);
            //Act
            var actualResult = await _folderSerivce.GetFolder(folderId);
            //Assert
            actualResult.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [MemberData(nameof(GenerateFolderTests.GenerateGetFolderByIdReturnNullCases), MemberType = typeof(GenerateFolderTests))]
        public async Task GetByIdAsync_ShouldRetrunNull_WhenFolderNotExists(int folderId, Folder folder)
        {
            //Arrange
            _folderRepoMoc.Setup(e => e.GetById(It.IsAny<int>())).ReturnsAsync(folder);
            //Act
            var actualResult = await _folderSerivce.GetFolder(folderId);
            //Assert
            Assert.Null(actualResult);
        }

        [Theory]
        [MemberData(nameof(GenerateFolderTests.GenerateCreateFolderCases), MemberType = typeof(GenerateFolderTests))]
        public async Task CreateFolder_shouleReturnFolderCore(Folder output, FolderCore inputFolder, FolderCore outPutFolder)
        {
            //Arrange
            _folderRepoMoc.Setup(e => e.Add(It.IsAny<Folder>())).ReturnsAsync(output);
            _unitOfWork.Setup(e => e.Save()).ReturnsAsync(true);
            //Act
            var actualResult = await _folderSerivce.CreateFolder(inputFolder);
            //Assert
            actualResult.Should().BeEquivalentTo(outPutFolder);
        }

        [Theory]
        [MemberData(nameof(GenerateFolderTests.GenerateUpdateFolderCases), MemberType = typeof(GenerateFolderTests))]
        public async Task UpdateFolder_shouleReturnTrue(int folderId, Folder folderEntity, FolderCore inputFolder, List<Folder> folders)
        {
            //Arrange
            _folderRepoMoc.Setup(x => x.FindByCondition(
                                      It.IsAny<Expression<Func<Folder, bool>>>(),
                                      It.IsAny<Func<IQueryable<Folder>, IIncludableQueryable<Folder, object>>>(),
                                      It.IsAny<Func<IQueryable<Folder>, IOrderedQueryable<Folder>>>()))
                          .ReturnsAsync(folders);
            _folderRepoMoc.Setup(e => e.Update(folderEntity)).ReturnsAsync(folderEntity);
            _unitOfWork.Setup(e => e.Save()).ReturnsAsync(true);
            //Act
            var actualResult = await _folderSerivce.UpdateFolder(folderId, inputFolder);
            //Assert
            actualResult.Should().BeTrue();
        }

        [Fact]
        public void SaveFolder_shouleReturnFalse_WhenFolderPathIsExistsBefore()
        {
            //Act
            var actualResult = _folderSerivce.SaveFolder("E:\\Test");
            //Assert
            actualResult.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(GenerateFolderTests.GenerateGetFoldersInPartitionCases), MemberType = typeof(GenerateFolderTests))]
        public async Task GetFoldersInPartition_ShouldRetrunNull_WhenNoFoldersExistsInPartition(int folderId, List<Folder> folders)
        {
            //Arrange
            _folderRepoMoc.Setup(x => x.FindByCondition(
                                        It.IsAny<Expression<Func<Folder, bool>>>(),
                                        It.IsAny<Func<IQueryable<Folder>, IIncludableQueryable<Folder, object>>>(),
                                        It.IsAny<Func<IQueryable<Folder>, IOrderedQueryable<Folder>>>()))
                            .ReturnsAsync(folders);
            //Act 
            var actuaclResult = await _folderSerivce.GetFoldersInPartition(folderId);
            //Assert 
            actuaclResult.Should().BeNull();
        }

        [Fact]
        public async Task GetFolderById_shouleReturnFalse_WhenNoFolderExists()
        {
            //Arrange
            _folderRepoMoc.Setup(x => x.FindByCondition(
                                        It.IsAny<Expression<Func<Folder, bool>>>(),
                                        It.IsAny<Func<IQueryable<Folder>, IIncludableQueryable<Folder, object>>>(),
                                        It.IsAny<Func<IQueryable<Folder>, IOrderedQueryable<Folder>>>()))
                            .ReturnsAsync(new List<Folder>());
            //Act
            var actualResult = await _folderSerivce.GetFolderById(It.IsAny<int>());
            //Assert
            actualResult.Should().BeNull();
        }

    }
}
