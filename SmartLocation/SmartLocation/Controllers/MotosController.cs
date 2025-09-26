using Microsoft.AspNetCore.Mvc;
using SmartLocation.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly Contexto _contexto;

        public MotosController(Contexto contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Motos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
        {
            return Ok(await _contexto.Moto.ToListAsync());
        }

        // GET: api/Motos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetMotoPorId(int id)
        {
            var moto = await _contexto.Moto.FindAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            return Ok(moto);
        }

        // GET: api/Motos/search?modelo=CG
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Moto>>> BuscarPorModelo([FromQuery] string modelo)
        {
            var motos = await _contexto.Moto
                .Where(m => m.Modelo.Contains(modelo))
                .ToListAsync();

            if (motos == null || motos.Count == 0)
            {
                return NotFound();
            }

            return Ok(motos);
        }

        // POST: api/Motos
        [HttpPost]
        public async Task<ActionResult<Moto>> CriarMoto(Moto moto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _contexto.Moto.Add(moto);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMotoPorId), new { id = moto.Id }, moto);
        }

        // PUT: api/Motos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarMoto(int id, Moto moto)
        {
            if (id != moto.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(moto).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_contexto.Moto.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Motos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirMoto(int id)
        {
            var moto = await _contexto.Moto.FindAsync(id);

            if (moto == null)
            {
                return NotFound();
            }

            _contexto.Moto.Remove(moto);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}

