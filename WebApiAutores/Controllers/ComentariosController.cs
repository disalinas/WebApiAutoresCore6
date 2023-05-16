using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Modelos.Comun;
using Modelos.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")] // Los comentarios dependen del libro indicado mediante su identificador.
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

            var comentariosEntity = await this.context.Comentarios.Where(item => item.LibroId == IdLibro).ToListAsync();
            var comentariosDTO = this.automapper.Map<List<ComentarioDTO>>(comentariosEntity);

            return comentariosDTO;
        }

        [HttpGet("{id:int}", Name = "ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetPorId(int id)
        {
            var comentario = await this.context.Comentarios.FirstOrDefaultAsync(item => item.Id == id);

            if (null == comentario)
            {
                return NotFound();
            }

            var comentarioDTO = this.automapper.Map<ComentarioDTO>(comentario);

            return comentarioDTO;
        }


        [HttpPost]   
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentario)
        {
            try
            {
                var existeLibro = await this.context.Libros.AnyAsync(libro => libro.Id == libroId);

                if (!existeLibro)
                {
                    return NotFound();
                }

                var comentarioEntity = this.automapper.Map<Comentario>(comentario);
                comentarioEntity.LibroId = libroId;
                this.context.Comentarios.Add(comentarioEntity);
                await this.context.SaveChangesAsync();

                var comentarioDTO = this.automapper.Map<ComentarioDTO>(comentarioEntity);

                //return Ok();
                return CreatedAtRoute("ObtenerComentario", new { libroId = libroId, id = comentarioEntity.Id }, comentarioDTO);
            }
            catch(Exception ex)
            {
                string aux = ex.ToString();
                return BadRequest(ex.Message);
            }
        }

        
    }
}
