namespace Api.Security.Middlewares
{
    using Domain.Security.CustomEntities.Params;
    using Domain.Security.Interfaces.Security;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>Middleware responsable de auditar todas las solicitudes y respuestas HTTP, registrando tanto los datos de entrada como los de salida en la base de datos,
    /// incluyendo información sobre el método, la ruta, el código de estado y cualquier error.
    /// </summary>
    public class RequestAuditingMiddleware : IMiddleware
    {
        private readonly IAuditoriaService _auditoriaService;

        /// <summary>
        /// Inicializa una nueva instancia del <see cref="RequestAuditingMiddleware"/>.
        /// </summary>
        /// <param name="auditoriaService">
        /// Servicio responsable de registrar la información de auditoría en la base de datos.
        /// </param>
        public RequestAuditingMiddleware(IAuditoriaService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        /// <summary>
        /// Intercepta cada solicitud HTTP para registrar sus datos y la respuesta generada.
        /// </summary>
        /// <param name="context">Contexto HTTP actual.</param>
        /// <param name="next">Delegado que representa el siguiente middleware en el pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            string requestBody = string.Empty;
            string responseBody = string.Empty;
            int statusCode = 500;

            // Evitar auditar Swagger, archivos estáticos y favicon
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/favicon.ico") ||
                context.Request.Path.StartsWithSegments("/_framework") || // Blazor y SPA
                context.Request.Path.StartsWithSegments("/css") ||
                context.Request.Path.StartsWithSegments("/js") ||
                context.Request.Path.StartsWithSegments("/lib"))
            {
                return;
            }

            // Capturar datos de entrada (request)
            context.Request.EnableBuffering();

            if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
            {
                using (var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
            }

            // Capturar la respuesta para leer su contenido
            var originalResponseBodyStream = context.Response.Body;
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            try
            {
                // Continuar con el pipeline
                await next(context);

                // Capturar código de respuesta
                statusCode = context.Response.StatusCode;

                // Leer la respuesta generada
                newResponseBody.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(newResponseBody);
                responseBody = await reader.ReadToEndAsync();
                newResponseBody.Seek(0, SeekOrigin.Begin);

                // Copiar de nuevo al cuerpo original
                await newResponseBody.CopyToAsync(originalResponseBodyStream);
            }
            catch (ValidationException e)
            {

                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.UnprocessableEntity,
                    Type = "Validation",
                    Title = "Validation",
                    Detail = e.Message
                };

                string json = JsonSerializer.Serialize(problem);
                responseBody = json;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            catch (Exception e)
            {

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server Error",
                    Title = "Server Error",
                    Detail = e.Message+ "."
                };

                string json = JsonSerializer.Serialize(problem);
                responseBody = json;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
            finally
            {
                // Registrar auditoría
                try
                {
                    var auditoria = new ParamsAuditoria
                    {
                        AuditoriaFecha = DateTime.Now,
                        AuditoriaMetodoHttp = context.Request.Method,
                        AuditoriaRuta = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}",
                        AuditoriaDatosEntrada = requestBody,
                        AuditoriaCodigoRespuesta = statusCode,
                        AuditoriaDatosSalida = responseBody
                    };

                    await _auditoriaService.CreateAsync(auditoria);
                }
                catch
                {
                    // Si falla el registro en BD, se ignora para no interrumpir la petición
                }

                if (!context.Response.HasStarted)
                {
                    context.Response.Body = originalResponseBodyStream;
                    if (!string.IsNullOrEmpty(responseBody))
                    {
                        await context.Response.WriteAsync(responseBody);
                    }
                }
                else
                {
                    // Si la respuesta ya empezó a escribirse, solo restauramos el stream
                    context.Response.Body = originalResponseBodyStream;
                }
            }
        }
    }
}
