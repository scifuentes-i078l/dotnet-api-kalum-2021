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
    [Route("/kalum-notas/v1/detalleNotas")]
    
    [ApiController]
    public class DetalleNotasController : ControllerBase
    {

        private readonly KalumNotasDBContext kalumNotasDBContext;

        private readonly ILogger<DetalleNotasController> logger;

         private readonly IMapper mapper;
        public DetalleNotasController(KalumNotasDBContext kalumNotasDBContext, ILogger<DetalleNotasController>  logger,IMapper mapper)
        {  
            this.mapper = mapper;

             this.kalumNotasDBContext = kalumNotasDBContext;
            this.logger = logger;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleNotaDTO>>> GetDetalleNotaes(){
            
            this.logger.LogDebug("Iniciando proceso de consulta de la informacion de detalleNotas");
            var detalleNotas  = await this.kalumNotasDBContext.DetalleNotas.ToListAsync();
            if (detalleNotas== null || detalleNotas.Count == 0){
                this.logger.LogWarning("No se encontraron registros");
                return new NoContentResult();
            }
            this.logger.LogInformation("Consulta realizada exitosamente");
            List<DetalleNotaDTO> detalleNotasDTO = mapper.Map<List<DetalleNotaDTO>>(detalleNotas);
            return Ok(detalleNotasDTO);         
        }

          [HttpGet("{DetalleNotaId}", Name = "GetDetalleNota")]
       public async Task<ActionResult<DetalleNotaDetalleDTO>> GetDetalleNota(string DetalleNotaId){
           this.logger.LogDebug($"Iniciando proceso de consulta de la DetalleNota con el id = {DetalleNotaId}");
           var detalleNota  = await this.kalumNotasDBContext.DetalleNotas.Include(d =>d.DetalleActividad).Include(a => a.Alumno).FirstOrDefaultAsync(c => c.DetalleNotaId == DetalleNotaId );
           if (detalleNota == null)
           {
               logger.LogWarning($"El detalleNota con el id {DetalleNotaId} no existe");
               return NotFound();
           }
           else{
               logger.LogInformation("Se ejecuto exitosamente la consulta");
               
               return Ok(mapper.Map<DetalleNotaDetalleDTO>(detalleNota));
           }
       }

       [HttpPost]
        public async Task<ActionResult<DetalleNotaDTO>> PostDetalleNota([FromBody] DetalleNotaDTO NuevoDetalleNota)
        {
            logger.LogDebug("Iniciando el proceso de un nuevo detalleNota");
            logger.LogDebug($"Realiando la consulta del Detalle de Actividad con el id {NuevoDetalleNota.DetalleActividadId}");
            DetalleActividad detalleActividad = await this.kalumNotasDBContext.DetalleActividades.FirstOrDefaultAsync(c => c.DetalleActividadId== NuevoDetalleNota.DetalleActividadId);
            if (detalleActividad == null){
                logger.LogInformation($"No existe el Detalle de Actividad con el id { NuevoDetalleNota.DetalleActividadId}");
                return BadRequest();
            }
            NuevoDetalleNota.DetalleNotaId=Guid.NewGuid().ToString();
            var detalleNota= mapper.Map<DetalleNota>(NuevoDetalleNota);
            await this.kalumNotasDBContext.DetalleNotas.AddAsync(detalleNota);
            await this.kalumNotasDBContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetDetalleNota",new {DetalleNotaId = NuevoDetalleNota.DetalleNotaId}, mapper.Map<DetalleNotaDTO>(detalleNota));
        }


       [HttpDelete("{DetalleNotaId}")]
       public async Task<ActionResult> DeleteDetalleNota(String DetalleNotaId){
           logger.LogDebug("Iniciando el proceso de eliminacion del DetalleNota");
           DetalleNota DetalleNota = await this.kalumNotasDBContext.DetalleNotas.FirstOrDefaultAsync(a => a.DetalleNotaId==DetalleNotaId );
           if (DetalleNota == null)
           {
               logger.LogInformation($"No existe el DetalleNota con el ID {DetalleNotaId}");
               return NotFound();
           }
           else{
               this.kalumNotasDBContext.DetalleNotas.Remove(DetalleNota);
               await this.kalumNotasDBContext.SaveChangesAsync();
               logger.LogInformation($"Se ha realizado la elimnacion del registro con id {DetalleNotaId}");
               return NoContent();
           }
       }

       [HttpPut("{DetalleNotaId}")]
         public async Task<ActionResult> PutDetalleNota(string DetalleNotaId, [FromBody] DetalleNotaDTO ActualizarDetalleNota)
         {
             logger.LogDebug($"Inicio del proceso de modificacion de un DetalleNota con el id {DetalleNotaId}");
             DetalleNota DetalleNota = await this.kalumNotasDBContext.DetalleNotas.FirstOrDefaultAsync(a => a.DetalleNotaId == DetalleNotaId);
             if (DetalleNota ==null)
             {
                 logger.LogInformation($"No existe el DetalleNota con el id {DetalleNotaId}");
                 return NotFound();
                 //o reutnr NoContent() o BadRequest();
             }
             else
             {
               logger.LogDebug("Iniciando el proceso de modificacion del DetalleNota");
               logger.LogDebug($"Realiando la consulta del Detalle de Actividad con el id {ActualizarDetalleNota.DetalleActividadId}");
            DetalleActividad detalleActividad = await this.kalumNotasDBContext.DetalleActividades.FirstOrDefaultAsync(c => c.DetalleActividadId== ActualizarDetalleNota.DetalleActividadId);
            if (detalleActividad == null){
                logger.LogInformation($"No existe el Detalle de Actividad con id { ActualizarDetalleNota.DetalleActividadId}");
                return BadRequest();
            }

            }            
            DetalleNota.DetalleActividadId = ActualizarDetalleNota.DetalleActividadId;
            DetalleNota.ValorNota = ActualizarDetalleNota.ValorNota;
            DetalleNota.Carne = ActualizarDetalleNota.Carne;
            this.kalumNotasDBContext.Entry(DetalleNota).State = EntityState.Modified;
            await this.kalumNotasDBContext.SaveChangesAsync();
            logger.LogInformation("Los datos del DetalleNota fueron actualizados exitosamente");
            return NoContent();

         }
        
    }
}