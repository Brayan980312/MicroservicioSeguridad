namespace Domain.Security.Services.Security
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Transactions;
    using AutoMapper;
    using Domain.Security.CustomEntities.Poco;
    using Domain.Security.Entities;
    using Domain.Security.Interfaces.Security;
    using Domain.Security.Interfaces.UnitOfWork;
    using Domain.Security.CustomEntities.Params;
    using Utilitarios.Extensions;
    using Utilitarios.Security;
    using Utilitarios.Helpers;
    using Domain.Security.Enums;
    using Utilitarios;
    using Utilitarios.Constants;
    using Domain.Security.Interfaces.JWT;
    using Domain.Security.DTOs;

    /// <summary> Implementación reglas de negocio para el servicio de Usuario.</summary>
    public class UsuarioService : IUsuarioService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Inyeccion de convertidor o resolutor de modelos.</summary>
        private readonly IMapper _iMapper;

        private readonly IJwtService _jwtService;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase UsuarioService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        /// <param name="iMapper">Inyección de convertidor o resolutor de modelos.</param>
        public UsuarioService(IUnitOfWork iUnitOfWork, IMapper iMapper, IJwtService jwtService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _jwtService = jwtService;
        }

        #endregion

        #region Metodos

        /// <summary>Realiza la creación de un nuevo usuario del sistema (Solo estudiante).</summary>
        /// <param name="paramsRegistrarUsuario">Parámetros que contienen la información del usuario a registrar.</param>
        /// <returns>Un objeto de tipo Usuario que representa el usuario creado.</returns>
        public async Task<Usuario> CreateAsync(ParamsCreacionUsuario paramsRegistrarUsuario, int? userId = null)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5), TransactionScopeAsyncFlowOption.Enabled))
            {
                await ValidarCamposCreacionUsuario(paramsRegistrarUsuario);
                Usuario usuarioNuevo = new Usuario
                {
                    UsuarioIdentificacion = EncryptionHelper.Encrypt(paramsRegistrarUsuario.UsuarioIdentificacion),
                    UsuarioNombreCompleto = paramsRegistrarUsuario.UsuarioNombreCompleto,
                    UsuarioCorreo = EncryptionHelper.Encrypt(paramsRegistrarUsuario.UsuarioCorreo),
                    UsuarioTelefono = EncryptionHelper.Encrypt(paramsRegistrarUsuario.UsuarioTelefono),
                    UsuarioClave = SecurityHelper.HashPassword(paramsRegistrarUsuario.UsuarioClave),
                };

                await _iUnitOfWork.Repository<Usuario>().AdicionarAsync(usuarioNuevo);
                await _iUnitOfWork.SaveChangesAsync();

                /* Asocia el usuario creado al rol de estudiante */
                UsuarioRol usuarioRolRegistrar = new UsuarioRol
                {
                    UsuarioId = (int)usuarioNuevo.UsuarioId,
                    RolId = (int)Enum.Roles.Cliente
                };

                await _iUnitOfWork.Repository<UsuarioRol>().AdicionarAsync(usuarioRolRegistrar);
                await _iUnitOfWork.SaveChangesAsync();

                scope.Complete();
                return usuarioNuevo;
            }
        }

        /// <summary>Realiza la validación del usuario que se está tratando de loguear.</summary>
        /// <param name="paramsIniciarSesion">Parámetros que contienen la información del usuario a loguear.</param>
        /// <returns>Objeto con el token JWT y la información del usuario.</returns>
        public async Task<IniciarSesionPoco> LoginUsuario(ParamsIniciarSesion paramsIniciarSesion)
        {
            IniciarSesionPoco inicioSesion = new IniciarSesionPoco();
            await ValidarCamposLoginUsuario(paramsIniciarSesion);

            /* Obtiene la información del usuario logueado */
            Usuario validarFiltro = new Usuario
            {
                UsuarioIdentificacion = EncryptionHelper.Encrypt(paramsIniciarSesion.UsuarioIdentificacion),
            };
            Expression<Func<Usuario, bool>> filtro = validarFiltro.ToFilterExpression<Usuario>();
            IEnumerable<Usuario> usuarioLogin = await _iUnitOfWork.Repository<Usuario>().ConsultarListaAsync(filtro);

            if (usuarioLogin.Count() > 0)
            {
                /* Asociar la información del usuario logueado */
                List<UsuarioRol> rolesUsuario = await ConsultarRolesUsuario(usuarioLogin.First().UsuarioId ?? 0);
                string tokenJWT = _jwtService.GenerarJwtToken(
                    usuarioId: usuarioLogin.First().UsuarioId ?? 0,
                    usuarioNombre: usuarioLogin.First().UsuarioIdentificacion,
                    roles: rolesUsuario.Select(r => r.RolId.ToString()).ToArray()
                );

                RefreshTokens refreshToken = await _jwtService.GenerarRefreshTokenAsync(usuarioLogin.First().UsuarioId ?? 0);

                inicioSesion.UsuarioId = usuarioLogin.First().UsuarioId ?? 0;
                inicioSesion.UsuarioIdentificacion = EncryptionHelper.Decrypt(usuarioLogin.First().UsuarioIdentificacion ?? "");
                inicioSesion.UsuarioNombreCompleto = usuarioLogin.First().UsuarioNombreCompleto ?? "";
                inicioSesion.UsuarioTelefono = EncryptionHelper.Decrypt(usuarioLogin.First().UsuarioTelefono ?? "");
                inicioSesion.UsuarioCorreo = EncryptionHelper.Decrypt(usuarioLogin.First().UsuarioCorreo ?? "");
                inicioSesion.Roles = rolesUsuario;
                inicioSesion.AccessToken = tokenJWT;
                inicioSesion.RefreshToken = refreshToken.Token;
                inicioSesion.ExpiraEn = refreshToken.ExpiraEn;
            }

            return inicioSesion;
        }

        /// <summary>Realiza la consulta de los roles asociados al usuario.</summary>
        /// <param name="UsuarioId">Identificador del usuarioId que realiza el logueo en el sistema.</param>
        /// <returns>Lista de roles asociados al usuario.</returns>
        public async Task<List<UsuarioRol>> ConsultarRolesUsuario(int UsuarioId)
        {
            List<UsuarioRol> rolesUsuario = new List<UsuarioRol>();
            UsuarioRol consultaRoles = new UsuarioRol
            {
                UsuarioId = UsuarioId
            };
            Expression<Func<UsuarioRol, bool>> filtro = consultaRoles.ToFilterExpression<UsuarioRol>();
            IEnumerable<UsuarioRol> usuarioLogin = await _iUnitOfWork.Repository<UsuarioRol>().ConsultarListaAsync(filtro);


            return (List<UsuarioRol>)usuarioLogin;
        }

        /// <summary>Valida si el usuario existe en el sistema.</summary>
        /// <param name="searchUsuario">Parametros con el que se validará el usuario en el sistema.</param>
        /// <returns>Objeto del usuario consultado.</returns>
        public async Task<Usuario> ConsultaUsuario(ParamsConsultaUsuario searchUsuario)
        {
            Usuario usuario = new Usuario();
            Expression<Func<Usuario, bool>> filtro = searchUsuario.ToFilterExpression<Usuario>();
            Usuario consultaUsuario = await _iUnitOfWork.Repository<Usuario>().ConsultarUnoAsync(filtro);          

            return consultaUsuario == null ? usuario : consultaUsuario;
        }

        /// <summary>
        /// Genera un nuevo par de tokens (JWT y Refresh Token) a partir de un Refresh Token válido.
        /// </summary>
        /// <param name="refreshToken">Token de refresco previamente emitido al usuario durante el inicio de sesión.</param>
        /// <returns>
        /// Un objeto <see cref="InicioSesionDto"/> con los tokens actualizados y 
        /// la información básica del usuario autenticado.
        /// </returns>
        /// <exception cref="UnauthorizedAccessException">
        /// Se lanza si el Refresh Token no existe, expiró o fue invalidado.
        /// </exception>
        public async Task<IniciarSesionPoco> RefrescarTokenAsync(string refreshToken)
        {
            // Validar el token de refresco
            RefreshTokens tokenActual = await ValidarCamposRefrescarToken(refreshToken);

            // Obtener el usuario y sus roles
            Usuario usuario = await _iUnitOfWork.Repository<Usuario>().ConsultarPorIdAsync(tokenActual.UsuarioId);
            List<UsuarioRol> rolesUsuario = await ConsultarRolesUsuario(usuario.UsuarioId ?? 0);
            string[] rolesArray = rolesUsuario.Select(r => r.RolId.ToString()).ToArray();

            // Generar nuevo JWT y Refresh Token
            string nuevoTokenJWT = _jwtService.GenerarJwtToken(
                usuarioId: usuario.UsuarioId ?? 0,
                usuarioNombre: usuario.UsuarioIdentificacion ?? "",
                roles: rolesArray
            );

            RefreshTokens nuevoRefreshToken = new RefreshTokens
            {
                UsuarioId = usuario.UsuarioId ?? 0,
                Token = Guid.NewGuid().ToString("N"),
                ExpiraEn = DateTime.UtcNow.AddDays(7),
                Revocado = false
            };

            // Marcar el token anterior como revocado y guardar ambos cambios
            tokenActual.Revocado = true;

            await _iUnitOfWork.Repository<RefreshTokens>().ActualizarAsync(tokenActual);
            await _iUnitOfWork.Repository<RefreshTokens>().AdicionarAsync(nuevoRefreshToken);
            await _iUnitOfWork.SaveChangesAsync();

            // Armar respuesta
            IniciarSesionPoco resultado = new IniciarSesionPoco
            {
                AccessToken = nuevoTokenJWT,
                RefreshToken = nuevoRefreshToken.Token,
                ExpiraEn = nuevoRefreshToken.ExpiraEn,
                UsuarioId = usuario.UsuarioId ?? 0,
                UsuarioIdentificacion = EncryptionHelper.Decrypt(usuario.UsuarioIdentificacion ?? ""),
                UsuarioNombreCompleto = usuario.UsuarioNombreCompleto ?? "",
                UsuarioCorreo = EncryptionHelper.Decrypt(usuario.UsuarioCorreo ?? ""),
                UsuarioTelefono = EncryptionHelper.Decrypt(usuario.UsuarioTelefono ?? ""),
                Roles = rolesUsuario
            };

            return resultado;
        }
        #endregion

        #region ValidacionCampos
        /// <summary>Valida los campos obligatorios para realizar la creación de un usuario.</summary>
        /// <param name="paramsRegistrarUsuario">El objeto de tipo parametrosRegistrarUsuario que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposCreacionUsuario(ParamsCreacionUsuario paramsRegistrarUsuario)
        {
            string errores = string.Empty;

            if (string.IsNullOrEmpty(paramsRegistrarUsuario.UsuarioIdentificacion))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "identificación");
            }
            else
            {
                Usuario validarFiltro = new Usuario
                {
                    UsuarioIdentificacion = EncryptionHelper.Encrypt(paramsRegistrarUsuario.UsuarioIdentificacion),
                };
                Expression<Func<Usuario, bool>> filtro = validarFiltro.ToFilterExpression<Usuario>();
                IEnumerable<Usuario> usuariosCreados = await _iUnitOfWork.Repository<Usuario>().ConsultarListaAsync(filtro);

                if (usuariosCreados.Count() > 0)
                {
                    errores += DefaultMessages.UserAlreadyExistsById;
                }
            }

            if (string.IsNullOrEmpty(paramsRegistrarUsuario.UsuarioNombreCompleto))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "nombre completo");
            }

            if (string.IsNullOrEmpty(paramsRegistrarUsuario.UsuarioCorreo))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "correo");
            }
            else
            {
                if (!RegexHelper.EsCorreo(paramsRegistrarUsuario.UsuarioCorreo))
                {
                    errores += DefaultMessages.InvalidEmail;
                    
                }
                else 
                {
                    Usuario validarFiltro = new Usuario
                    {
                        UsuarioCorreo = EncryptionHelper.Encrypt(paramsRegistrarUsuario.UsuarioCorreo),
                    };
                    Expression<Func<Usuario, bool>> filtro = validarFiltro.ToFilterExpression<Usuario>();
                    IEnumerable<Usuario> usuariosCreados = await _iUnitOfWork.Repository<Usuario>().ConsultarListaAsync(filtro);

                    if (usuariosCreados.Count() > 0)
                    {
                        errores += DefaultMessages.UserAlreadyExistsByEmail;
                    }
                }
            }

            if (string.IsNullOrEmpty(paramsRegistrarUsuario.UsuarioTelefono))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "telefono");
            }

            if (string.IsNullOrEmpty(paramsRegistrarUsuario.UsuarioClave))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "clave");
            }
            else if (!RegexHelper.EsPasswordFuerte(paramsRegistrarUsuario.UsuarioClave))
            {
                errores += DefaultMessages.PasswordTooWeak;
            }

            if (string.IsNullOrEmpty(paramsRegistrarUsuario.UsuarioClaveConfirmar))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "confirmar clave");
            }

            if (paramsRegistrarUsuario.UsuarioClave != paramsRegistrarUsuario.UsuarioClaveConfirmar)
            {
                errores += DefaultMessages.PasswordsDoNotMatch;
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>Valida los campos obligatorios para realizar el logueo en el sistema.</summary>
        /// <param name="paramsIniciarSesion">El objeto de tipo ParamsIniciarSesion que contiene los detalles a validar.</param>
        /// <exception cref="ValidationException">Lanza una excepción si alguno de los campos obligatorios es nulo o tiene un valor inválido.</exception>
        private async Task ValidarCamposLoginUsuario(ParamsIniciarSesion paramsIniciarSesion)
        {
            string errores = string.Empty;

            if (string.IsNullOrEmpty(paramsIniciarSesion.UsuarioIdentificacion))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "usuario");
            }
            else
            {
                Usuario validarFiltro = new Usuario
                {
                    UsuarioIdentificacion = EncryptionHelper.Encrypt(paramsIniciarSesion.UsuarioIdentificacion),
                };
                Expression<Func<Usuario, bool>> filtro = validarFiltro.ToFilterExpression<Usuario>();
                IEnumerable<Usuario> usuariosCreados = await _iUnitOfWork.Repository<Usuario>().ConsultarListaAsync(filtro);

                if (usuariosCreados.Count() == 0)
                {
                    errores += DefaultMessages.UserNotFound;
                }
            }
            if (string.IsNullOrEmpty(paramsIniciarSesion.UsuarioClave) && string.IsNullOrEmpty(errores))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "clave");
            }
            else if (string.IsNullOrEmpty(errores))
            {
                Usuario validarFiltro = new Usuario
                {
                    UsuarioIdentificacion = EncryptionHelper.Encrypt(paramsIniciarSesion.UsuarioIdentificacion),
                };
                Expression<Func<Usuario, bool>> filtro = validarFiltro.ToFilterExpression<Usuario>();
                IEnumerable<Usuario> usuarioLogin = await _iUnitOfWork.Repository<Usuario>().ConsultarListaAsync(filtro);

                if (usuarioLogin.Count() > 0)
                {
                    bool valido = SecurityHelper.VerifyPassword(usuarioLogin.FirstOrDefault().UsuarioClave, paramsIniciarSesion.UsuarioClave);
                    if (!valido) {
                        errores += DefaultMessages.InvalidCredentials;
                    }                    
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }
        }

        /// <summary>
        /// Realiza las validaciones de negocio necesarias antes de refrescar un token JWT.
        /// </summary>
        /// <param name="refreshToken">Token de refresco enviado por el cliente.</param>
        /// <returns>Entidad RefreshToken válida si pasa las validaciones.</returns>
        /// <exception cref="ValidationException">Si el token no cumple alguna condición de negocio.</exception>
        private async Task<RefreshTokens> ValidarCamposRefrescarToken(string refreshToken)
        {
            string errores = string.Empty;

            if (string.IsNullOrEmpty(refreshToken))
            {
                errores += string.Format(DefaultMessages.FieldRequiredWithName, "refresh token");
                throw new ValidationException(errores);
            }

            // Buscar el token en la base de datos
            Expression<Func<RefreshTokens, bool>> filtro = t => t.Token == refreshToken;
            IEnumerable<RefreshTokens> tokens = await _iUnitOfWork.Repository<RefreshTokens>().ConsultarListaAsync(filtro);
            RefreshTokens tokenActual = tokens.FirstOrDefault();

            if (tokenActual == null)
            {
                errores += "El token de refresco no existe.";
            }
            else
            {
                if (tokenActual.Revocado)
                {
                    errores += "El token de refresco fue revocado."; 
                }

                if (tokenActual.ExpiraEn <= DateTime.UtcNow)
                {
                    errores += "El token de refresco ha expirado."; 
                }

                // Verificar que el usuario exista
                Usuario usuario = await _iUnitOfWork.Repository<Usuario>().ConsultarPorIdAsync(tokenActual.UsuarioId);
                if (usuario == null)
                {
                    errores += "No se encontró el usuario asociado al token."; 
                }
            }

            if (!string.IsNullOrEmpty(errores))
            {
                throw new ValidationException(errores);
            }

            return tokenActual;
        }
        #endregion
    }
}
