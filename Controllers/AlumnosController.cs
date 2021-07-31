using System.Collections.Generic;
using System.Threading.Tasks;
using ApiKalumNotas.DbContexts;
using ApiKalumNotas.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ApiKalumNotas.DTOs;
using Microsoft.Data.SqlClient;
using System;
using AutoMapper;

namespace ApiKalumNotas.Controllers
{
    [Route("/kalum-notas/v1/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly KalumNotasDBContext kalumNotasDBContext;
        private readonly ILogger<AlumnosController> logger;
        private readonly IMapper mapper;
        public AlumnosController(KalumNotasDBContext kalumNotasDBContext, ILogger<AlumnosController> logger, IMapper mapper)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.kalumNotasDBContext = kalumNotasDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetAlumnos()
        {
            List<Alumno> alumnos = null;
            this.logger.LogDebug("Iniciando el proceso de consulta de la información de alumnos");
            alumnos = await this.kalumNotasDBContext.Alumnos.ToListAsync();
            if (alumnos == null || alumnos.Count == 0)
            {
                this.logger.LogWarning("No se encontraron registros en la tabla alumnos");
                return new NoContentResult();
            }
            this.logger.LogInformation("La consulta se realizo exitosamente");
            return Ok(alumnos);
        }
        [HttpGet("{carne}", Name = "GetAlumno")]
        public async Task<ActionResult<Alumno>> GetAlumno(string carne)
        {
            this.logger.LogDebug("Iniciando el proceso de la consulta de alumno por carné");
            var alumno = await this.kalumNotasDBContext.Alumnos.FirstOrDefaultAsync(a => a.Carne == carne);
            if (alumno == null)
            {
                logger.LogWarning($"No existe el alumno con el carné {carne}");
                return NotFound();
            }
            else
            {
                logger.LogInformation("Consulta ejecutada exitosamente");
                return Ok(alumno);
            }
        }
        [HttpPost]
        public async Task<ActionResult<AlumnoDTO>> Post([FromBody] AlumnoCreateDTO value)
        {
            logger.LogDebug("Iniciando el proceso para la creación de un nuevo alumno");
            logger.LogDebug("Iniciando el proceso de la llamada del sp_registrar_alumno ");
            AlumnoDTO alumnoDTO = null;
            var ApellidosParameter = new SqlParameter("@Apellidos", value.Apellidos);
            var NombresParameter = new SqlParameter("@Nombres", value.Nombres);
            var EmailParameter = new SqlParameter("@Email", value.Email);
            var Resultado = await this.kalumNotasDBContext.Alumnos
                                                .FromSqlRaw("sp_registrar_alumno @Apellidos, @Nombres, @Email", 
                                                    ApellidosParameter, NombresParameter, EmailParameter)
                                                .ToListAsync();
            logger.LogDebug($"Resultado de procedimiento almacenado ${Resultado}");
            if(Resultado.Count == 0)
            {
                return NoContent();
            }
            foreach (Object registro in Resultado)
            {
                alumnoDTO = mapper.Map<AlumnoDTO>(registro);
            }
            return new CreatedAtRouteResult("GetAlumno", new { carne = alumnoDTO.Carne}, alumnoDTO);
        }

    }
}