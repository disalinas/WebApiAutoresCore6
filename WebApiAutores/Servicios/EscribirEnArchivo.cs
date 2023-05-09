using System.Text;

namespace WebApiAutores.Servicios
{
    /// <summary>
    /// Para escribir en un fichero cada 5 segundos.
    /// </summary>
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string nombreArchivo = "Archivo1.txt";
        private Timer timer;

        public EscribirEnArchivo(IWebHostEnvironment env)
        {
            this.env = env;
        }

        /// <summary>
        /// Se ejecuta cuando arranque la API.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            this.Escribir("Proceso iniciado.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Se ejecuta cuando se detiene la API con normalidad.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer.Dispose();
            this.Escribir("Proceso finalizado.");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            this.Escribir("Proceso en ejecución: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        public void Escribir(string mensaje)
        {
            StringBuilder ruta = new StringBuilder()
                .Append(env.ContentRootPath)
                .Append("\\wwwroot\\")
                .Append(this.nombreArchivo);

            using (StreamWriter writer = new StreamWriter(ruta.ToString(), append: true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
