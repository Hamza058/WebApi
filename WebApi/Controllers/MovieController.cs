using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly Context _dbcontext;
        //Dependicy Injection
        public MovieController(Context dbContext) => _dbcontext = dbContext;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> Get()
        {
            if (_dbcontext.Movies == null)
            {
                return NotFound();
            }
            return await _dbcontext.Movies.Where(x => !x.IsDeleted).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Get(int id)
        {
            var get = _dbcontext.Movies.Find(id);
            if (_dbcontext.Movies == null || get == null)
                return NotFound();

            return await _dbcontext.Movies.FindAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> Post(Movie movie)
        {
            _dbcontext.Movies.Add(movie);
            await _dbcontext.SaveChangesAsync();
            return Ok("Başarıyla Eklendi");
        }

        [HttpPut]
        public async Task<ActionResult<Movie>> Put(Movie movie)
        {
            if (_dbcontext.Movies.FirstOrDefault(x => x.Id == movie.Id) == null)
                return BadRequest();

            _dbcontext.Entry(movie).State = EntityState.Modified;
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExxits(movie.Id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        private bool MovieExxits(int id)
        {
            return (_dbcontext.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (_dbcontext.Movies == null)
                return NotFound();

            var findMovie = await _dbcontext.Movies.FindAsync(id);
            if (findMovie == null)
                return NotFound();

            _dbcontext.Movies.Remove(findMovie);
            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }
    }
}
