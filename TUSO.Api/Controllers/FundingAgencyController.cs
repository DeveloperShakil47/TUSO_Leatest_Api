using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateFundingAgency(FundingAgency fundingAgency)
        {
            try
            {
                if (await IsFundingAgencyDuplicate(fundingAgency) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                fundingAgency.DateCreated = DateTime.Now;
                fundingAgency.IsDeleted = false;

                context.FundingAgencyRepository.Add(fundingAgency);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFundingAgencyByKey", new { key = fundingAgency.Oid }, fundingAgency);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateFundingAgency", "FundingAgencyController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: tuso-api/agencies
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgencies)]
        public async Task<IActionResult> ReadFundingAgencies()
        {
            try
            {
                var fundingAgencies = await context.FundingAgencyRepository.GetFindingAgencies();

                return Ok(fundingAgencies);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencies", "FundingAgencyController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
           
        }

        /// <summary>
        /// URl: tuso-api/agencies
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgenciesPage)]
        public async Task<IActionResult> ReadFundingAgencies(int start, int take)
        {
            try
            {
                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFindingAgenciesByPage(start, take);

                var response = new
                {
                    fundingAgency = fundingAgencyInDb,
                    currentPage = start+1,
                    totalRows = await context.FundingAgencyRepository.GetFindingAgenciesCount()
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencies", "FundingAgencyController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/fundingAgency/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table FundingAgency</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgencyByKey)]
        public async Task<IActionResult> ReadFundingAgencyByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFundingAgencyByKey(key);

                if (fundingAgencyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(fundingAgencyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencyByKey", "FundingAgencyController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/fundingAgency/sysyem/{key}
        /// </summary>
        /// <param name="key">Primary key of the table System</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFundingAgencyBySystem)]
        public async Task<IActionResult> ReadFundingAgencyBySystem(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFundingAgencyBySystem(key);

                if (fundingAgencyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(fundingAgencyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencyBySystem", "FundingAgencyController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateFundingAgency(int key, FundingAgency fundingAgency)
        {
            try
            {
                if (key != fundingAgency.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsFundingAgencyDuplicate(fundingAgency) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                fundingAgency.DateModified = DateTime.Now;
                fundingAgency.IsDeleted = false;

                context.FundingAgencyRepository.Update(fundingAgency);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateFundingAgency", "FundingAgencyController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/fundingAgency/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFundingAgency)]
        public async Task<IActionResult> DeleteFundingAgency(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var fundingAgencyInDb = await context.FundingAgencyRepository.GetFundingAgencyByKey(key);

                if (fundingAgencyInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                fundingAgencyInDb.IsDeleted = true;
                fundingAgencyInDb.DateModified = DateTime.Now;

                context.FundingAgencyRepository.Update(fundingAgencyInDb);
                await context.SaveChangesAsync();

                return Ok(fundingAgencyInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteFundingAgency", "FundingAgencyController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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