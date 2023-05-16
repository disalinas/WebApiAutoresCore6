using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiAutores.Filtros;
using WebApiAutores.Middlewares;

namespace WebApiAutores
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Para configurar servicios.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            
            //services.AddControllers();
            services
                // Registramos el filtro de manera global.
                .AddControllers(opciones => opciones.Filters.Add(typeof(FiltroDeExcepcion))) 
                // Para evitar ciclos en EF al recuperar datos, cambio la instrucción inicial por la de la línea que hay a continuación,
                // estableciendo una configuración JSON.
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
                .AddNewtonsoftJson(); // Configurar NewtonSoft.
            
            // Con la siguiente línea se consigue establecer la inyección de dependencia de 'ApplicationDbContext' en aquellos objetos 
            // que tengan declarado como parámetro en su constructor dicho tipo.
            // El servicio se configura como 'scoped'.
            services.AddDbContext<Modelos.Entidades.ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            
            // Crear servicio transitorio. Al sistema de inyección de dependencias se le está diciendo que cuando una clase requiera de un objeto de tipo
            // 'IServicio', tiene que pasarle una nueva instancia del tipo 'ServicioA'. En el caso de 'ServicioA' tiene depedencia de 'ILogger', pero el sistema
            // de inyección de dependencias ya se encarga de ella. Como el servicio 'ILogger' ya viene configurado por defecto, el sistema de inyección de
            // dependencias puede resolverlo de manera automática.
            //services.AddTransient<IServicio, ServicioA>();

            // Dentro del mismo contexto http, obtendremos el mismo objeto 'ServicioA'. Pero para distintas peticiones http obtendremos distintos objetos.
            //services.AddScoped<IServicio, ServicioA>();

            // Con un singleton obtendremos siempre el mismo objeto, sin importar el contexto de la llamada. Todas las llamadas compartirían el mismo objeto 
            // 'ServicioA'.
            //services.AddSingleton<IServicio, ServicioA>();

            services.AddTransient<MiFiltroDeAccion>();
            //services.AddHostedService<EscribirEnArchivo>();

            //services.AddResponseCaching(); // Para poder utilizar caché en la aplicación.
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(); // Para poder utilizar autenticación.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebAPIAutores", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup)); // Configurar AutoMapper.
        }

        /// <summary>
        /// Para configurar middleware. Ejemplo: Swagger.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Capturar todas las respuestas que los controladores realicen a las llamadas.
            //app.Use(async (contexto, siguiente) =>
            //{
            //    // Las respuestas de la petición están en un buffer, por lo que hace falta utilizar un memorystream para después colocarla en la respuésta (buffer).
            //    using (var ms = new MemoryStream())
            //    {
            //        var cuerpoOriginalRespuesta = contexto.Response.Body;
            //        contexto.Response.Body = ms;

            //        await siguiente.Invoke(); // Continuamos con el pipeline (las llamadas encadenadas del pipeline).

            //        // Leemos la respuesta, la guardamos en el string 'respuesta' y posicionamos en el inicio el cursor de lectura.
            //        ms.Seek(0, SeekOrigin.Begin);
            //        string respuesta = new StreamReader(ms).ReadToEnd();
            //        ms.Seek(0, SeekOrigin.Begin);
            //        // Copiamos el contenido de la respesta en la respuesta original.
            //        await ms.CopyToAsync(cuerpoOriginalRespuesta);
            //        contexto.Response.Body = cuerpoOriginalRespuesta;

            //        logger.LogInformation(respuesta);
            //    }
            //});
            // El código de arriba está dentro de 'LoguearRespuestaHTTPMiddleware'.
            //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            app.UseLoguearRespuestaHTTP(); // En vez de utilizar la línea de arriba, utilizo una extensión que hace eso mismo. Así no se expone la clase.

            //app.Map("/ruta1", app =>
            //{ // Si se quiere acceder a /ruta1, entonces se ejecuta lo que hay aquí dentro y no se ejecutará lo que haya fuera de él.
            //    app.Run(async contexto =>
            //    {
            //        await contexto.Response.WriteAsync("Intercepción del pipeline.");
            //    });
            //});
            

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //app.UseSwaggerUI();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIAutores v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseResponseCaching(); // Para utilizar caché en la aplicación. Hace falta establecer el servicio más arriba.
            //app.UseAuthorization(); // Para utilizar autenticación.
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
