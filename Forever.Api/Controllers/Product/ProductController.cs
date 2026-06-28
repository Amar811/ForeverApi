using Forever.Api.Configuration;
using Forever.Api.DTOs.Product;
using Forever.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forever.Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Policy = AuthorizationPolicies.AdminOnly )]
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductRequestDto request)
        {
            var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
            var response = await _productService.AddProduct(request, userId);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProdutWithCategory()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
    }
}
