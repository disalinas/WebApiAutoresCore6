using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name = "ObtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {
            var libroEntity = await this.context.Libros
                .Include(item => item.AutoresLibros)
                .ThenInclude(item => item.Autor)
                .Include(item => item.Comentarios)
                .FirstOrDefaultAsync(libro => libro.Id == id);
            libroEntity.AutoresLibros = libroEntity.AutoresLibros.OrderBy(item => item.Orden).ToList();
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
            this.AsignarOrdenAutores(libroEntity);
            this.context.Add(libroEntity);
            await this.context.SaveChangesAsync();

            var libroDTO = this.automapper.Map<LibroDTO>(libroEntity);

            //return Ok();
            return CreatedAtRoute("ObtenerLibro", new {id = libroEntity.Id }, libroDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, LibroCreacionDTO libro)
        {
            var libroDB = await this.context.Libros
                .Include(item => item.AutoresLibros)
                .FirstOrDefaultAsync(item => item.Id == id);

            if (null == libroDB)
            {
                return NotFound();
            }

            libroDB = this.automapper.Map(libro, libroDB);
            this.AsignarOrdenAutores(libroDB);
            await this.context.SaveChangesAsync();

            return NoContent();
        }

        private void AsignarOrdenAutores(Libro libro)
        {
            if (null != libro.AutoresLibros)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (null == patchDocument)
            {
                return BadRequest();
            }

            var libroEntity = await this.context.Libros.FirstOrDefaultAsync(item => item.Id == id);

            if (null == libroEntity)
            {
                return NotFound();
            }

            var libroDTO = this.automapper.Map<LibroPatchDTO>(libroEntity);
            patchDocument.ApplyTo(libroDTO, ModelState); // Aplicar las actualizaciones indicadas en 'patchDocument' en el objeto 'libroDTO'.
            var esValido = TryValidateModel(libroDTO); // Comprobar que las reglas de validación se estén cumpliento.

            if (!esValido)
            {
                return BadRequest(ModelState); // Se devuelven los errores de validación encontrados.
            }

            this.automapper.Map(libroDTO, libroEntity);
            await this.context.SaveChangesAsync();

            return NoContent();

        }

    }
}
