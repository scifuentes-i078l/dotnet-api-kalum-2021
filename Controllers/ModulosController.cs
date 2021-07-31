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
    [Route("/kalum-notas/v1/modulos")]
    
    [ApiController]
    public class ModulosController : ControllerBase
    {

        private readonly KalumNotasDBContext kalumNotasDBContext;

        private readonly ILogger<ModulosController> logger;

        private readonly IMapper mapper;

        public ModulosController(KalumNotasDBContext kalumNotasDBContext, ILogger<ModulosController>  logger, IMapper mapper)
        {
            this.mapper = mapper;
            this.kalumNotasDBContext = kalumNotasDBContext;
            this.logger = logger;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuloDTO>>> GetModuloes(){
            
            this.logger.LogDebug("Iniciando proceso de consulta de la informacion de modulos");
            var modulos  = await this.kalumNotasDBContext.Modulos.ToListAsync();
            if (modulos== null || modulos.Count == 0){
                this.logger.LogWarning("No se encontraron registros");
                return new NoContentResult();
            }

            this.logger.LogInformation("Consulta realizada exitosamente");
            return Ok(mapper.Map<List<ModuloDTO>>(modulos));         
        }

          [HttpGet("{moduloId}", Name = "GetModulo")]
       public async Task<ActionResult<ModuloDTO>> GetModulo(string moduloId){
           this.logger.LogDebug($"Iniciando proceso de consulta de la Modulo con el id = {moduloId}");
           var modulo  = await this.kalumNotasDBContext.Modulos.Include(d =>d.CarreraTecnica).FirstOrDefaultAsync(c => c.ModuloID == moduloId );
           if (modulo == null)
           {
               logger.LogWarning($"El modulo con el id {moduloId} no existe");
               return NotFound();
           }
           else{
               logger.LogInformation("Se ejecuto exitosamente la consulta");
               return Ok(mapper.Map<ModuloDTO>(modulo));
           }
       }

       [HttpPost]
        public async Task<ActionResult<ModuloDTO>> PostModulo([FromBody] ModuloDTO NuevoModulo)
        {
            logger.LogDebug("Iniciando el proceso de un nuevo modulo");
            logger.LogDebug($"Realiando la consulta de la carrera tecnica con el id {NuevoModulo.CarreraID}");
            CarreraTecnica carreraTecnica = await this.kalumNotasDBContext.Carreras.FirstOrDefaultAsync(c => c.CarreraId== NuevoModulo.CarreraID);
            if (carreraTecnica == null){
                logger.LogInformation($"No existe la carrera tecnica con el id { NuevoModulo.CarreraID}");
                return BadRequest();
            }
            NuevoModulo.ModuloID=Guid.NewGuid().ToString();
            var modulo = mapper.Map<Modulo>(NuevoModulo);
            await this.kalumNotasDBContext.Modulos.AddAsync(modulo);
            await this.kalumNotasDBContext.SaveChangesAsync();
            return new CreatedAtRouteResult("GetModulo",new {ModuloId = NuevoModulo.ModuloID},mapper.Map<ModuloDTO>(modulo));
        }


       [HttpDelete("{moduloId}")]
       public async Task<ActionResult> DeleteModulo(String moduloId){
           logger.LogDebug("Iniciando el proceso de eliminacion del Modulo");
           Modulo Modulo = await this.kalumNotasDBContext.Modulos.FirstOrDefaultAsync(a => a.ModuloID==moduloId );
           if (Modulo == null)
           {
               logger.LogInformation($"No existe el Modulo con el ID {moduloId}");
               return NotFound();
           }
           else{
               this.kalumNotasDBContext.Modulos.Remove(Modulo);
               await this.kalumNotasDBContext.SaveChangesAsync();
               logger.LogInformation($"Se ha realizado la elimnacion del registro con id {moduloId}");
               return NoContent();
           }
       }

       [HttpPut("{ModuloID}")]
         public async Task<ActionResult> PutModulo(string ModuloID, [FromBody] ModuloDTO ActualizarModulo)
         {
             logger.LogDebug($"Inicio del proceso de modificacion de un Modulo con el id {ModuloID}");
             Modulo Modulo = await this.kalumNotasDBContext.Modulos.FirstOrDefaultAsync(a => a.ModuloID == ModuloID);
             if (Modulo ==null)
             {
                 logger.LogInformation($"No existe el Modulo con el id {ModuloID}");
                 return NotFound();
                 //o reutnr NoContent() o BadRequest();
             }
             else
             {
               logger.LogDebug("Iniciando el proceso de modificacion del Modulo");
               logger.LogDebug($"Realiando la consulta de la carrera tecnica con el id {ActualizarModulo.CarreraID}");
            CarreraTecnica carreraTecnica = await this.kalumNotasDBContext.Carreras.FirstOrDefaultAsync(c => c.CarreraId== ActualizarModulo.CarreraID);
            if (carreraTecnica == null){
                logger.LogInformation($"No existe la carrera tecnica con el id { ActualizarModulo.CarreraID}");
                return BadRequest();
            }

            }
            Modulo.NombreModulo = ActualizarModulo.NombreModulo;
            Modulo.CarreraID = ActualizarModulo.CarreraID;
            Modulo.NumeroSeminarios = ActualizarModulo.NumeroSeminarios;
            this.kalumNotasDBContext.Entry(Modulo).State = EntityState.Modified;
            await this.kalumNotasDBContext.SaveChangesAsync();
            logger.LogInformation("Los datos del Modulo fueron actualizados exitosamente");
            return NoContent();

         }
        
    }
}