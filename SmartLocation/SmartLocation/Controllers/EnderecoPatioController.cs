using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;

namespace SmartLocation.Controllers
{
    [ApiController]
    public class EnderecoPatiosController : ControllerBase
    {
        private readonly Contexto _context;

        public EnderecoPatiosController(Contexto context)
        {
            _context = context;
        }

        // GET: api/EnderecoPatios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnderecoPatio>>> GetEnderecosPatio()
        {
            return await _context.EnderecosPatio.ToListAsync();
        }

        // GET: api/EnderecoPatios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnderecoPatio>> GetEnderecoPatio(int id)
        {
            var enderecoPatio = await _context.EnderecosPatio.FindAsync(id);

            if (enderecoPatio == null)
            {
                return NotFound();
            }

            return enderecoPatio;
        }

        // POST: api/EnderecoPatios
        [HttpPost]
        public async Task<ActionResult<EnderecoPatio>> PostEnderecoPatio(EnderecoPatio enderecoPatio)
        {
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
                return BadRequest();
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
                else
                {
                    throw;
                }
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

