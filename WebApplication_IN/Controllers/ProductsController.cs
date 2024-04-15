using Microsoft.AspNetCore.Mvc;
using WebApplication_IN.Enums;
using WebApplication_IN.Models;
using WebApplication_IN.Service;

namespace WebApplication_IN.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(ProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get product collection (category filter with enum) 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("api/products1")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductItemsAsync([FromQuery] ProductCollectionModel filter)
        {
            var filteredProducts = await _service.GetCollectionV1Async(filter);
            return Ok(filteredProducts);
        }

        /// <summary>
        /// Get product collection (category filter string) 
        /// </summary>
        /// <param name="page"></param>
        /// <example>1</example>
        /// <param name="pageSize"></param>
        /// <param name="categoryName"> Category name <p><i>Options:</i> <ul><li>1 = Food</li><li>2 = Drinks</li><li>3 = Clothes</li><li>4 = Sport</li></ul></p> </param>
        /// <returns></returns>
        [HttpGet("api/products2")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductItemsAsync(string categoryName, int page = 1, int pageSize = 10)
        {
            var filteredProducts = await _service.GetCollectionV2Asyncstring(categoryName, page, pageSize);
            return Ok(filteredProducts);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            var product  = await _service.GetProductByIdAsync(id);
            if (product.Value == null)
            {
                return NotFound();
            }
            return Ok(product.Value);
        }

        // PUT: api/Products/5
        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(Product product)
        {
            try
            {
                await _service.UpdateProductAsync(product);
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProductAsync(Product product)
        {
            try
            {
                var productDB = await _service.CreateProductAsync(product);

                CreatedAtAction("GetProduct", new { id = productDB.Value.ProductId }, productDB.Value);

                return Ok(productDB.Value);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            try
            {
                await _service.DeleteProductAsync(id);
                return NoContent();
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
