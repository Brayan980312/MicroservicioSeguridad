namespace Domain.Security.CustomEntities.Params
{
    /// <summary>Representa la solicitud para refrescar el token JWT.</summary>
    public class ParamsRefreshToken
    {
        /// <summary>Token de actualización (refresh token) actual.</summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
