using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinnesLayer.Abstract;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DtoLayer.FeatureDto;
using SignalR.DtoLayer.ProductDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ProductList()
        {
            var value = _mapper.Map<List<ResultProductDto>>(_productService.TGetListAll());
            return Ok(value);
        }

        [HttpGet("ProductListWithCategory")]
        public IActionResult ProductListWithCategory()
        {
            var context = new SignalRContext();
            var values = context.Products.Include(x => x.Category).Select(y => new ResultProductWithCategory
            {
                ProductID = y.ProductID,
                ProductName = y.ProductName,
                Description = y.Description,
                Price = y.Price,
                ImageUrl = y.ImageUrl,
                ProductStatus = y.ProductStatus,
                CategoryName = y.Category.CategoryName
            });
            return Ok(values.ToList());
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductDto createProduct)
        {
            _productService.TAdd(new Product()
            {
                Description = createProduct.Description,
                ImageUrl = createProduct.ImageUrl,
                Price = createProduct.Price,
                ProductName = createProduct.ProductName,
                ProductStatus = createProduct.ProductStatus
                
            });
            return Ok("Ürünler oluşturuldu");
        }

        [HttpDelete]
        public IActionResult DeleteProduct (int id)
        {
            var values = _productService.TGetByID(id);
            _productService.TDelete(values);
            return Ok("Ürünler Silindi");
        }

        [HttpGet("GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var value = _productService.TGetByID(id);
            return Ok(value);
        }
        [HttpPut]

        public IActionResult UpdateProduct(UpdateProductDto updateProductDto)
        {
            _productService.TUpdate(new Product()
            {
                ProductID = updateProductDto.ProductID,
                Description = updateProductDto.Description,
                ImageUrl = updateProductDto.ImageUrl,
                Price = updateProductDto.Price,
                ProductName = updateProductDto.ProductName,
                ProductStatus = updateProductDto.ProductStatus,

            });
            return Ok("Ürünler Güncellendi");
        }
    }
}
