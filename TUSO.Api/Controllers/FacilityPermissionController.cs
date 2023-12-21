using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan 
 * Date created: 17.09.2022
 * Last modified: 17.09.2022
 * Modified by: Stephan
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///IncidentPriority Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class FacilityPermissionController : ControllerBase
    {
        private readonly IUnitOfWork context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public FacilityPermissionController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/facility-permission
        /// </summary>
        /// <param name="facilityPermission">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateFacilityPermission)]
        public async Task<IActionResult> CreateIncidentPriority(FacilityPermission facilityPermission)
        {
            try
            {
                if (await context.FacilityPermissionRepository.IsDuplicatePermission(facilityPermission.FacilityId, facilityPermission.UserId) is not null)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                facilityPermission.DateCreated = DateTime.Now;
                facilityPermission.IsDeleted = false;

                context.FacilityPermissionRepository.Add(facilityPermission);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadFacilityPermissionsByKey", new { key = facilityPermission.Oid }, facilityPermission);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility-permission/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table row.</param>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityPermissionsByKey)]
        public async Task<IActionResult> ReadFacilityPermissionsByKey(int key)
        {
            try
            {
                var facilityPermission = await context.FacilityPermissionRepository.GetFacilityPermissionByKey(key);

                return Ok(facilityPermission);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL : tuso-api/facility-users/key/{facilityID}
        /// </summary>
        /// <param name="FacilityID">Primary key of the Facility.</param>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityPermission)]
        public async Task<IActionResult> ReadFacilityUsers(int FacilityId)
        {
            try
            {
                var facilityPermission = await context.FacilityPermissionRepository.GetFacilityUserByKey(FacilityId);

                return Ok(facilityPermission);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility-users/key/{facilityID}
        /// </summary>
        /// <param name="FacilityID">Primary key of the Facility.</param>
        /// <returns>List of table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilitiePermissionPage)]
        public async Task<IActionResult> ReadFacilitiesUsers(int FacilityId, int start, int take)
        {
            try
            {
                var facilityPermission = await context.FacilityPermissionRepository.GetFacilitiesUserByKey(FacilityId, start, take);
                var response = new
                {
                    data = facilityPermission,
                    currentPage = start+1,
                    totalRows = await context.FacilityPermissionRepository.GetTotalRows(FacilityId)
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL : tuso-api/facility-permissions
        /// </summary>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadFacilityPermissions)]
        public async Task<IActionResult> ReadFacilityPermissions()
        {
            try
            {
                List<FacilitiesPermissionDto> facilitiesPermissionDtos = new List<FacilitiesPermissionDto>();

                var facilityPermissions = await context.FacilityPermissionRepository.GetFacilityPermissions();

                foreach (var facility in facilityPermissions)
                {
                    facilitiesPermissionDtos.Add(new FacilitiesPermissionDto()
                    {
                        OID = facility.Oid,
                        UserID = facility.UserId,
                        FacilityID = facility.FacilityId,
                        CreatedDate = facility.DateCreated?.ToString("dd/MMM/yyyy"),
                        ModifiedDate = facility.DateModified?.ToString("dd/MMM/yyyy"),
                        FacilityName = facility.Facility.FacilityName,
                        UserName = facility.UserAccount.Username

                    });
                }

                if (facilitiesPermissionDtos == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(facilitiesPermissionDtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/facility-permission/{key}
        /// </summary>
        /// <param name="key">primary key of the table</param>
        /// <param name="facilityPermission">Object to be update</param>
        /// <returns>Update row in the table</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateFacilityPermissions)]
        public async Task<IActionResult> UpdateIncidentPriority(int key, FacilityPermission facilityPermission)
        {
            try
            {
                if (key != facilityPermission.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await context.FacilityPermissionRepository.IsDuplicatePermission(facilityPermission.FacilityId, facilityPermission.UserId) is not null)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                facilityPermission.DateModified = DateTime.Now;
                facilityPermission.IsDeleted = false;

                context.FacilityPermissionRepository.Update(facilityPermission);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/facility-permission/delete/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteFacilityPermission)]
        public async Task<IActionResult> DeleteFacilityPermission(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var facilityPermission = await context.FacilityPermissionRepository.GetFacilityPermissionByKey(key);

                if (facilityPermission == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                facilityPermission.IsDeleted = true;
                facilityPermission.DateModified = DateTime.Now;

                context.FacilityPermissionRepository.Update(facilityPermission);
                await context.SaveChangesAsync();

                return Ok(facilityPermission);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }
    }
}