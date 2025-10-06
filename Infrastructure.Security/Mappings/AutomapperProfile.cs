namespace Infrastructure.Security.Mappings
{
    using AutoMapper;
    using Domain.Security.CustomEntities.Params;
    using Domain.Security.CustomEntities.Poco;
    using Domain.Security.DTOs;
    using Domain.Security.Entities;

    /// <summary>Configura los mapeos entre las entidades de dominio de seguridad y los objetos de transferencia de datos (DTOs/Params) mediante AutoMapper.
    /// Permite la conversión bidireccional entre los tipos configurados.
    /// </summary>
    public class AutomapperProfile : Profile
    {

        /// <summary>Inicializa una nueva instancia de <see cref="AutomapperProfile"/> y configura los mapeos entre entidades y DTOs/Params.</summary>
        public AutomapperProfile()
        {
            /// <summary>Mapea la entidad <see cref="Auditoria"/> a <see cref="ParamsAuditoria"/> y viceversa.</summary>
            CreateMap<Auditoria, ParamsAuditoria>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Auditoria"/> a <see cref="AuditoriaDto"/> y viceversa.</summary>
            CreateMap<Auditoria, AuditoriaDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="UsuarioRol"/> a <see cref="UsuarioRolDto"/> y viceversa.</summary>
            CreateMap<UsuarioRol, UsuarioRolDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="Usuario"/> a <see cref="UsuarioRegistroDto"/> y viceversa.</summary>
            CreateMap<Usuario, UsuarioRegistroDto>().ReverseMap();
            /// <summary>Mapea la entidad <see cref="IniciarSesionPoco"/> a <see cref="InicioSesionDto"/> y viceversa.</summary>
            CreateMap<IniciarSesionPoco, InicioSesionDto>().ReverseMap();

        }
    }
}
