namespace Domain.Security.Interfaces.UnitOfWork
{
    /// <summary>Define el contrato para la Unidad de Trabajo de servicios (Service Unit of Work).</summary>
    /// <remarks>
    /// Esta interfaz actúa como un punto central de acceso para todos los servicios del dominio.
    /// Permite resolver dinámicamente cualquier servicio registrado en el contenedor de dependencias,
    /// manteniendo una arquitectura desacoplada y cumpliendo con los principios SOLID.
    /// </remarks>
    public interface IServiceUnitOfWork
    {
        /// <summary>Obtiene una instancia del servicio especificado desde el contenedor de dependencias.</summary>
        /// <typeparam name="TService">Tipo del servicio que se desea resolver.</typeparam>
        /// <returns>Instancia del servicio solicitado, si está registrado en el contenedor.</returns>
        TService GetService<TService>() where TService : class;
    }
}
