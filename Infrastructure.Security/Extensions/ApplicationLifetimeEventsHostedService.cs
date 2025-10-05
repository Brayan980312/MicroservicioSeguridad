namespace Infrastructure.Security.Extensions
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Servicio hospedado que se suscribe a los eventos del ciclo de vida de la aplicación.
    /// Este servicio implementa <see cref="IHostedService"/> y utiliza <see cref="IHostApplicationLifetime"/> para ejecutar lógica personalizada cuando la aplicación:
    /// - Se inicia completamente (<see cref="IHostApplicationLifetime.ApplicationStarted"/>).
    /// - Comienza su proceso de apagado (<see cref="IHostApplicationLifetime.ApplicationStopping"/>).
    /// - Se ha detenido por completo (<see cref="IHostApplicationLifetime.ApplicationStopped"/>).
    /// Se recomienda usar este servicio para inicialización de recursos globales, cierre ordenado de conexiones, 
    /// notificación a sistemas externos o auditoría de eventos de ciclo de vida.
    /// </summary>
    public class ApplicationLifetimeEventsHostedService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly ILogger<ApplicationLifetimeEventsHostedService> _logger;

        /// <summary> Crea una nueva instancia de <see cref="ApplicationLifetimeEventsHostedService"/>.</summary>
        /// <param name="appLifetime">Interfaz que expone los tokens de cancelación y callbacks asociados al ciclo de vida de la aplicación.</param>
        /// <param name="logger">Instancia de <see cref="ILogger"/> para registrar mensajes durante los eventos del ciclo de vida.</param>
        public ApplicationLifetimeEventsHostedService(
            IHostApplicationLifetime appLifetime,
            ILogger<ApplicationLifetimeEventsHostedService> logger)
        {
            _appLifetime = appLifetime;
            _logger = logger;
        }

        /// <summary>Método invocado al iniciar el servicio hospedado. 
        /// Registra las acciones a ejecutar en los eventos <c>ApplicationStarted</c>, <c>ApplicationStopping</c> y <c>ApplicationStopped</c>.
        /// </summary>
        /// <param name="cancellationToken">Token que indica si el inicio del servicio ha sido cancelado.</param>
        /// <returns>Tarea completada de manera síncrona.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        /// <summary>Acción ejecutada cuando la aplicación ha finalizado correctamente su inicio y está lista para aceptar peticiones.
        /// 
        /// Uso típico:
        /// - Validar conectividad con recursos externos (BD, Redis, colas, etc.).
        /// - Precargar datos en caché.
        /// - Registrar en logs o auditoría que el servicio se encuentra operativo.
        /// </summary>
        private void OnStarted()
        {
            _logger.LogInformation("Microservicio de Seguridad iniciado");
        }

        /// <summary>Acción ejecutada cuando la aplicación está en proceso de apagarse, pero aún no se ha detenido por completo.
        /// 
        /// Uso típico:
        /// - Cerrar conexiones de manera ordenada.
        /// - Guardar datos transitorios.
        /// - Notificar a sistemas externos que el servicio entrará en mantenimiento.
        /// </summary>
        private void OnStopping()
        {
            _logger.LogWarning("Microservicio de Seguridad se está deteniendo...");
        }

        /// <summary>Acción ejecutada una vez que la aplicación ha terminado completamente su proceso de apagado.
        /// 
        /// Uso típico:
        /// - Liberar recursos en memoria.
        /// - Confirmar el apagado en sistemas de monitoreo.
        /// - Registrar auditoría final.
        /// </summary>
        private void OnStopped()
        {
            _logger.LogWarning("Microservicio de Seguridad detenido");
        }

        /// <summary>Método invocado al detener el servicio hospedado. 
        /// Generalmente se utiliza para realizar tareas de limpieza final que no dependan del ciclo de vida del <see cref="IHostApplicationLifetime"/>.
        /// </summary>
        /// <param name="cancellationToken">Token que indica si el apagado fue forzado.</param>
        /// <returns>Tarea completada de manera síncrona.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
