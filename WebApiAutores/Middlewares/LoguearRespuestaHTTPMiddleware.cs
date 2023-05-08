using Microsoft.Extensions.Logging;

namespace WebApiAutores.Middlewares
{
    public static class LoguearRespuestaHTTPMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        }
    }

    public class LoguearRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate siguiente;
        private readonly ILogger<LoguearRespuestaHTTPMiddleware> logger;

        public LoguearRespuestaHTTPMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHTTPMiddleware> logger)
        {
            this.siguiente = siguiente;
            this.logger = logger;
        }

        // Invoke o InvokeAsync -> devuelve Task.
        public async Task InvokeAsync(HttpContext contexto)
        {
            using (var ms = new MemoryStream())
            {
                var cuerpoOriginalRespuesta = contexto.Response.Body;
                contexto.Response.Body = ms;

                //await siguiente.Invoke(); // Continuamos con el pipeline (las llamadas encadenadas del pipeline).
                await this.siguiente(contexto); // Es lo mismo que la línea de arriba, pero al no estar en Startup hay que hacerlo así.

                // Leemos la respuesta, la guardamos en el string 'respuesta' y posicionamos en el inicio el cursor de lectura.
                ms.Seek(0, SeekOrigin.Begin);
                string respuesta = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);
                // Copiamos el contenido de la respesta en la respuesta original.
                await ms.CopyToAsync(cuerpoOriginalRespuesta);
                contexto.Response.Body = cuerpoOriginalRespuesta;

                this.logger.LogInformation(respuesta);
            }
        }
    }
}
