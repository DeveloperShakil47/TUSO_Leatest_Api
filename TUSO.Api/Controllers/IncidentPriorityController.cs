using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///IncidentPriority Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class IncidentPriorityController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<IncidentPriorityController> logger;
        public IncidentPriorityController(IUnitOfWork context, ILogger<IncidentPriorityController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/incident-priority
        /// </summary>
        /// <param name="incidentPriority">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIncidentPriority)]
        public async Task<IActionResult> CreateIncidentPriority(IncidentPriority incidentPriority)
        {
            try
            {
                if (await IsIncidentPriorityDuplicate(incidentPriority) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                incidentPriority.DateCreated = DateTime.Now;
                incidentPriority.IsDeleted = false;

                context.IncidentPriorityRepository.Add(incidentPriority);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadIncidentPriorityByKey", new { key = incidentPriority.Oid }, incidentPriority);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateIncidentPriority", "IncidentPriorityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/incident-priorities
        /// </summary>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentPriorities)]
        public async Task<IActionResult> ReadIncidentPriorities()
        {
            try
            {
                var incidentPriorityInDb = await context.IncidentPriorityRepository.GetIncidentPriorities();

                return Ok(incidentPriorityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIncidentPriorities", "IncidentPriorityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/incident-priorities
        /// </summary>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentPrioritiesPage)]
        public async Task<IActionResult> ReadIncidentPriorities(int start, int take)
        {
            try
            {
                var incidentPriorityInDb = await context.IncidentPriorityRepository.GetIncidentPriorities(start, take);

                var response = new
                {
                    data = incidentPriorityInDb,
                    currentPage = start + 1,
                    totalRows = await context.IncidentPriorityRepository.GetIncidentPriorityCount()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIncidentPriorities", "IncidentPriorityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/incident-priority/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table IncidentPriority</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentPriorityByKey)]
        public async Task<IActionResult> ReadIncidentPriorityByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var incidentPriorityInDb = await context.IncidentPriorityRepository.GetIncidentPriorityByKey(key);

                if (incidentPriorityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(incidentPriorityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadIncidentPriorityByKey", "IncidentPriorityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/incident-priority/{key}
        /// </summary>
        /// <param name="key">primary key of the table</param>
        /// <param name="incidentPriority">Object to be update</param>
        /// <returns>Update row in the table</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIncidentPriority)]
        public async Task<IActionResult> UpdateIncidentPriority(int key, IncidentPriority incidentPriority)
        {
            try
            {
                if (key != incidentPriority.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsIncidentPriorityDuplicate(incidentPriority) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                incidentPriority.DateModified = DateTime.Now;
                incidentPriority.IsDeleted = false;

                context.IncidentPriorityRepository.Update(incidentPriority);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateIncidentPriority", "IncidentPriorityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/incident-priority/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIncidentPriority)]
        public async Task<IActionResult> DeleteIncidentPriority(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var incidentPriorityInDb = await context.IncidentPriorityRepository.GetIncidentPriorityByKey(key);

                if (incidentPriorityInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                incidentPriorityInDb.IsDeleted = true;
                incidentPriorityInDb.DateModified = DateTime.Now;

                context.IncidentPriorityRepository.Update(incidentPriorityInDb);
                await context.SaveChangesAsync();

                return Ok(incidentPriorityInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteIncidentPriority", "IncidentPriorityController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the incidentPriority is duplicate?
        /// </summary>
        /// <param name="incidentPriority">IncidentPriority object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsIncidentPriorityDuplicate(IncidentPriority incidentPriority)
        {
            try
            {
                var incidentPriorityInDb = await context.IncidentPriorityRepository.GetIncidentPriorityByName(incidentPriority.Priority);

                if (incidentPriorityInDb != null)
                    if (incidentPriorityInDb.Oid != incidentPriority.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsIncidentPriorityDuplicate", "IncidentPriorityController.cs", ex.Message);

                throw;
            }
        }
    }
}