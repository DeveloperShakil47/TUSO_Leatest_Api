using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

namespace TUSO.Api.Controllers
{
    /// <summary>
    ///FundingAgency Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class FundingAgencyController : Controller
    {
        private readonly IUnitOfWork context;

        private readonly ILogger<FundingAgencyController> logger;
        /// <summary>
        /// FundingAgency constructor.
        /// </summary>
        /// <param name="context"></param>
        public FundingAgencyController(IUnitOfWork context, ILogger<FundingAgencyController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/fundingAgency
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFundingAgency)]
        public async Task<ResponseDto> CreateFundingAgency(FundingAgency fundingAgency)
        {
            try
            {
                if (await IsFundingAgencyDuplicate(fundingAgency) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                fundingAgency.DateCreated = DateTime.Now;
                fundingAgency.IsDeleted = false;

                context.FundingAgencyRepository.Add(fundingAgency);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", fundingAgency);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateFundingAgency", "FundingAgencyController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/agencies
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgencies)]
        public async Task<ResponseDto> ReadFundingAgencies()
        {
            try
            {
                var fundingAgencies = await context.FundingAgencyRepository.GetFindingAgencies();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", fundingAgencies);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencies", "FundingAgencyController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }

        }

        /// <summary>
        /// URl: tuso-api/agencies
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgenciesPage)]
        public async Task<ResponseDto> ReadFundingAgencies(int start, int take)
        {
            try
            {
                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFindingAgenciesByPage(start, take);

                var response = new
                {
                    fundingAgency = fundingAgencyInDb,
                    currentPage = start + 1,
                    totalRows = await context.FundingAgencyRepository.GetFindingAgenciesCount()
                };
                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencies", "FundingAgencyController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/fundingAgency/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgency</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgencyByKey)]
        public async Task<ResponseDto> ReadFundingAgencyByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFundingAgencyByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, fundingAgencyInDb == null ? "Data Not Found" : "Successfully Get Data by Key", fundingAgencyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencyByKey", "FundingAgencyController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/fundingAgency/sysyem/{key}
        /// </summary>
        /// <param name="key">Primary key of the table System</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgencyBySystem)]
        public async Task<ResponseDto> ReadFundingAgencyBySystem(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFundingAgencyBySystem(key);

                if (fundingAgencyInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                return new ResponseDto(HttpStatusCode.OK, true, "Successfully Get Data by Key", fundingAgencyInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencyBySystem", "FundingAgencyController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/fundingAgency/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="fundingAgency">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFundingAgency)]
        public async Task<ResponseDto> UpdateFundingAgency(int key, FundingAgency fundingAgency)
        {
            try
            {
                if (key != fundingAgency.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsFundingAgencyDuplicate(fundingAgency) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                fundingAgency.DateModified = DateTime.Now;
                fundingAgency.IsDeleted = false;

                context.FundingAgencyRepository.Update(fundingAgency);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", fundingAgency);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateFundingAgency", "FundingAgencyController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/fundingAgency/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFundingAgency)]
        public async Task<ResponseDto> DeleteFundingAgency(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFundingAgencyByKey(key);

                if (fundingAgencyInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                fundingAgencyInDb.IsDeleted = true;
                fundingAgencyInDb.DateModified = DateTime.Now;

                context.FundingAgencyRepository.Update(fundingAgencyInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", fundingAgencyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFundingAgency", "FundingAgencyController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the fundingAgency is duplicate?
        /// </summary>
        /// <param name="fundingAgency"></param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsFundingAgencyDuplicate(FundingAgency fundingAgency)
        {
            try
            {
                var userfundingAgencyInDb = await context.FundingAgencyRepository.GetFundingAgencyByNameAndSystem(fundingAgency.FundingAgencyName, fundingAgency.ProjectId);

                if (userfundingAgencyInDb != null)

                    if (userfundingAgencyInDb.Oid != fundingAgency.Oid)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsFundingAgencyDuplicate", "FundingAgencyController.cs", ex.Message);

                throw;
            }
        }
    }
}