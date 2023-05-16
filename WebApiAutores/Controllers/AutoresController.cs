using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Comun;
using Modelos.Entidades;
using WebApiAutores.Filtros;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper automapper;

        public AutoresController(ApplicationDbContext context, IMapper automapper) 
        {
            this.context = context;
            this.automapper = automapper;
        }

        [HttpGet]               // api/autores        
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autoresEntity = await this.context.Autores.ToListAsync();
            var autores = this.automapper.Map<List<AutorDTO>>(autoresEntity);
            
            return autores;
        }

        [HttpGet("{id:int}")]   // api/autores/{id}
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var autorEntity = await this.context.Autores
                .Include(item => item.AutoresLibros)
                .ThenInclude(item => item.Libro)
                .FirstOrDefaultAsync(autor => autor.Id == id);
                
            if (null == autorEntity)
            {
                return NotFound();
            }

            var autor = this.automapper.Map<AutorDTOConLibros>(autorEntity);

            return autor;
        }

        [HttpGet("{nombre}")]   // api/autores/{id}
        public async Task<ActionResult<List<AutorDTO>>> Get(string nombre)
        {
            var autoresEntity = await this.context.Autores.Where(autor => autor.Nombre.Contains(nombre)).ToListAsync();
            var autores = this.automapper.Map<List<AutorDTO>>(autoresEntity);

            return autores;
        }


        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacion)
        {
            var existeAutor = await this.context.Autores.AnyAsync(item => item.Nombre == autorCreacion.Nombre);

            if (existeAutor)
            {
                return BadRequest(string.Format("Ya existe un autor con el mismo nombre '{0}'.", autorCreacion.Nombre));
            }

            var autor = this.automapper.Map<Autor>(autorCreacion);
            this.context.Autores.Add(autor);
            await this.context.SaveChangesAsync();

            var autorDTO = this.automapper.Map<AutorDTO>(autor);

            return CreatedAtRoute(new { id = autor.Id }, autorDTO);
            //return Ok();
        }

        [HttpPut("{id:int}")] // api/autores/id  : sólo aceptamos valores numéricos.
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("El identificador del autor no coincide con el identificador de la URL.");
            }

            var existe = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
