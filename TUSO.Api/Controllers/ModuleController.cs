using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
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
    ///Module Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ModuleController> logger;

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="context"></param>
        public ModuleController(IUnitOfWork context, ILogger<ModuleController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        ///<summary>
        ///URL: tuso-api/module-option
        ///</summary>
        ///
        [HttpPost]
        [Route(RouteConstants.CreateModule)]
        public async Task<IActionResult> CreateModule(Module module)
        {
            try
            {
                if (await IsModuleDuplicate(module))
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                module.DateCreated = DateTime.Now;
                module.IsDeleted = false;

                context.ModuleRepository.Add(module);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadModuleByKey", new { key = module.Oid }, module);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateModule", "ModuleController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
       

        /// <summary>
        /// URl: tuso-api/modules-option
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModules)]
        public async Task<IActionResult> ReadModules()
        {
            try
            {
                var moduleInDb = await context.ModuleRepository.GetModules();
                return Ok(moduleInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/countrypage
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModulesByPage)]
        public async Task<IActionResult> ReadModulesByPage(int start, int take)
        {
            try
            {
                var module = await context.ModuleRepository.GetModulebyPage(start, take);
                var response = new
                {
                    module = module,
                    currentPage = start + 1,
                    totalRows = await context.ModuleRepository.GetModuleCount()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadModulesByPage", "ModuleController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL : tuso-api/module/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModuleByKey)]
        public async Task<IActionResult> ReadModuleByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var moduleInDb = await context.ModuleRepository.GetModuleByKey(key);

                if (moduleInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(moduleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadModuleByKey", "ModuleController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/country/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="country">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateModule)]
        public async Task<IActionResult> UpdateModule(int key, Module module)
        {
            try
            {
                if (key != module.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsModuleDuplicate(module) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);
                module.DateModified = DateTime.Now;
                module.IsDeleted = false;

                context.ModuleRepository.Update(module);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);

            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateModule", "ModuleController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: tuso-api/module/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteModule)]
        public async Task<IActionResult> DeleteModule(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var moduleInDb = await context.ModuleRepository.GetModuleByKey(key);

                if (moduleInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                moduleInDb.IsDeleted = true;
                moduleInDb.DateModified = DateTime.Now;

                context.ModuleRepository.Update(moduleInDb);
                await context.SaveChangesAsync();

                return Ok(moduleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteModule", "ModuleController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the module is duplicate?
        /// </summary>
        /// <param name="module">Module object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsModuleDuplicate(Module module)
        {
            try
            {
                var moduleInDb = await context.ModuleRepository.GetModuleByName(module.ModuleName);

                if (moduleInDb != null)
                    if (moduleInDb.Oid != module.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsModuleDuplicate", "ModuleController.cs", ex.Message);

                throw;
            }
        }
    }
}