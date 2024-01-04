using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Authorization;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

namespace TUSO.Api.Controllers
{
    /// <summary>
    ///ImplementingPartner Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class ImplementingPartnerController : Controller
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ImplementingPartnerController> logger;

        /// <summary>
        /// FundingAgency constructor.
        /// </summary>
        /// <param name="context"></param>
        public ImplementingPartnerController(IUnitOfWork context, ILogger<ImplementingPartnerController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/implementingPartner
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateImplementingPartner)]
        [CustomAuthorization]
        public async Task<ResponseDto> CreateImplementingPartner(ImplementingPartner implementingPartner)
        {
            try
            {
                if (await IsImplementingPartnerDuplicate(implementingPartner) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                implementingPartner.DateCreated = DateTime.Now;
                implementingPartner.IsDeleted = false;

                context.ImplementingPartnerRepository.Add(implementingPartner);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", implementingPartner);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateImplementingPartner", "ImplementingPartnerController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/implementingPartners
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImplementingPartners)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadImplementingPartners()
        {
            try
            {
                var impelementingPartner = await context.ImplementingPartnerRepository.GetImplementingPatrners();

                return new ResponseDto(HttpStatusCode.OK, true, impelementingPartner == null ? "Data Not Found" : "Successfully Get All Data", impelementingPartner);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImplementingPartners", "ImplementingPartnerController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/implementingPartners
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImplementingPartnersPage)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadImplementingPartnersPage(int start, int take)
        {
            try
            {

                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPatrnerByPage(start, take);

                var response = new
                {
                    implementions = implementingPartnerInDb,
                    currentPage = start+1,
                    totaRows = await context.ImplementingPartnerRepository.GetImplementingPatrnersCount()
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImplementingPartnersPage", "ImplementingPartnerController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/implementingPartner/key/{OID}
        /// </summary>
        /// <param name="key">Primary key of the table ImplementingPartner</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImplementingPartnerByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadImplementingPartnerByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPartnerByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, implementingPartnerInDb == null ? "Data Not Found" : "Successfully Get Data by Key", implementingPartnerInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImplementingPartnerByKey", "ImplementingPartnerController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/implementingPartner/system/{key}
        /// </summary>
        /// <param name="key">Primary key of the table System</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImplementingPartnerBySystem)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadImplementingPartnerBySystem(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPartnerBySystem(key);

                return new ResponseDto(HttpStatusCode.OK, true, implementingPartnerInDb == null ? "Data Not Found" : "Successfully Get Data by Key", implementingPartnerInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImplementingPartnerBySystem", "ImplementingPartnerController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/implementingPartner/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="oid">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateImplementingPartner)]
        [CustomAuthorization]
        public async Task<ResponseDto> UpdateImplementingPartner(int key, ImplementingPartner implementingPartner)
        {
            try
            {
                if (key != implementingPartner.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsImplementingPartnerDuplicate(implementingPartner) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                implementingPartner.DateModified = DateTime.Now;
                implementingPartner.IsDeleted = false;

                context.ImplementingPartnerRepository.Update(implementingPartner);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", implementingPartner);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateImplementingPartner", "ImplementingPartnerController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/implementingPartner/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteImplementingPartner)]
        [CustomAuthorization]
        public async Task<ResponseDto> DeleteImplementingPartner(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPartnerByKey(key);

                if (implementingPartnerInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                implementingPartnerInDb.IsDeleted = true;
                implementingPartnerInDb.DateModified = DateTime.Now;

                context.ImplementingPartnerRepository.Update(implementingPartnerInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", implementingPartnerInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteImplementingPartner", "ImplementingPartnerController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the implementingPartner is duplicate?
        /// </summary>
        /// <param name="implementingPartner"></param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsImplementingPartnerDuplicate(ImplementingPartner implementingPartner)
        {
            try
            {
                var userimplementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPartnerByNameAndSystem(implementingPartner.ImplementingPartnerName, implementingPartner.ProjectId);

                if (userimplementingPartnerInDb != null)

                    if (userimplementingPartnerInDb.Oid != implementingPartner.Oid)
                        return true;

                return false;
            }
            catch(Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteImplementingPartner", "IsImplementingPartnerDuplicate.cs", ex.Message);

                throw;
            }
        }
    }
}