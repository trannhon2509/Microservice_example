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
    public class CategoriesController : ODataController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Endpoint lấy dữ liệu thực thể
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Categories);
        }

        // Endpoint đếm số lượng thực thể
        [EnableQuery]
        public IActionResult GetCount()
        {
            var count = _context.Categories.Count();
            return Ok(count);
        }

        // Endpoint lấy dữ liệu DTO
        [EnableQuery]
        public IActionResult GetCategoryDtos()
        {
            var categories = _context.Categories;
            var categoryDtos = _mapper.ProjectTo<CategoryDTO>(categories);
            return Ok(categoryDtos);
        }

        [EnableQuery]
        public IActionResult GetCategoryDtos(int key)
        {
            var category = _context.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            return Ok(categoryDto);
        }

        public IActionResult Post([FromBody] CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Created(category);
        }

        public IActionResult Put(int key, [FromBody] CategoryDTO categoryDto)
        {
            if (key != categoryDto.Id)
            {
                return BadRequest();
            }
            var category = _mapper.Map<Category>(categoryDto);
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return Updated(category);
        }

        public IActionResult Delete(int key)
        {
            var category = _context.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
