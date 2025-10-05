namespace Domain.Security.Services.Security
{
    using Domain.Security.Interfaces.Security;
    using Domain.Security.Interfaces.UnitOfWork;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Security.Entities;
    using Domain.Security.CustomEntities.Params;
    using Domain.Security.CustomEntities.Poco;

    /// <summary>Implementación de reglas de negocio para el servicio de Auditoria.</summary>
    public class AuditoriaService : IAuditoriaService
    {
        #region Variables

        /// <summary>Instancia de la unidad de trabajo - UnitOfWork.</summary>
        private readonly IUnitOfWork _iUnitOfWork;

        /// <summary>Instancia del Mapeador - Imapper.</summary>
        private readonly IMapper _iMapper;

        #endregion

        #region Constructor

        ///<summary>Inicializa una nueva instancia de la clase AuditoriaService.</summary>
        /// <param name="iUnitOfWork">Inyección de dependencias de la unidad de trabajo - UnitOfWork.</param>
        public AuditoriaService(IUnitOfWork iUnitOfWork, IMapper iMapper)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
        }

        #endregion

        #region Métodos

        /// <inheritdoc />
        public async Task<IEnumerable<Auditoria>> GetAllAsync()
        {
            return await _iUnitOfWork.Repository<Auditoria>().ConsultarTodosAsync();
        }

        /// <inheritdoc />
        public async Task<Auditoria> CreateAsync(ParamsAuditoria paramsCreate)
        {
            Auditoria newAuditoria = _iMapper.Map<Auditoria>(paramsCreate);
            await _iUnitOfWork.Repository<Auditoria>().AdicionarAsync(newAuditoria);
            await _iUnitOfWork.SaveChangesAsync();
            return newAuditoria;
        }

        #endregion

        #region Validaciones

        #endregion
    }
}
