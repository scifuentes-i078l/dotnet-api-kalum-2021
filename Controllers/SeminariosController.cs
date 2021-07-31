using System.Collections.Generic;
using System.Threading.Tasks;
using ApiKalumNotas.DbContexts;
using ApiKalumNotas.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using ApiKalumNotas.DTOs;
using AutoMapper;

namespace ApiKalumNotas.Controllers
{
[Route("/kalum-notas/v1/seminarios")]
    
    [ApiController]
    public class SeminariosController : ControllerBase
    {

        private readonly KalumNotasDBContext kalumNotasDBContext;

        private readonly ILogger<SeminariosController> logger;
        private readonly IMapper mapper;
        public SeminariosController(KalumNotasDBContext kalumNotasDBContext, ILogger<SeminariosController>  logger,IMapper mapper)
        {
            this.mapper = mapper;
            this.kalumNotasDBContext = kalumNotasDBContext;
            this.logger = logger;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeminarioDTO>>> GetSeminarios(){
            
            this.logger.LogDebug("Iniciando proceso de consulta de la informacion de seminarios");
            var seminarios  = await this.kalumNotasDBContext.Seminarios.ToListAsync();
            if (seminarios== null || seminarios.Count == 0){
                this.logger.LogWarning("No se encontraron registros");
                return new NoContentResult();
            }
            this.logger.LogInformation("Consulta realizada exitosamente");
            return Ok(mapper.Map<List<SeminarioDTO>>(seminarios));         
        }

          [HttpGet("{SeminarioId}", Name = "GetSeminario")]
       public async Task<ActionResult<SeminarioDTO>> GetSeminario(string SeminarioId){
           this.logger.LogDebug($"Iniciando proceso de consulta de la Seminario con el id = {SeminarioId}");
           var seminario  = await this.kalumNotasDBContext.Seminarios.Include(d =>d.Modulo).FirstOrDefaultAsync(c => c.SeminarioId == SeminarioId );
           if (seminario == null)
           {
               logger.LogWarning($"El modulo con el id {SeminarioId} no existe");
               return NotFound();
           }
           else{
               logger.LogInformation("Se ejecuto exitosamente la consulta");
               return Ok(mapper.Map<SeminarioDTO>(seminario));
           }
       }

       [HttpPost]
        public async Task<ActionResult<SeminarioDTO>> PostSeminario([FromBody] SeminarioDTO NuevoSeminario)
        {
            logger.LogDebug("Iniciando el proceso de un nuevo seminario");
            logger.LogDebug($" fe {NuevoSeminario.FechaInicio}");

            if (NuevoSeminario.FechaFinal < NuevoSeminario.FechaInicio){
                logger.LogDebug("Fecha final no puede ser menor a fecha inicio");
                  return BadRequest("Fecha final no puede ser menor a fecha inicio");
            }
            
            logger.LogDebug($"Realiando la consulta del modulo con el id {NuevoSeminario.ModuloId}");
            Modulo modulo = await this.kalumNotasDBContext.Modulos.FirstOrDefaultAsync(c => c.ModuloID== NuevoSeminario.ModuloId);
            if (modulo == null){
                logger.LogInformation($"No existe el modulo con el id { NuevoSeminario.ModuloId}");
                return BadRequest();
            }
            NuevoSeminario.SeminarioId=Guid.NewGuid().ToString();
            var seminario =mapper.Map<Seminario>(NuevoSeminario);
            await this.kalumNotasDBContext.Seminarios.AddAsync(seminario);
            await this.kalumNotasDBContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetSeminario",new {SeminarioId = NuevoSeminario.SeminarioId}, mapper.Map<SeminarioDTO>(seminario));
        }


       [HttpDelete("{SeminarioId}")]
       public async Task<ActionResult> DeleteSeminario(String SeminarioId){
           logger.LogDebug("Iniciando el proceso de eliminacion del Seminario");
           
           Seminario Seminario = await this.kalumNotasDBContext.Seminarios.FirstOrDefaultAsync(a => a.SeminarioId==SeminarioId );
           if (Seminario == null)
           {
               logger.LogInformation($"No existe el Seminario con el ID {SeminarioId}");
               return NotFound();
           }
           else{
               this.kalumNotasDBContext.Seminarios.Remove(Seminario);
               await this.kalumNotasDBContext.SaveChangesAsync();
               logger.LogInformation($"Se ha realizado la eliminacion del registro con id {SeminarioId}");
               return NoContent();
           }
       }

       [HttpPut("{SeminarioId}")]
         public async Task<ActionResult> PutSeminario(string SeminarioId, [FromBody] SeminarioDTO ActualizarSeminario)
         {
             logger.LogDebug($"Inicio del proceso de modificacion de un Seminario con el id {SeminarioId}");
            if (ActualizarSeminario.FechaFinal < ActualizarSeminario.FechaInicio){
                logger.LogDebug("Fecha final no puede ser menor a fecha inicio");
                  return BadRequest("Fecha final no puede ser menor a fecha inicio");
            }

             Seminario Seminario = await this.kalumNotasDBContext.Seminarios.FirstOrDefaultAsync(a => a.SeminarioId == SeminarioId);
             if (Seminario ==null)
             {
                 logger.LogInformation($"No existe el Seminario con el id {SeminarioId}");
                 return NotFound();
                 //o reutnr NoContent() o BadRequest();
             }
             else
             {
               logger.LogDebug("Iniciando el proceso de modificacion del Seminario");
               logger.LogDebug($"Realiando la consulta del modulo con el id {ActualizarSeminario.ModuloId}");
            Modulo modulo = await this.kalumNotasDBContext.Modulos.FirstOrDefaultAsync(c => c.ModuloID== ActualizarSeminario.ModuloId);
            if (modulo == null){
                logger.LogInformation($"No existe el modulo con el id { ActualizarSeminario.ModuloId}");
                return BadRequest();
            }

            }
            Seminario.NombreSeminario = ActualizarSeminario.NombreSeminario;
            Seminario.ModuloId = ActualizarSeminario.ModuloId;
            Seminario.FechaInicio = ActualizarSeminario.FechaInicio;
            Seminario.FechaFinal = ActualizarSeminario.FechaFinal;
            this.kalumNotasDBContext.Entry(Seminario).State = EntityState.Modified;
            await this.kalumNotasDBContext.SaveChangesAsync();
            logger.LogInformation("Los datos del Seminario fueron actualizados exitosamente");
            return NoContent();

         }
        
    }
}
