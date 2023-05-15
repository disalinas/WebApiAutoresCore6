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
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var libroEntity = await this.context.Libros
                .Include(item => item.AutoresLibros)
                .ThenInclude(item => item.Autor)
                .Include(item => item.Comentarios)
                .FirstOrDefaultAsync(libro => libro.Id == id);
            var libroDTO = this.automapper.Map<LibroDTOConAutores>(libroEntity);

            return libroDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libro)
        {
            if (null == libro.AutoresIds)
            {
                return BadRequest("No se puede crear un libro sin especificar el autor.");
            }

            var autoresIds = await this.context.Autores
                .Where(autor => libro.AutoresIds
                .Contains(autor.Id))
                .Select(autor => autor.Id)
                .ToListAsync();

            if (autoresIds.Count() != libro.AutoresIds.Count())
            {
                return BadRequest("No existe alguno de los autores indicados.");
            }

            var libroEntity = this.automapper.Map<Libro>(libro);
            this.context.Add(libroEntity);
            await this.context.SaveChangesAsync();

            return Ok();
        }
    }
}
