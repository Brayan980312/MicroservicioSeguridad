namespace Api.Security.Controllers
{ 
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>Endpoint para tener el MS activo.</summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public async Task<ActionResult> Get()
        {
            return Ok();
        }
    }
}