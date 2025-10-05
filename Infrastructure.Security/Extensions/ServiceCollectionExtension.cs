namespace Infrastructure.Security.Extensions
{
    using Domain.Security.Interfaces.UnitOfWork;
    using Domain.Security.Services.UnitOfWork;
    using Infrastructure.Security.Persistence.Context;
    using Infrastructure.Security.Repository;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using Utilitarios.Contracts;
    using Utilitarios.Data;

    /// <summary>Clase estática que contiene métodos de extensión para configurar los servicios y dependencias del contenedor de inversión de control (IoC).
    /// Esta clase facilita la configuración del contexto de base de datos, la inyección de dependencias (DI) y el registro automático de servicios según la convención de nombres utilizada en la capa de dominio
    /// </summary>
    public static class ServiceCollectionExtension
    {
        #region Métodos

        /// <summary>Configura y registra el contexto de base de datos principal dentro del contenedor de servicios.</summary>
        /// <param name="services">Colección de servicios a la que se agregará el contexto.</param>
        /// <param name="configuration">Proveedor de configuración que permite acceder a las cadenas de conexión definidas en el archivo <c>appsettings.json</c>.</param>
        public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración del contexto de base de datos usando SQL Server.
            services.AddDbContext<SecurityContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("FlyHub")));
        }

        /// <summary>Registra todas las dependencias necesarias para la aplicación, incluyendo los repositorios genéricos, las unidades de trabajo (UoW) y todos los servicios que siguen la convención de nombres "<c>*Service</c>".</summary>
        /// <param name="services">Colección de servicios utilizada por el contenedor de inyección de dependencias.</param>
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            // Registro del repositorio genérico.
            services.AddScoped(typeof(ICrudSqlRepositorio<>), typeof(CrudSqlRepositorio<>));

            // Registro de las unidades de trabajo (Unit of Work).
            services.AddTransient<IServiceUnitOfWork, ServiceUnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Registro automático de servicios por convención.
            RegisterServicesByConvention(services);

            // Servicio hospedado para manejar eventos del ciclo de vida de la aplicación.
            services.AddHostedService<ApplicationLifetimeEventsHostedService>();
        }

        /// <summary>Registra de forma automática todos los servicios de la capa de dominio siguiendo una convención de nombres.</summary>
        /// <param name="services">Contenedor de servicios donde se registrarán las dependencias.</param>
        private static void RegisterServicesByConvention(IServiceCollection services)
        {
            // Obtiene el ensamblado donde está definida la interfaz IServiceUnitOfWork
            Assembly assemblyApplication = typeof(IServiceUnitOfWork).Assembly;

            // Obtiene todas las clases que cumplan con la convención de servicios.
            Type[] serviceTypeArray = assemblyApplication.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.Name.EndsWith("Service"))
                .ToArray();

            // Registra cada servicio con su interfaz correspondiente.
            foreach (Type implementationType in serviceTypeArray)
            {
                // Busca la interfaz correspondiente siguiendo la convención "I{NombreClase}".
                Type? interfaceType = implementationType.GetInterface($"I{implementationType.Name}");

                // Si la interfaz existe, se registra en el contenedor con un ciclo de vida transitorio.
                if (interfaceType != null)
                {
                    services.AddTransient(interfaceType, implementationType);
                }
            }
        }

        #endregion
    }
}
