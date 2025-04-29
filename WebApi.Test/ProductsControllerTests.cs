using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApi.Controllers;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Test
{
    [TestClass]
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [TestMethod]
        public void GetProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var testProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Test Product 1", Priced = 10.99m },
                new Product { Id = 2, Name = "Test Product 2", Priced = 20.99m }
            };
            _mockService.Setup(service => service.GetAllProducts()).Returns(testProducts);

            // Act
            var result = _controller.GetProducts();

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var returnedProducts = ((OkObjectResult)result.Result).Value as IEnumerable<Product>;
            Assert.AreEqual(2, returnedProducts.Count());
        }

        [TestMethod]
        public void GetProductById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var testProduct = new Product { Id = 1, Name = "Test Product", Priced = 10.99m };
            _mockService.Setup(service => service.GetProductById(1)).Returns(testProduct);

            // Act
            var result = _controller.GetProductById(1);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var returnedProduct = ((OkObjectResult)result.Result).Value as Product;
            Assert.AreEqual(1, returnedProduct.Id);
            Assert.AreEqual("Test Product", returnedProduct.Name);
        }

        [TestMethod]
        public void GetProductById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockService.Setup(service => service.GetProductById(999)).Returns((Product)null);

            // Act
            var result = _controller.GetProductById(999);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void CreateProduct_ValidProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var productToCreate = new Product { Name = "New Product", Priced = 15.99m };
            var createdProduct = new Product { Id = 1, Name = "New Product", Priced = 15.99m };

            _mockService.Setup(service => service.CreateProduct(It.IsAny<Product>()))
                .Returns(createdProduct);

            // Act
            var result = _controller.CreateProduct(productToCreate);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            Assert.AreEqual(nameof(ProductsController.CreateProduct), ((CreatedAtActionResult)result.Result).ActionName);
            var returnValue = ((CreatedAtActionResult)result.Result).Value as Product;
            Assert.AreEqual(1, returnValue.Id);
        }

        [TestMethod]
        public void UpdateProduct_ValidUpdate_ReturnsOkResult()
        {
            // Arrange
            var productToUpdate = new Product { Id = 1, Name = "Updated Product", Priced = 25.99m };
            var updatedProduct = new Product { Id = 1, Name = "Updated Product", Priced = 25.99m };

            _mockService.Setup(service => service.UpdateProduct(It.IsAny<Product>()))
                .Returns(updatedProduct);

            // Act
            var result = _controller.UpdateProduct(productToUpdate);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var name = ((WebApi.Models.Product)((Microsoft.AspNetCore.Mvc.ObjectResult)((ActionResult<Product>)result.Result).Result).Value).Name;
            var price = ((WebApi.Models.Product)((Microsoft.AspNetCore.Mvc.ObjectResult)((ActionResult<Product>)result.Result).Result).Value).Priced;
            Assert.AreEqual("Updated Product", name);
            Assert.AreEqual(25.99m, price);
        }

        [TestMethod]
        public void DeleteProduct_ValidId_ReturnsNoContent()
        {
            // Arrange
            var productId = 1;
            _mockService.Setup(service => service.DeleteProduct(productId)).Returns(true);

            // Act
            var result = _controller.DeleteProduct(productId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteProduct_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var productId = 999;
            _mockService.Setup(service => service.DeleteProduct(productId)).Returns(false);

            // Act
            var result = _controller.DeleteProduct(productId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}