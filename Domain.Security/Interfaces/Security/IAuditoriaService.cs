namespace Domain.Security.Interfaces.Security
{
    using Domain.Security.CustomEntities.Params;
    using Domain.Security.Entities;
    using Utilitarios.Contracts.Crud;

    /// <summary>Define el contrato del servicio de auditoría.
    /// <para>Este servicio permite operaciones de creación y lectura de auditorías, utilizando los contratos genéricos <see cref="ICreateService{TParam, TEntity}"/> y <see cref="IReadAllService{TEntity}"/>.</para>
    /// </summary>
    public interface IAuditoriaService : ICreateService<ParamsAuditoria,Auditoria>, IReadAllService<Auditoria>
    {
        /* Aquí se debe dejar el contrato personalizado */
    }
}
