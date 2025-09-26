using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;
using SmartLocation.Api.Infrastructure;

namespace SmartLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnderecoPatiosController : ControllerBase
    {
        private readonly Contexto _context;

        public EnderecoPatiosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/EnderecoPatios (com paginação + HATEOAS)
        [HttpGet]
        public async Task<ActionResult<PagedResult<EnderecoPatio>>> GetEnderecosPatio(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var result = await _context.EnderecosPatio
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .ToPagedResultAsync(page, pageSize, baseUrl);

            return Ok(result);
        }

        // GET: api/EnderecoPatios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnderecoPatio>> GetEnderecoPatio(int id)
        {
            var enderecoPatio = await _context.EnderecosPatio
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enderecoPatio == null)
            {
                return NotFound();
            }

            return Ok(enderecoPatio);
        }

        // POST: api/EnderecoPatios
        [HttpPost]
        public async Task<ActionResult<EnderecoPatio>> PostEnderecoPatio(EnderecoPatio enderecoPatio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EnderecosPatio.Add(enderecoPatio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnderecoPatio), new { id = enderecoPatio.Id }, enderecoPatio);
        }

        // PUT: api/EnderecoPatios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnderecoPatio(int id, EnderecoPatio enderecoPatio)
        {
            if (id != enderecoPatio.Id)
            {
                return BadRequest("ID da URL diferente do objeto enviado.");
            }

            _context.Entry(enderecoPatio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnderecoPatioExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/EnderecoPatios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnderecoPatio(int id)
        {
            var enderecoPatio = await _context.EnderecosPatio.FindAsync(id);
            if (enderecoPatio == null)
            {
                return NotFound();
            }

            _context.EnderecosPatio.Remove(enderecoPatio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EnderecoPatioExists(int id)
        {
            return _context.EnderecosPatio.Any(e => e.Id == id);
        }
    }
}


