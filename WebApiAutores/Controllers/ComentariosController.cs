using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modelos.Comun;
using Modelos.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{IdLibro:int}/comentarios")] // Los comentarios dependen del libro indicado mediante su identificador.
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper automapper;

        public ComentariosController(ApplicationDbContext context, IMapper automapper)
        {
            this.context = context;
            this.automapper = automapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int IdLibro)
        {
            var existeLibro = await this.context.Libros.AnyAsync(libro => libro.Id == IdLibro);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentariosEntity = await this.context.Comentarios.Where(item => item.IdLibro == IdLibro).ToListAsync();
            var comentariosDTO = this.automapper.Map<List<ComentarioDTO>>(comentariosEntity);

            return comentariosDTO;
        }

        [HttpPost]   
        public async Task<ActionResult> Post(int IdLibro, ComentarioCreacionDTO comentario)
        {
            var existeLibro = await this.context.Libros.AnyAsync(libro => libro.Id == IdLibro);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentarioEntity = this.automapper.Map<Comentario>(comentario);
            comentarioEntity.Id = IdLibro;
            this.context.Comentarios.Add(comentarioEntity);
            await this.context.SaveChangesAsync();

            return Ok();
        }

        
    }
}
