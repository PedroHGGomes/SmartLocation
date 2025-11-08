using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartLocation.ML;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartLocation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ml")]
    public class MlController : ControllerBase
    {
        private readonly ManutencaoMlService _svc;
        public MlController(ManutencaoMlService svc) => _svc = svc;

        [HttpPost("predict")]
        [SwaggerOperation(Summary = "Prediz probabilidade de manutenção nos próximos 30 dias")]
        public IActionResult Predict([FromBody] ManutencaoRequest req)
        {
            var pred = _svc.Prever(req.Km, req.Falhas, req.IdadeMeses);
            return Ok(new
            {
                req.Km,
                req.Falhas,
                req.IdadeMeses,
                pred.Predito,
                pred.Probability
            });
        }
    }
}