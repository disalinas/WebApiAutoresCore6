using Microsoft.AspNetCore.Mvc;
using Modelos.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : Controller
    {
        [HttpGet]
        public ActionResult<List<Autor>> Get()
        {
            return new List<Autor>()
            {
                new Autor() { Id = 1, Nombre = "Dean R. Koontz" },
                new Autor() { Id = 2, Nombre = "Stephen King" }
            };
        }
    }
}
