using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreApp.Core.Entities;
using NetCoreApp.Core.Interfaces.Logging;
using NetCoreApp.Core.Interfaces.Services;

namespace NetCoreApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ICategorieService _categorieService;
        private readonly IAppLogger<CategorieController> _logger;

        public CategorieController(ICategorieService categorieService,
             IAppLogger<CategorieController> logger)
        {
            _categorieService = categorieService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Categorie>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Start Get API Categorie");
            var result = await _categorieService.GetAllCategories();
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Categorie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _categorieService.GetCategorieById(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody]Categorie categorie)
        {
            var result = await _categorieService.AddCategorie(categorie);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put (int id, [FromBody]Categorie categorie)
        {
            var result = await _categorieService.UpdateCategorie(id, categorie.Libelle);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete (int id)
        {
            var result = await _categorieService.DeleteCategorie(id);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}