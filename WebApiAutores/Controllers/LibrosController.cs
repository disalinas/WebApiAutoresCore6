using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await this.context.Libros.Include(libro => libro.Autor).FirstOrDefaultAsync(libro => libro.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await this.context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest("No existe el autor indicado.");
            }

            this.context.Add(libro);
            await this.context.SaveChangesAsync();

            return Ok();
        }
    }
}
