using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Services;

namespace NetCoreApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ICategorieService _categorieService;

        public CategorieController(ICategorieService categorieService)
        {
            _categorieService = categorieService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _categorieService.GetAllCategories();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _categorieService.GetCategorieById(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}