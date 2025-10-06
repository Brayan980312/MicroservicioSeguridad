namespace Domain.Security.Interfaces.Security
{
    using Domain.Security.CustomEntities.Params;
    using Domain.Security.CustomEntities.Poco;
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
    }
}
