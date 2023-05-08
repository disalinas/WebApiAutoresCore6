using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApiAutores.Servicios;

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
            // Para evitar ciclos en EF al recuperar datos, cambio la siguiente instrucción por la de la línea que hay a continuación,
            // estableciendo una configuración JSON.
            //services.AddControllers();
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            // Con la siguiente línea se consigue establecer la inyección de dependencia de 'ApplicationDbContext' en aquellos objetos 
            // que tengan declarado como parámetro en su constructor dicho tipo.
            // El servicio se configura como 'scoped'.
            services.AddDbContext<Modelos.Entidades.ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));
            // Crear servicio transitorio. Al sistema de inyección de dependencias se le está diciendo que cuando una clase requiera de un objeto de tipo
            // 'IServicio', tiene que pasarle una nueva instancia del tipo 'ServicioA'. En el caso de 'ServicioA' tiene depedencia de 'ILogger', pero el sistema
            // de inyección de dependencias ya se encarga de ella. Como el servicio 'ILogger' ya viene configurado por defecto, el sistema de inyección de
            // dependencias puede resolverlo de manera automática.
            services.AddTransient<IServicio, ServicioA>();

            // Dentro del mismo contexto http, obtendremos el mismo objeto 'ServicioA'. Pero para distintas peticiones http obtendremos distintos objetos.
            //services.AddScoped<IServicio, ServicioA>();

            // Con un singleton obtendremos siempre el mismo objeto, sin importar el contexto de la llamada. Todas las llamadas compartirían el mismo objeto 
            // 'ServicioA'.
            //services.AddSingleton<IServicio, ServicioA>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        /// <summary>
        /// Para configurar middleware. Ejemplo: Swagger.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
