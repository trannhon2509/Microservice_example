using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Service.ProductAPI.Data;
using Service.ProductAPI.Models;
using Microsoft.AspNetCore.OData.Query;
using Service.ProductAPI.Dtos;

namespace Service.ProductAPI.Controllers
{
    public class ProductsController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Endpoint lấy dữ liệu thực thể
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Products);
        }

        // Endpoint đếm số lượng thực thể
        [EnableQuery]
        public IActionResult GetCount()
        {
            var count = _context.Products.Count();
            return Ok(count);
        }

        // Endpoint lấy dữ liệu DTO
        [EnableQuery]
        public IActionResult GetProductDtos()
        {
            var products = _context.Products;
            var productDtos = _mapper.ProjectTo<ProductDTO>(products);
            return Ok(productDtos);
        }

        [EnableQuery]
        public IActionResult GetProductDtos(int key)
        {
            var product = _context.Products.Find(key);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDTO>(product);
            return Ok(productDto);
        }

        public IActionResult Post([FromBody] ProductDTO productDto)
        {
            // Kiểm tra xem Category có tồn tại không
            var category = _context.Categories.Find(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Category does not exist.");
            }

            // Chuyển đổi DTO thành thực thể
            var product = _mapper.Map<Product>(productDto);
            product.Category = category; // Gán category đã tồn tại cho product

            // Thêm sản phẩm và lưu thay đổi
            _context.Products.Add(product);
            _context.SaveChanges();
            return Created(product);
        }

        public IActionResult Put(int key, [FromBody] ProductDTO productDto)
        {
            if (key != productDto.Id)
            {
                return BadRequest();
            }
            var product = _mapper.Map<Product>(productDto);
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
            return Updated(product);
        }

        public IActionResult Delete(int key)
        {
            var product = _context.Products.Find(key);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
