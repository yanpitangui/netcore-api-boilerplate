using System;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Hero;
using Boilerplate.Application.Filters;
using Boilerplate.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HeroController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroController(IHeroService heroService)
        {
            _heroService = heroService;
        }


        /// <summary>
        /// Returns all heroes in the database
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedList<GetHeroDto>>> GetHeroes([FromQuery] GetHeroesFilter filter)
        {
            return Ok(await _heroService.GetAllHeroes(filter));
        }


        /// <summary>
        /// Get one hero by id from the database
        /// </summary>
        /// <param name="id">The hero's ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetHeroDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<GetHeroDto>> GetHeroById(Guid id)
        {
            var hero = await _heroService.GetHeroById(id);
            if (hero == null) return NotFound();
            return Ok(hero);
        }

        /// <summary>
        /// Insert one hero in the database
        /// </summary>
        /// <param name="dto">The hero information</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GetHeroDto>> Create([FromBody] CreateHeroDto dto)
        {
            var newHero = await _heroService.CreateHero(dto);
            return CreatedAtAction(nameof(GetHeroById), new { id = newHero.Id }, newHero);

        }

        /// <summary>
        /// Update a hero from the database
        /// </summary>
        /// <param name="id">The hero's ID</param>
        /// <param name="dto">The update object</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GetHeroDto>> Update(Guid id, [FromBody] UpdateHeroDto dto)
        {

            var updatedHero = await _heroService.UpdateHero(id, dto);

            if (updatedHero == null) return NotFound();

            return NoContent();
        }


        /// <summary>
        /// Delete a hero from the database
        /// </summary>
        /// <param name="id">The hero's ID</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deleted = await _heroService.DeleteHero(id);
            if (deleted) return NoContent();
            return NotFound();
        }
    }
}
