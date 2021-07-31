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
[Route("/kalum-notas/v1/detalleActividades")]
    
    [ApiController]
    public class DetalleActividadesController : ControllerBase
    {

        private readonly KalumNotasDBContext kalumNotasDBContext;

        private readonly ILogger<DetalleActividadesController> logger;

        private readonly IMapper mapper;

        public DetalleActividadesController(KalumNotasDBContext kalumNotasDBContext, ILogger<DetalleActividadesController>  logger,IMapper mapper)
        {
            this.mapper = mapper;
            this.kalumNotasDBContext = kalumNotasDBContext;
            this.logger = logger;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleActividadDTO>>> GetDetalleActividades(){
            
            this.logger.LogDebug("Iniciando proceso de consulta de la informacion de detalleActividades");
            var detalleActividades  = await this.kalumNotasDBContext.DetalleActividades.ToListAsync();
            if (detalleActividades== null || detalleActividades.Count == 0){
                this.logger.LogWarning("No se encontraron registros");
                return new NoContentResult();
            }
            this.logger.LogInformation("Consulta realizada exitosamente");
            return Ok(mapper.Map<List<DetalleActividadDTO>>(detalleActividades));         
        }

          [HttpGet("{DetalleActividadId}", Name = "GetDetalleActividad")]
       public async Task<ActionResult<DetalleActividadDTO>> GetDetalleActividad(string DetalleActividadId){
           this.logger.LogDebug($"Iniciando proceso de consulta de la DetalleActividad con el id = {DetalleActividadId}");
           var detalleActividad  = await this.kalumNotasDBContext.DetalleActividades.Include(d =>d.Seminario).FirstOrDefaultAsync(c => c.DetalleActividadId == DetalleActividadId );
           if (detalleActividad == null)
           {
               logger.LogWarning($"El Seminario con el id {DetalleActividadId} no existe");
               return NotFound();
           }
           else{
               logger.LogInformation("Se ejecuto exitosamente la consulta");
               return Ok(mapper.Map<DetalleActividadDTO>(detalleActividad));
           }
       }

       [HttpPost]
        public async Task<ActionResult<DetalleActividadDTO>> PostDetalleActividad([FromBody] DetalleActividadDTO NuevoDetalleActividad)
        {

           
            if ( NuevoDetalleActividad.FechaEntrega < NuevoDetalleActividad.FechaCreacion){
                logger.LogDebug("Fecha Entrega no puede ser menor a fecha creacion");
                  return BadRequest("Fecha Entrega no puede ser menor a fecha creacion");
            }

            
            if ( NuevoDetalleActividad.FechaPostergacion < NuevoDetalleActividad.FechaEntrega){
                logger.LogDebug("Fecha postergacion no puede ser menor a fecha entrega");
                  return BadRequest("Fecha postergacion no puede ser menor a fecha entrega");
            }

            logger.LogDebug("Iniciando el proceso de un nuevo detalleActividad");
            logger.LogDebug($"Realiando la consulta del Seminario con el id {NuevoDetalleActividad.SeminarioId}");
            Seminario Seminario = await this.kalumNotasDBContext.Seminarios.FirstOrDefaultAsync(c => c.SeminarioId== NuevoDetalleActividad.SeminarioId);
            if (Seminario == null){
                logger.LogInformation($"No existe el Seminario con el id { NuevoDetalleActividad.SeminarioId}");
                return BadRequest();
            }
            NuevoDetalleActividad.DetalleActividadId=Guid.NewGuid().ToString();
            var detalleActividad= mapper.Map<DetalleActividad>(NuevoDetalleActividad);
            await this.kalumNotasDBContext.DetalleActividades.AddAsync(detalleActividad);
            await this.kalumNotasDBContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetDetalleActividad",new {DetalleActividadId = NuevoDetalleActividad.DetalleActividadId}, mapper.Map<DetalleActividadDTO>(detalleActividad));
        }


       [HttpDelete("{DetalleActividadId}")]
       public async Task<ActionResult> DeleteDetalleActividad(String DetalleActividadId){
           logger.LogDebug("Iniciando el proceso de eliminacion del DetalleActividad");
           DetalleActividad DetalleActividad = await this.kalumNotasDBContext.DetalleActividades.FirstOrDefaultAsync(a => a.DetalleActividadId==DetalleActividadId );
           if (DetalleActividad == null)
           {
               logger.LogInformation($"No existe el DetalleActividad con el ID {DetalleActividadId}");
               return NotFound();
           }
           else{
               this.kalumNotasDBContext.DetalleActividades.Remove(DetalleActividad);
               await this.kalumNotasDBContext.SaveChangesAsync();
               logger.LogInformation($"Se ha realizado la eliminacion del registro con id {DetalleActividadId}");
               return NoContent();
           }
       }

       [HttpPut("{DetalleActividadId}")]
         public async Task<ActionResult> PutDetalleActividad(string DetalleActividadId, [FromBody] DetalleActividadDTO ActualizarDetalleActividad)
         {
            
             if ( ActualizarDetalleActividad.FechaEntrega < ActualizarDetalleActividad.FechaCreacion){
                logger.LogDebug("Fecha Entrega no puede ser menor a fecha creacion");
                  return BadRequest("Fecha Entrega no puede ser menor a fecha creacion");
            }

            
            if ( ActualizarDetalleActividad.FechaPostergacion < ActualizarDetalleActividad.FechaEntrega){
                logger.LogDebug("Fecha postergacion no puede ser menor a fecha entrega");
                  return BadRequest("Fecha postergacion no puede ser menor a fecha entrega");
            }
            
             logger.LogDebug($"Inicio del proceso de modificacion de un DetalleActividad con el id {DetalleActividadId}");
             DetalleActividad DetalleActividad = await this.kalumNotasDBContext.DetalleActividades.FirstOrDefaultAsync(a => a.DetalleActividadId == DetalleActividadId);
             if (DetalleActividad ==null)
             {
                 logger.LogInformation($"No existe el DetalleActividad con el id {DetalleActividadId}");
                 return NotFound();
                 //o reutnr NoContent() o BadRequest();
             }
             else
             {
               logger.LogDebug("Iniciando el proceso de modificacion del DetalleActividad");
               logger.LogDebug($"Realiando la consulta del Seminario con el id {ActualizarDetalleActividad.SeminarioId}");
            Seminario Seminario = await this.kalumNotasDBContext.Seminarios.FirstOrDefaultAsync(c => c.SeminarioId== ActualizarDetalleActividad.SeminarioId);
            if (Seminario == null){
                logger.LogInformation($"No existe el seminario con el id { ActualizarDetalleActividad.SeminarioId}");
                return BadRequest();
            }

            }
            DetalleActividad.NombreActividad = ActualizarDetalleActividad.NombreActividad;
            DetalleActividad.SeminarioId = ActualizarDetalleActividad.SeminarioId;
            DetalleActividad.NotaActividad = ActualizarDetalleActividad.NotaActividad;
            DetalleActividad.FechaCreacion = ActualizarDetalleActividad.FechaCreacion;
            DetalleActividad.FechaEntrega = ActualizarDetalleActividad.FechaEntrega;
            DetalleActividad.FechaPostergacion = ActualizarDetalleActividad.FechaPostergacion;
            DetalleActividad.Estado = ActualizarDetalleActividad.Estado;


            this.kalumNotasDBContext.Entry(DetalleActividad).State = EntityState.Modified;
            await this.kalumNotasDBContext.SaveChangesAsync();
            logger.LogInformation("Los datos de DetalleActividad fueron actualizados exitosamente");
            return NoContent();

         }
        
    }
}
