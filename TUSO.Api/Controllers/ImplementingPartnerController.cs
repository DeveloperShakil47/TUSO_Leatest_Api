using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// FundingAgency constructor.
        /// </summary>
        /// <param name="context"></param>
        public ImplementingPartnerController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/implementingPartner
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateImplementingPartner)]
        public async Task<IActionResult> CreateImplementingPartner(ImplementingPartner implementingPartner)
        {
            try
            {
                if (await IsImplementingPartnerDuplicate(implementingPartner) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                implementingPartner.DateCreated = DateTime.Now;
                implementingPartner.IsDeleted = false;

                context.ImplementingPartnerRepository.Add(implementingPartner);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadImplementingPartnerByKey", new { key = implementingPartner.Oid }, implementingPartner);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: tuso-api/implementingPartners
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImplementingPartners)]
        public async Task<IActionResult> ReadImplementingPartners(int start, int take, bool isDropdown)
        {
            try
            {
                if (isDropdown)
                {
                   return Ok(await context.ImplementingPartnerRepository.GetImplementingPatrners());
                }
                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPatrners(start, take);
                var response = new
                {
                    implementions = implementingPartnerInDb,
                    currentPage = start+1,
                    totaRows = await context.ImplementingPartnerRepository.GetImplementingPatrnersCount()
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/implementingPartner/key/{OID}
        /// </summary>
        /// <param name="key">Primary key of the table ImplementingPartner</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImplementingPartnerByKey)]
        public async Task<IActionResult> ReadImplementingPartnerByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPartnerByKey(key);

                if (implementingPartnerInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(implementingPartnerInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/implementingPartner/system/{key}
        /// </summary>
        /// <param name="key">Primary key of the table System</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadImplementingPartnerBySystem)]
        public async Task<IActionResult> ReadImplementingPartnerBySystem(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPartnerBySystem(key);

                if (implementingPartnerInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(implementingPartnerInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateImplementingPartner(int key, ImplementingPartner implementingPartner)
        {
            try
            {
                if (key != implementingPartner.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsImplementingPartnerDuplicate(implementingPartner) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                implementingPartner.DateModified = DateTime.Now;
                implementingPartner.IsDeleted = false;

                context.ImplementingPartnerRepository.Update(implementingPartner);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/implementingPartner/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteImplementingPartner)]
        public async Task<IActionResult> DeleteImplementingPartner(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var implementingPartnerInDb = await context.ImplementingPartnerRepository.GetImplementingPartnerByKey(key);

                if (implementingPartnerInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                implementingPartnerInDb.IsDeleted = true;
                implementingPartnerInDb.DateModified = DateTime.Now;

                context.ImplementingPartnerRepository.Update(implementingPartnerInDb);
                await context.SaveChangesAsync();

                return Ok(implementingPartnerInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
            catch
            {
                throw;
            }
        }
    }
}