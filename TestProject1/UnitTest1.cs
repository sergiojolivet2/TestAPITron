using NUnit.Framework;
using WebApi.Services;

namespace TestProject1
{
    [TestClass]
    public class Tests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}