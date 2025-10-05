namespace Domain.Security.Services.UnitOfWork
{
    using Domain.Security.Interfaces.UnitOfWork;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    /// <summary>
    /// Representa una unidad de trabajo de servicios que centraliza la obtención 
    /// de dependencias registradas en el contenedor de inyección de dependencias (DI).
    /// </summary>
    /// <remarks>
    /// Esta clase implementa el patrón <b>Service Locator</b> sobre el contenedor de DI de .NET, 
    /// permitiendo resolver servicios de manera genérica y desacoplada.
    /// 
    /// Su propósito principal es proporcionar un punto único de acceso a los distintos servicios 
    /// de la capa de dominio, de forma similar al patrón <b>Unit of Work</b>, 
    /// pero orientado a servicios en lugar de repositorios.
    /// </remarks>
    public class ServiceUnitOfWork : IServiceUnitOfWork
    {
        private readonly IServiceProvider _provider;

        /// <summary>Inicializa una nueva instancia de la clase <see cref="ServiceUnitOfWork"/>.</summary>
        /// <param name="provider">
        /// Instancia del contenedor de servicios (<see cref="IServiceProvider"/>)que se utilizará para resolver las dependencias solicitadas.
        /// </param>
        public ServiceUnitOfWork(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>Obtiene una instancia del servicio especificado desde el contenedor de dependencias.</summary>
        /// <typeparam name="TService">Tipo del servicio a resolver.</typeparam>
        /// <returns>Una instancia del servicio solicitado, registrada previamente en el contenedor de DI.</returns>    
        public TService GetService<TService>() where TService : class
        {
            return _provider.GetRequiredService<TService>();
        }
    }
}
