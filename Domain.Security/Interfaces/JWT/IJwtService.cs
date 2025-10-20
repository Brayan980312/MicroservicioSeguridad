namespace Domain.Security.Interfaces.JWT
{
    using Domain.Security.Entities;

    /// <summary>
    /// Define los métodos necesarios para la generación y validación de tokens JWT y Refresh Tokens.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT para un usuario específico.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario.</param>
        /// <param name="usuarioNombre">Nombre del usuario.</param>
        /// <param name="roles">Lista de roles asociados al usuario.</param>
        /// <returns>Token JWT como cadena.</returns>
        string GenerarJwtToken(int usuarioId, string usuarioNombre, string[] roles);

        /// <summary>
        /// Genera un nuevo Refresh Token y lo almacena en base de datos.
        /// </summary>
        /// <param name="usuarioId">Identificador del usuario al que pertenece el token.</param>
        /// <returns>Instancia del <see cref="RefreshTokens"/> generado.</returns>
        Task<RefreshTokens> GenerarRefreshTokenAsync(int usuarioId);

        /// <summary>
        /// Valida un Refresh Token existente.
        /// </summary>
        /// <param name="token">Token de actualización a validar.</param>
        /// <returns>Entidad <see cref="RefreshTokens"/> si es válido; de lo contrario, null.</returns>
        Task<RefreshTokens?> ValidarRefreshTokenAsync(string token);

        /// <summary>
        /// Revoca (invalida) un Refresh Token una vez ha sido usado o comprometido.
        /// </summary>
        /// <param name="token">Token a revocar.</param>
        Task RevocarRefreshTokenAsync(string token);
    }
}
