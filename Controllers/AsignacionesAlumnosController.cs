using System.Collections.Generic;
using System.Threading.Tasks;
using ApiKalumNotas.DbContexts;
using ApiKalumNotas.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using AutoMapper;
using ApiKalumNotas.DTOs;

namespace ApiKalumNotas.Controllers
{
    [Route("/kalum-notas/v1/asignaciones")]
    //[Route("/kalum-notas/v1/alumnos]")]
    [ApiController]
    public class AsignacionesAlumnosController : ControllerBase
    {


        private readonly KalumNotasDBContext kalumNoasDBContext;

        private readonly ILogger<AsignacionesAlumnosController> logger;

        private readonly IMapper mapper;

        public AsignacionesAlumnosController(KalumNotasDBContext kalumNoasDBContext, ILogger<AsignacionesAlumnosController> logger, IMapper mapper)
        {
            this.mapper = mapper;
            this.kalumNoasDBContext = kalumNoasDBContext;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AsignacionAlumnoDetalleDTO>>> GetAsignaciones()
        {

            this.logger.LogDebug("Iniciando proceso de consulta de la informacion de asignaciones");
            var asignaciones = await this.kalumNoasDBContext.AsignacionesAlumnos.Include(a => a.Alumno).Include(c => c.Clase).ToListAsync();
            if (asignaciones == null || asignaciones.Count == 0)
            {
                this.logger.LogWarning("No se encontraron registros");
                return new NoContentResult();
            }
            List<AsignacionAlumnoDetalleDTO> asignacionAlumnoDTO = mapper.Map<List<AsignacionAlumnoDetalleDTO>>(asignaciones);
            this.logger.LogInformation("Consulta realizada exitosamente");
            return Ok(asignacionAlumnoDTO);
        }

        [HttpGet("{asignacionId}", Name = "GetAsignacion")]
        public async Task<ActionResult<AsignacionAlumnoDetalleDTO>> GetAsignacion(string asignacionId)
        {
            this.logger.LogDebug($"Iniciando proceso de consulta de la asignacion con el id = {asignacionId}");
            var asignacion = await this.kalumNoasDBContext.AsignacionesAlumnos.Include(a => a.Alumno).Include(a => a.Clase).FirstOrDefaultAsync(c => c.AsignacionId == asignacionId);
            if (asignacion == null)
            {
                logger.LogWarning($"La asignacion con el id {asignacionId} no existe");
                return NotFound();
            }
            else
            {
                logger.LogInformation("Se ejecuto exitosamente la consulta");
                var asignacionAlumnoDTO = mapper.Map<AsignacionAlumnoDetalleDTO>(asignacion);
                return Ok(asignacionAlumnoDTO);
            }
        }


        [HttpPost]
        public async Task<ActionResult<AsignacionAlumnoDetalleDTO>> PostAsignacion([FromBody] AsignacionAlumnoDTO nuevaAsignacion)
        {
            logger.LogDebug("Iniciando el proceso de una nueva asignacion");
            logger.LogDebug($"Realizando la consulta del alumno con el carne {nuevaAsignacion.Carne}");
            Alumno alumno = await this.kalumNoasDBContext.Alumnos.FirstOrDefaultAsync(a => a.Carne == nuevaAsignacion.Carne);
            if (alumno == null)
            {
                logger.LogInformation($"No existe el alumno con el carne {nuevaAsignacion.Carne}");
                return BadRequest();
            }
            logger.LogDebug($"Realiando la consulta de la clase con el id {nuevaAsignacion.ClaseId}");
            Clase clase = await this.kalumNoasDBContext.Clases.FirstOrDefaultAsync(c => c.ClaseId == nuevaAsignacion.ClaseId);
            if (clase == null)
            {
                logger.LogInformation($"No existe la clase con el id { nuevaAsignacion.ClaseId}");
                return BadRequest();
            }
            nuevaAsignacion.AsignacionId = Guid.NewGuid().ToString();
            var asignacion = mapper.Map<AsignacionAlumno>(nuevaAsignacion);
            await this.kalumNoasDBContext.AsignacionesAlumnos.AddAsync(asignacion);
            await this.kalumNoasDBContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetAsignacion", new { asignacionId = nuevaAsignacion.AsignacionId }, 
                mapper.Map<AsignacionAlumnoDetalleDTO>(asignacion));
        }

        [HttpPut("{asignacionId}")]
        public async Task<ActionResult> PutAsignacion(string asignacionId, [FromBody] AsignacionAlumnoDTO  ActualizarAsignacion)
        {
            logger.LogDebug($"Inicio del proceso de modificacion de una asignacion con el id {asignacionId}");
            AsignacionAlumno asignacion = await this.kalumNoasDBContext.AsignacionesAlumnos.FirstOrDefaultAsync(a => a.AsignacionId == asignacionId);
            if (asignacion == null)
            {
                logger.LogInformation($"No existe la asignacion con el id {asignacionId}");
                return NotFound();
                //o reutnr NoContent() o BadRequest();
            }
            else
            {
               
                logger.LogDebug($"Realizando la consulta del alumno con el carne {ActualizarAsignacion.Carne}");
                Alumno alumno = await this.kalumNoasDBContext.Alumnos.FirstOrDefaultAsync(a => a.Carne == ActualizarAsignacion.Carne);
                if (alumno == null)
                {
                    logger.LogInformation($"No existe el alumno con el carne {ActualizarAsignacion.Carne}");
                    return BadRequest();
                }

                logger.LogDebug($"Realiando la consulta de la clase con el id {ActualizarAsignacion.ClaseId}");
                Clase clase = await this.kalumNoasDBContext.Clases.FirstOrDefaultAsync(c => c.ClaseId == ActualizarAsignacion.ClaseId);
                if (clase == null)
                {
                    logger.LogInformation($"No existe la clase con el id { ActualizarAsignacion.ClaseId}");
                    return BadRequest();
                }
            }
            asignacion.Carne = ActualizarAsignacion.Carne;
            asignacion.ClaseId = ActualizarAsignacion.ClaseId;
            asignacion.FechaAsignacion = ActualizarAsignacion.FechaAsignacion;
            this.kalumNoasDBContext.Entry(asignacion).State = EntityState.Modified;
            await this.kalumNoasDBContext.SaveChangesAsync();
            logger.LogInformation("Los datos de la asignacion fueron actualizados exitosamente");
            return NoContent();

        }

        [HttpDelete("{asignacionId}")]

        public async Task<ActionResult> DeleteAsignacion(String asignacionId)
        {
            logger.LogDebug("Iniciando el proceso de eliminacion de la asignacion");
            AsignacionAlumno asignacion = await this.kalumNoasDBContext.AsignacionesAlumnos.FirstOrDefaultAsync(a => a.AsignacionId == asignacionId);
            if (asignacion == null)
            {
                logger.LogInformation($"No existe la asignacion con el ID {asignacionId}");
                return NotFound();
            }
            this.kalumNoasDBContext.AsignacionesAlumnos.Remove(asignacion);
            await this.kalumNoasDBContext.SaveChangesAsync();
            logger.LogInformation($"Se ha realizado la elimnacin del registro con id {asignacionId}");
            return NoContent();
          
        }


    }
}