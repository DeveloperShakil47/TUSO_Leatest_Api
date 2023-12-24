using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.AccessControl;
using TUSO.Domain.Dto;
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
        public async Task<ResponseDto> CreateModule(ModuleDto model)
        {
            try
            {
                Module module = new Module()
                {
                    ModuleName = model.ModuleName,
                    Description = model.Description,
                    DateCreated = DateTime.Now,
                    IsDeleted = false
                };
                if (await IsModuleDuplicate(module))
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);


                context.ModuleRepository.Add(module);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Module Sccessfully Created", null);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateModule", "ModuleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URl: tuso-api/modules-option
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModules)]
        public async Task<ResponseDto> ReadModules()
        {
            try
            {
                var moduleInDb = await context.ModuleRepository.GetModules();

                return new ResponseDto(HttpStatusCode.OK, true, moduleInDb==null ? "Data Not Found" : string.Empty, moduleInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/countrypage
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModulesByPage)]
        public async Task<ResponseDto> ReadModulesByPage(int start, int take)
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

                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadModulesByPage", "ModuleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL : tuso-api/module/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadModuleByKey)]
        public async Task<ResponseDto> ReadModuleByKey(int key)
        {
            try
            {
                if (key <= 0)

                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var moduleInDb = await context.ModuleRepository.GetModuleByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, moduleInDb==null ? "Data Not Found" : string.Empty, moduleInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadModuleByKey", "ModuleController.cs", ex.Message);
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
        public async Task<ResponseDto> UpdateModule(int key, Module module)
        {
            try
            {
                if (key != module.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsModuleDuplicate(module) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);
                module.DateModified = DateTime.Now;
                module.IsDeleted = false;

                context.ModuleRepository.Update(module);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", null);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateModule", "ModuleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL: tuso-api/module/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteModule)]
        public async Task<ResponseDto> DeleteModule(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var moduleInDb = await context.ModuleRepository.GetModuleByKey(key);

                if (moduleInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                moduleInDb.IsDeleted = true;
                moduleInDb.DateModified = DateTime.Now;

                context.ModuleRepository.Update(moduleInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", null);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteModule", "ModuleController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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