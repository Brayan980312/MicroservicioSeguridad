namespace Api.Security.Controllers
{
    using AutoMapper;
    using Domain.Security.CustomEntities.Params;
    using Domain.Security.CustomEntities.Poco;
    using Domain.Security.DTOs;
    using Domain.Security.Entities;
    using Domain.Security.Interfaces.Security;
    using Domain.Security.Interfaces.UnitOfWork;
    using Microsoft.AspNetCore.Mvc;
    
    [Route("api/[controller]")]
    [ApiController]
    public class SeguridadController : ControllerBase
    {
        #region Variables
        /// <summary>Inyección de Dependecias de servicios.</summary>
        private readonly IServiceUnitOfWork _iServiceUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;
        #endregion Variables

        #region Constructor

        /// <summary>Inicializa una nueva instancia de la clase SeguridadController.</summary>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        /// <param name="iServiceUnitOfWork">Inyección de dependecias de Servicios.</param>
        public SeguridadController(IMapper iMapper, IServiceUnitOfWork iServiceUnitOfWork)
        {
            _iMapper = iMapper;
            _iServiceUnitOfWork = iServiceUnitOfWork;
        }

        #endregion

        /// <summary>Endpoint para la consulta de ConsultarAuditoria.</summary>
        /// <param name="parametrosConsulta">Filtro para consultar la entidad de Auditoria.</param>
        /// <returns>Lista de AuditoriaDto.</returns>
        [HttpGet]
        [Route("ConsultarAuditoria")]
        public async Task<ActionResult<List<AuditoriaDto>>> ConsultarAuditoria([FromQuery] ParamsAuditoria parametrosConsulta)
        {
            var auditoriaService = _iServiceUnitOfWork.GetService<IAuditoriaService>();

            IEnumerable<Auditoria> auditorias = await auditoriaService.GetAllAsync();
            return Ok(_iMapper.Map<List<AuditoriaDto>>(auditorias?.ToList()));
        }
    }
}
