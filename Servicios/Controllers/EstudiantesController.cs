using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Servicios.Data;
using Servicios.Models;

namespace Servicios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly ILogger<EstudiantesController> _logger;
        private readonly DataContext _context;

        public EstudiantesController(ILogger<EstudiantesController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;

        }

        [HttpGet(Name = "GetEstudiantes")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            return await _context.Estudiantes.ToListAsync();
        }
        [HttpGet("{id}", Name = "GetEstudiante")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
            {
                return NotFound();
            }

            return estudiante;
        }

        [HttpPost]
        public async Task<ActionResult<Estudiante>> Post(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("GetEstudiante", new { id = estudiante.Id }, estudiante);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id)
            {
                return BadRequest();
            }
            _context.Entry(estudiante).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Estudiante>> Delete(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
            {
                return NotFound();
            }
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return estudiante;
        }

        //Login

        [HttpPost("login")]
        public async Task<ActionResult<Estudiante>> Login(Estudiante estudiante)
        {
            var estudianteLogin = await _context.Estudiantes.FirstOrDefaultAsync(e => e.Correo == estudiante.Correo && e.Contraseña == estudiante.Contraseña);

            if (estudianteLogin == null )
            {
                return Unauthorized();
            }

            var estudianteResponse = new{
                estudianteLogin.Nombre,
                estudianteLogin.Apellido,
                estudianteLogin.FechaNacimiento,
                estudianteLogin.Correo,
                estudianteLogin.Contraseña
            };

            return Ok(estudianteResponse);
        }
    }
}