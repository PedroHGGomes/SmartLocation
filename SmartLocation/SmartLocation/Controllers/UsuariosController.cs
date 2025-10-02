using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;
using SmartLocation.Api.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpGet]
        [SwaggerOperation(Summary = "Listar usuários")]
        [SwaggerResponse(200, "Lista de usuários retornada com sucesso", typeof(PagedResult<Usuario>))]
        public async Task<ActionResult<PagedResult<Usuario>>> GetUsuarios([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var result = await _contexto.Usuario
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToPagedResultAsync(page, pageSize, baseUrl);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar usuário por ID")]
        [SwaggerResponse(200, "Usuário encontrado", typeof(Usuario))]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<ActionResult<Usuario>> GetUsuarioPorId(int id)
        {
            var usuario = await _contexto.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Buscar usuários por nome")]
        [SwaggerResponse(200, "Usuários encontrados", typeof(IEnumerable<Usuario>))]
        [SwaggerResponse(404, "Nenhum usuário encontrado")]
        public async Task<ActionResult<IEnumerable<Usuario>>> BuscarPorNome([FromQuery] string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return BadRequest("O parâmetro 'nome' é obrigatório.");
            var usuarios = await _contexto.Usuario
                .Where(u => u.Nome.Contains(nome))
                .AsNoTracking()
                .ToListAsync();

            if (!usuarios.Any()) return NotFound();
            return Ok(usuarios);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criar usuário")]
        [SwaggerResponse(201, "Usuário criado com sucesso", typeof(Usuario))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult<Usuario>> CriarUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _contexto.Usuario.Add(usuario);
            await _contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuarioPorId), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar usuário")]
        [SwaggerResponse(204, "Usuário atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID inconsistente")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest("ID da URL diferente do objeto enviado.");

            _contexto.Entry(usuario).State = EntityState.Modified;
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_contexto.Usuario.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir usuário")]
        [SwaggerResponse(204, "Usuário excluído com sucesso")]
        [SwaggerResponse(404, "Usuário não encontrado")]
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            var usuario = await _contexto.Usuario.FindAsync(id);
            if (usuario == null) return NotFound();

            _contexto.Usuario.Remove(usuario);
            await _contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}


