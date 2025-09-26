using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;
using SmartLocation.Api.Infrastructure;

namespace SmartLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly Contexto _contexto;

        public UsuariosController(Contexto contexto)
        {
            _contexto = contexto;
        }

        // GET: api/Usuarios (com paginação + HATEOAS)
        [HttpGet]
        public async Task<ActionResult<PagedResult<Usuario>>> GetUsuarios(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var result = await _contexto.Usuario
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToPagedResultAsync(page, pageSize, baseUrl);

            return Ok(result);
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioPorId(int id)
        {
            var usuario = await _contexto.Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // GET: api/Usuarios/search?nome=Pedro
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Usuario>>> BuscarPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return BadRequest("O parâmetro 'nome' é obrigatório.");
            }

            var usuarios = await _contexto.Usuario
                .Where(u => u.Nome.Contains(nome))
                .AsNoTracking()
                .ToListAsync();

            if (!usuarios.Any())
            {
                return NotFound();
            }

            return Ok(usuarios);
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> CriarUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _contexto.Usuario.Add(usuario);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuarioPorId), new { id = usuario.Id }, usuario);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("ID da URL diferente do objeto enviado.");
            }

            _contexto.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_contexto.Usuario.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var usuario = await _contexto.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _contexto.Usuario.Remove(usuario);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}

