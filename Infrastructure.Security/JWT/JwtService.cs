namespace Infrastructure.Security.JWT
{
    using Domain.Security.Entities;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Domain.Security.Interfaces.JWT;
    using Domain.Security.Interfaces.UnitOfWork;
    using System.Linq.Expressions;
    using Utilitarios.Extensions;

    /// <summary>
    /// Implementación del servicio para la generación y validación de tokens JWT y Refresh Tokens.
    /// </summary>
    public class JwtService : IJwtService
    {
        #region Propiedades privadas

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly string _secretKey = "NPBA10338052213015585139PESM52542135";
        private readonly string _issuer = "ProyectoFlyHub.Api";
        private readonly string _audience = "ProyectoFlyHub.Clientes";
        private readonly int _jwtExpirationMinutes = 10;
        private readonly int _refreshTokenExpirationDays = 7;

        #endregion

        #region Constructor

        /// <summary>
        /// Inicializa una nueva instancia del servicio <see cref="JwtService"/>.
        /// </summary>
        /// <param name="context">Contexto de base de datos de seguridad.</param>
        public JwtService(IUnitOfWork iUnitOfWork)
        {
            _iUnitOfWork = iUnitOfWork;
        }

        #endregion

        #region Métodos públicos

        /// <inheritdoc/>
        public string GenerarJwtToken(int usuarioId, string usuarioNombre, string[] roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var rolSeleccionado = roles?.FirstOrDefault() ?? "Desconocido";

            string rolNombre = rolSeleccionado switch
            {
                "1" => "Administrador",
                "2" => "Cliente",
                _ => "Desconocido"
            };

            // Crear claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioNombre),
                new Claim("UsuarioId", usuarioId.ToString()),
                new Claim(ClaimTypes.Role, rolNombre),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Generar el token
            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <inheritdoc/>
        public async Task<RefreshTokens> GenerarRefreshTokenAsync(int usuarioId)
        {
            var refreshToken = new RefreshTokens
            {
                UsuarioId = usuarioId,
                Token = Guid.NewGuid().ToString("N"),
                ExpiraEn = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays),
                Revocado = false
            };

            await _iUnitOfWork.Repository<RefreshTokens>().AdicionarAsync(refreshToken);
            await _iUnitOfWork.SaveChangesAsync();

            return refreshToken;
        }

        /// <inheritdoc/>
        public async Task<RefreshTokens?> ValidarRefreshTokenAsync(string token)
        {
            RefreshTokens tokens = new RefreshTokens();
            tokens.Token = token;
            tokens.Revocado = false;
            Expression<Func<RefreshTokens, bool>> filtro = tokens.ToFilterExpression<RefreshTokens>();
            RefreshTokens consultaTokens = await _iUnitOfWork.Repository<RefreshTokens>().ConsultarUnoAsync(filtro);

            if (consultaTokens == null || consultaTokens.ExpiraEn <= DateTime.UtcNow)
                return null;

            return consultaTokens;
        }

        /// <inheritdoc/>
        public async Task RevocarRefreshTokenAsync(string token)
        {
            RefreshTokens tokens = new RefreshTokens();
            tokens.Token = token;
            Expression<Func<RefreshTokens, bool>> filtro = tokens.ToFilterExpression<RefreshTokens>();
            RefreshTokens consultaTokens = await _iUnitOfWork.Repository<RefreshTokens>().ConsultarUnoAsync(filtro);

            if (consultaTokens != null)
            {
                consultaTokens.Revocado = true;
                await _iUnitOfWork.Repository<RefreshTokens>().ActualizarAsync(consultaTokens);
                await _iUnitOfWork.SaveChangesAsync();
            }
        }

        #endregion
    }
}
