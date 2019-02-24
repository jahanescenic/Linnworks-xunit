using Xunit;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LinnworksTest.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LinnworksTest;
using Moq;
using LinnworksTest.Controllers;
using LinnworksTest.Models;
using System.Linq;

namespace linnworks_xunit
{
    public class CategoryControllerTest
    {
        public List<LinnworksTest.DataAccess.Category> _categorylist;
        public LinnworksTest.DataAccess.Category _category;


        [Fact]
        public async Task Get_WhenCalled_ReturnsList()
        {
            // Arrange
            _categorylist = new List<LinnworksTest.DataAccess.Category>()
                {
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                        CategoryName = "Orange Juice" },
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                        CategoryName = "Diary Milk"},
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                        CategoryName = "Frozen Pizza" }
                };
            var category = new CategoryWithStock { CategoryName = "Category" };
            var mockRepo = new Mock<IGenericRepository<LinnworksTest.DataAccess.Category>>();
            mockRepo.Setup(repo => repo.GetAllAsync())
                     .ReturnsAsync(_categorylist);

            var controller = new CategoryController(mockRepo.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var model = Assert.IsAssignableFrom<IEnumerable<CategoryWithStock>>(
                result);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task ForId_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
             _categorylist = new List<LinnworksTest.DataAccess.Category>()
                {
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                        CategoryName = "Orange Juice" },
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                        CategoryName = "Diary Milk"},
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                        CategoryName = "Frozen Pizza" }
                };

            Guid CatId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c201");

            var mockRepo = new Mock<IGenericRepository<LinnworksTest.DataAccess.Category>>();
            mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_categorylist.Where(a => a.Id == CatId)
            .FirstOrDefault());

            String id = CatId.ToString();

            var controller = new CategoryController(mockRepo.Object);

            // Act
            var result = await controller.Details(id);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundResult>(result);
            
        }

        [Fact]
        public async Task ForId_ReturnsCategoryForId()
        {
            // Arrange
            _categorylist = new List<LinnworksTest.DataAccess.Category>()
                {
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                        CategoryName = "Orange Juice" },
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                        CategoryName = "Diary Milk"},
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                        CategoryName = "Frozen Pizza" }
                };

            Guid CatId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            var mockRepo = new Mock<IGenericRepository<LinnworksTest.DataAccess.Category>>();
            mockRepo.Setup(repo => repo.GetByIdAsync(CatId))
                .ReturnsAsync(_categorylist.Where(a => a.Id == CatId)
            .FirstOrDefault());

            var controller = new CategoryController(mockRepo.Object);
            String id = CatId.ToString();

            // Act
            var result = await controller.Details(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);           
        }

        [Fact]
        public async Task ForId_DeleteCategoryForId()
        {
            // Arrange
            _categorylist = new List<LinnworksTest.DataAccess.Category>()
                {
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                        CategoryName = "Orange Juice" },
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f"),
                        CategoryName = "Diary Milk"},
                    new LinnworksTest.DataAccess.Category() { Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad"),
                        CategoryName = "Frozen Pizza" }
                };

            Guid CatId = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200");

            var mockRepo = new Mock<IGenericRepository<LinnworksTest.DataAccess.Category>>();
            mockRepo.Setup(repo => repo.DeleteAsync(CatId))
                .Returns(Task.CompletedTask);

            var controller = new CategoryController(mockRepo.Object);
            String id = CatId.ToString();

            // Act
            var result = await controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
        }

    }
}