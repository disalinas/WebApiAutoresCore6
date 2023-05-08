using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;

        public AutoresController(ApplicationDbContext context, IServicio servicio) 
        {
            this.context = context;
            this.servicio = servicio;
        }

        [HttpGet]               // api/autores
        [HttpGet("listado")]    // api/autores/listado
        [HttpGet("/listado")]   // listado
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await this.context.Autores.Include(autor => autor.Libros).ToListAsync();
        }

        [HttpGet("{id:int}")]   // api/autores/{id}
        //[HttpGet("{id:int}/{param2?}")]   // api/autores/{id}/{param2}  donde {param2} es opcional (?).
        //[HttpGet("{id:int}/{param2=persona}")]   // api/autores/{id}/{param2}  donde {param2} tiene valor por defecto, persona, si no se especifica.
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await this.context.Autores.FirstOrDefaultAsync(autor => autor.Id == id);

            if (null == autor)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpGet("{nombre}")]   // api/autores/{id}
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await this.context.Autores.FirstOrDefaultAsync(autor => autor.Nombre.Contains(nombre));

            if (null == autor)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpGet("primero")]    // api/autores/primero
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await this.context.Autores.FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            var existeAutor = await this.context.Autores.AnyAsync(autor => autor.Nombre == autor.Nombre);

            if (existeAutor)
            {
                return BadRequest(string.Format("Ya existe un autor con el mismo nombre '{0}'.", autor.Nombre));
            }

            this.context.Add(autor);
            await context.SaveChangesAsync();

            return Ok();
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
