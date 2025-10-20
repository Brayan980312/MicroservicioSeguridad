namespace Domain.Security.Interfaces.Security
{
    using Domain.Security.CustomEntities.Params;
    using Domain.Security.CustomEntities.Poco;
    using Domain.Security.DTOs;
    using Domain.Security.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio para registro y login de usuario.
    /// <para>Este servicio permite operaciones de creación utilizando los contratos genéricos <see cref="ICreateService{TParam, TEntity}"/>.</para>
    /// </summary>
    public interface IUsuarioService : ICreateService<ParamsCreacionUsuario, Usuario>
    {

        /// <summary>Realiza la validación del usuario que se está tratando de loguear.</summary>
        /// <param name="paramsIniciarSesion">Parámetros que contienen la información del usuario a loguear.</param>
        /// <returns>Objeto con el token JWT y la información del usuario.</returns>
        Task<IniciarSesionPoco> LoginUsuario(ParamsIniciarSesion paramsIniciarSesion);

        /// <summary>Valida si el usuario existe en el sistema.</summary>
        /// <param name="searchUsuario">Parametros con el que se validará el usuario en el sistema.</param>
        /// <returns>Objeto del usuario consultado.</returns>
        Task<Usuario> ConsultaUsuario(ParamsConsultaUsuario searchUsuario);

        /// <summary>Genera un nuevo par de tokens (JWT y Refresh Token) a partir de un Refresh Token válido.</summary>
        /// <param name="refreshToken">Token de refresco previamente emitido al usuario durante el inicio de sesión.</param>
        /// <returns>Un nuevo objeto <see cref="InicioSesionDto"/> con los tokens actualizados y la información básica del usuario autenticado.</returns>
        Task<IniciarSesionPoco> RefrescarTokenAsync(string refreshToken);

    }
}
