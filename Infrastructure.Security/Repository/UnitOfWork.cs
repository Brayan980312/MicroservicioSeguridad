namespace Infrastructure.Security.Repository
{
    using Domain.Security.Interfaces.UnitOfWork;
    using Infrastructure.Security.Persistence.Context;
    using Microsoft.EntityFrameworkCore;
    using Utilitarios.Contracts;
    using Utilitarios.Data;
    using Utilitarios.Entities;
    /// <summary>Implementación del patrón Unit of Work.
    /// Encapsula el contexto de base de datos y coordina los repositorios (genéricos y especializados).
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SecurityContext _contexto;
        private readonly Dictionary<Type, object> _repositories = new();

        /// <summary>Inicializa una nueva instancia de la clase <see cref="UnitOfWork"/>.</summary>
        /// <param name="contexto">Contexto de base de datos de Entity Framework utilizado para persistir cambios.</param>
        public UnitOfWork(SecurityContext contexto)
        {
            _contexto = contexto;
        }

        /// <summary>Obtiene una instancia del repositorio genérico para la entidad especificada.
        /// Si no existe aún en el diccionario de repositorios, se crea una nueva instancia.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad que hereda de <see cref="EntidadBase"/>.</typeparam>
        /// <returns>Una instancia de <see cref="ICrudSqlRepositorio{T}"/> asociada a la entidad <typeparamref name="T"/>.</returns>
        public ICrudSqlRepositorio<T> Repository<T>() where T : EntidadBase
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new CrudSqlRepositorio<T>(_contexto);
                _repositories.Add(type, repoInstance);
            }

            return (ICrudSqlRepositorio<T>)_repositories[type];
        }

        /// <summary>Guarda de manera sincrónica todos los cambios efectuados en el contexto.</summary>
        public void SaveChanges() => _contexto.SaveChanges();

        /// <summary>Guarda de manera asincrónica todos los cambios efectuados en el contexto.</summary>
        /// <returns>Una tarea asincrónica que representa la operación de guardado.</returns>
        public async Task SaveChangesAsync() => await _contexto.SaveChangesAsync();

        /// <summary>Libera los recursos utilizados por el contexto de base de datos.</summary>
        public void Dispose()
        {
            if (_contexto != null)
                _contexto.Dispose();
        }

        /// <summary>Libera los recursos utilizados por el contexto de base de datos de manera asincrónica.</summary>
        /// <returns>Una tarea asincrónica que representa la operación de liberación de recursos.</returns>
        public async Task DisposeAsync()
        {
            if (_contexto != null)
                await _contexto.DisposeAsync();
        }
    }
}