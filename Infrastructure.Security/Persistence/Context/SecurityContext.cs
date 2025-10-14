namespace Infrastructure.Security.Persistence.Context
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>DbContext para las entidades relacionadas con seguridad y auditoría.
    /// Contiene la configuración y DbSet de la tabla de auditoría y aplica los mapeos correspondientes.
    /// </summary>
    public class SecurityContext : DbContext
    {
        #region Constructores

        /// <summary>Constructor por defecto. Necesario para algunas herramientas (EF tools, scaffolding).</summary>
        public SecurityContext()
        {
        }

        /// <summary>Constructor que recibe las opciones del DbContext (inyección de dependencias).</summary>
        /// <param name="options">Opciones de configuración del contexto.</param>
        public SecurityContext(DbContextOptions<SecurityContext> options)
            : base(options)
        {
        }

        #endregion

        #region Configuración del Context

        /// <summary>Configuración opcional del contexto. Si no está configurado desde fuera, aplica un comportamiento por defecto (NoTracking para lecturas).</summary>
        /// <param name="optionsBuilder">Constructor de opciones del contexto.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Sólo aplicamos la configuración por defecto si el contexto no fue configurado desde fuera.
            if (!optionsBuilder.IsConfigured)
            {
                // NoTracking por defecto para evitar overhead en consultas de solo lectura.
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }

        /// <summary>Configura el modelo aplicando los mapeos (IEntityTypeConfiguration).</summary>
        /// <param name="modelBuilder">ModelBuilder para configurar el modelo.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Forzar collation a nivel EF si es necesario en tu entorno.
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            // Aplica cualquier otro mapping que exista en el ensamblado.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecurityContext).Assembly);
        }

        #endregion
    }
}
