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

        /// <summary>Endpoint para realizar la creación de un nuevo usuario.</summary>
        /// <param name="paramsCreacionUsuario">Parametros de entrada para la creación de un nuevo usuario.</param>
        /// <returns> Objeto de tipo UsuarioRegistroDto creado.</returns>
        [HttpPost]
        [Route("RegistrarUsuario")]
        public async Task<ActionResult<UsuarioRegistroDto>> RegistrarUsuario(ParamsCreacionUsuario paramsCreacionUsuario)
        {
            var usuarioService = _iServiceUnitOfWork.GetService<IUsuarioService>();

            Usuario usuarioCreado = await usuarioService.CreateAsync(paramsCreacionUsuario);
            return Ok(_iMapper.Map<UsuarioRegistroDto>(usuarioCreado));
        }

        /// <summary>Endpoint para realizar el logueo de un usuario.</summary>
        /// <param name="paramsIniciarSesion">Parametros de entrada para el login de un usuario.</param>
        /// <returns> Objeto con información del usuario y token JWT</returns>
        [HttpPost]
        [Route("LoginUsuario")]
        public async Task<ActionResult<InicioSesionDto>> LoginUsuario(ParamsIniciarSesion paramsIniciarSesion)
        {
            var usuarioService = _iServiceUnitOfWork.GetService<IUsuarioService>();

            IniciarSesionPoco usuarioLogueado = await usuarioService.LoginUsuario(paramsIniciarSesion);
            return Ok(_iMapper.Map<InicioSesionDto>(usuarioLogueado));
        }

        /// <summary>Endpoint para realizar la validación del usuario existente.</summary>
        /// <param name="searchUsuario">Paramretros de entrada para la validación de un usuario existente.</param>
        /// <returns> Objeto con la información del usuario existente.</returns>
        [HttpGet]
        [Route("ConsultaUsuario")]
        public async Task<ActionResult<UsuarioDto>> ConsultaUsuario([FromQuery] ParamsConsultaUsuario searchUsuario)
        {
            IUsuarioService Service = _iServiceUnitOfWork.GetService<IUsuarioService>();

            Usuario usuario = await Service.ConsultaUsuario(searchUsuario);
            return Ok(_iMapper.Map<UsuarioDto>(usuario));
        }
    }
}
