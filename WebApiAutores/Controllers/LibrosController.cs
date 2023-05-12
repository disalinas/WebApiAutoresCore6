using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Comun;
using Modelos.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper automapper;

        public LibrosController(ApplicationDbContext context, IMapper automapper)
        {
            this.context = context;
            this.automapper = automapper;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id)
        {
            var libroEntity = await this.context.Libros.FirstOrDefaultAsync(libro => libro.Id == id);
            var libroDTO = this.automapper.Map<LibroDTO>(libroEntity);

            return libroDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libro)
        {
            //var existeAutor = await this.context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);

            //if (!existeAutor)
            //{
            //    return BadRequest("No existe el autor indicado.");
            //}
            var libroEntity = this.automapper.Map<Libro>(libro);
            this.context.Add(libroEntity);
            await this.context.SaveChangesAsync();

            return Ok();
        }
    }
}
