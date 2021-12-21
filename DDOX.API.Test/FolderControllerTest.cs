using AutoMapper;
using DDOX.API.Controllers;
using DDOX.API.Core.Models.Folder;
using DDOX.API.Core.Models.Partition;
using DDOX.API.Core.Services.Interfaces;
using DDOX.API.Dtos.Folder;
using DDOX.API.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DDOX.API.Test.DataGenerator
{
    public class FolderControllerTest
    {
        private readonly FolderController _folderController;
        private readonly Mock<IFolderService> _folderServiceMock = new Mock<IFolderService>();
        private readonly IMapper _mapper;
        private readonly Mock<IPartitionSevice> _partitionSeviceMock = new Mock<IPartitionSevice>();

        public FolderControllerTest()
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
            _folderController = new FolderController(_folderServiceMock.Object, _mapper, _partitionSeviceMock.Object);
        }

        //Test
        [Theory]
        [MemberData(nameof(GenerateFolderControllerTest.GenerateCreateFolderCases), MemberType = typeof(GenerateFolderControllerTest))]
        public async Task CreateFolderTest_ShouldReturnStatusCode201_WhenFolderCreated(FolderToCreateDto folderToCreate, FolderCore folderCore)
        {
            //Arrange
            _folderServiceMock.Setup(e => e.GetFolderById(It.IsAny<int>())).ReturnsAsync(folderCore);
            _folderServiceMock.Setup(e => e.GetFolderPath(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(string.Empty);
            _folderServiceMock.Setup(e => e.SaveFolder(It.IsAny<string>())).Returns(true);
            _folderServiceMock.Setup(e => e.CreateFolder(It.IsAny<FolderCore>())).ReturnsAsync(folderCore);
            _partitionSeviceMock.Setup(e => e.GetPartitionById(It.IsAny<int>())).ReturnsAsync(new PartitionCore());
            //Act
            var actualResult = await _folderController.CreateFolder(folderToCreate);
            //Assert
            actualResult.Should().BeOfType<StatusCodeResult>()
           .Which.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }


    }
}
