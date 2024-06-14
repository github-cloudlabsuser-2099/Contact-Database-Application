using NUnit.Framework;
using CRUD_application_2.Controllers;
using CRUD_application_2.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CRUD_application_2.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController controller;

        [SetUp]
        public void Setup()
        {
            // Initialize UserController and add test data
            controller = new UserController();
            UserController.userlist = new List<User>
            {
                new User { Id = 1, Name = "Test User 1", Email = "test1@example.com" },
                new User { Id = 2, Name = "Test User 2", Email = "test2@example.com" }
            };
        }

        [Test]
        public void Index_ReturnsView_WithAllUsers()
        {
            // Act
            var result = controller.Index() as ViewResult;
            var model = result.Model as List<User>;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(2));
        }

        [Test]
        public void Details_ValidId_ReturnsUser()
        {
            // Act
            var result = controller.Details(1) as ViewResult;
            var model = result.Model as User;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(model.Id, Is.EqualTo(1));
        }

        [Test]
        public void Details_InvalidId_ReturnsHttpNotFound()
        {
            // Act
            var result = controller.Details(99);

            // Assert
            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }

        [Test]
        public void Create_Get_ReturnsCreateView()
        {
            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void Create_Post_ValidModel_AddsUser_RedirectsToIndex()
        {
            // Arrange
            var newUser = new User { Name = "New User", Email = "newuser@example.com" };

            // Act
            var result = controller.Create(newUser) as RedirectToRouteResult;

            // Assert
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(UserController.userlist, Contains.Item(newUser));
        }

        [Test]
        public void Create_Post_InvalidModel_ReturnsCreateView()
        {
            // Arrange
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = controller.Create(new User()) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void Edit_Get_ValidId_ReturnsEditView()
        {
            // Act
            var result = controller.Edit(1) as ViewResult;

            // Assert
            Assert.That(result.Model, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<User>());
        }

        [Test]
        public void Edit_Get_InvalidId_ReturnsHttpNotFound()
        {
            // Act
            var result = controller.Edit(99);

            // Assert
            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }

        [Test]
        public void Edit_Post_ValidModel_UpdatesUser_RedirectsToIndex()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Name = "Updated User", Email = "updated@example.com" };

            // Act
            var result = controller.Edit(1, updatedUser) as RedirectToRouteResult;

            // Assert
            var user = UserController.userlist.FirstOrDefault(u => u.Id == 1);
            Assert.That(user.Name, Is.EqualTo("Updated User"));
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void Edit_Post_InvalidModel_ReturnsEditView()
        {
            // Arrange
            controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = controller.Edit(1, new User()) as ViewResult;

            // Assert
            Assert.That(result.ViewName, Is.EqualTo("Edit"));
        }

        [Test]
        public void Edit_Post_NonExistingId_ReturnsHttpNotFound()
        {
            // Act
            var result = controller.Edit(99, new User { Id = 99 }) as HttpNotFoundResult;

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Delete_Get_ValidId_ReturnsDeleteView()
        {
            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.That(result.Model, Is.Not.Null);
            Assert.That(result.Model, Is.InstanceOf<User>());
        }

        [Test]
        public void Delete_Get_InvalidId_ReturnsHttpNotFound()
        {
            // Act
            var result = controller.Delete(99);

            // Assert
            Assert.That(result, Is.InstanceOf<HttpNotFoundResult>());
        }

        [Test]
        public void Delete_Post_ValidId_DeletesUser_RedirectsToIndex()
        {
            // Arrange
            int initialCount = UserController.userlist.Count;

            // Act
            var result = controller.Delete(1, new FormCollection()) as RedirectToRouteResult;

            // Assert
            Assert.That(UserController.userlist.Count, Is.EqualTo(initialCount - 1));
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        [Test]
        public void Delete_Post_NonExistingId_RedirectsToIndex()
        {
            // Act
            var result = controller.Delete(99, new FormCollection()) as RedirectToRouteResult;

            // Assert
            Assert.That(result.RouteValues["action"], Is.EqualTo("Index"));
        }

        
    }
}
