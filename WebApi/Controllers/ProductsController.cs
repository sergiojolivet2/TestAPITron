using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class ProductsController : ControllerBase
    {
        public readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [AllowAnonymous]
        [HttpGet("GetProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_productService.GetAllProducts());
        }
        [AllowAnonymous]
        [HttpGet("GetById{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }   
        [AllowAnonymous]
        [HttpPost("CreateProduct")]
        public ActionResult<Product> CreateProduct(Product product) 
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            var createProduct = _productService.CreateProduct(product);
            return CreatedAtAction(nameof(CreateProduct), new { id = createProduct.Id }, createProduct);
        
        }
        [AllowAnonymous]
        [HttpPut("UpdateProduct")]
        public ActionResult<Product> UpdateProduct(Product product) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedProduct = _productService.UpdateProduct(product);
            return Ok(updatedProduct);
        }
        [AllowAnonymous]
        [HttpDelete]
        public ActionResult DeleteProduct(int id) 
        { 
            var result = _productService.DeleteProduct(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
