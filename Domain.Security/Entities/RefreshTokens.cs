namespace Domain.Security.Entities
{
    using Utilitarios.Entities;
    using System;

    /// <summary>
    /// Representa el token de actualización (refresh token) asociado a un usuario
    /// para mantener la sesión activa sin necesidad de volver a iniciar sesión.
    /// </summary>
    public class RefreshTokens : EntidadBase
    {
        /// <summary>Identificador único del refresh token.</summary>
        public long? RefreshTokenId { get; set; }

        /// <summary>Identificador del usuario asociado al refresh token.</summary>
        public int UsuarioId { get; set; }

        /// <summary>Token de actualización asignado al usuario.</summary>
        public string Token { get; set; }

        /// <summary>Fecha y hora en la que el token expira.</summary>
        public DateTime ExpiraEn { get; set; }

        /// <summary>Indica si el token fue revocado (usado o invalidado).</summary>
        public bool Revocado { get; set; }
    }
}
