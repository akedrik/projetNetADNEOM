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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Categorie categorie)
        {
            var result = await _categorieService.AddCategorie(categorie);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError,result);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put ([FromBody]Categorie categorie)
        {
            var result = await _categorieService.UpdateCategorie(categorie.Id, categorie.Libelle);
            if (!result)
                return BadRequest();

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete ([FromBody]Categorie categorie)
        {
            var result = await _categorieService.DeleteCategorie(categorie.Id);
            if (!result)
                return BadRequest();

            return Ok(result);
        }
    }
}