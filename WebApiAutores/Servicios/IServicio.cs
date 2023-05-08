namespace WebApiAutores.Servicios
{
    public interface IServicio
    {
        void RealizarTarea();
    }

    public class ServicioA : IServicio
    {
        public ILogger<ServicioA> Logger { get; }

        public ServicioA(ILogger<ServicioA> logger)
        {
            Logger = logger;
        }

        public void RealizarTarea()
        {
        }
    }

    public class ServicioB : IServicio
    {
        public void RealizarTarea()
        {
        }
    }
}
