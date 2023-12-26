using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
    ///IncidentCategory Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class IncidentCategoryController : ControllerBase
    {
        private readonly IUnitOfWork context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public IncidentCategoryController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/incident-Category
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIncidentCategory)]
        public async Task<ResponseDto> CreateIncidentCategory(IncidentCategory incidentCategory)
        {
            try
            {
                if (await IsIncidentCategoryDuplicate(incidentCategory) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.NoMatchFoundError, null);

                incidentCategory.DateCreated = DateTime.Now;
                incidentCategory.IsDeleted = false;

                context.IncidentCategoryRepository.Add(incidentCategory);
                await context.SaveChangesAsync();


                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.SaveMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incident-categorys
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentCategorys)]
        public async Task<ResponseDto> ReadIncidentCategorys()
        {
            try
            {
                var incidentCategoryInDb = await context.IncidentCategoryRepository.GetIncidentCategories();

                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, incidentCategoryInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incident-categorypage
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentCategoryPageByFirstLevel)]
        public async Task<ResponseDto> ReadIncidentCategoryPageByFirstLevel(int start, int take)
        {
            try
            {
                var incidentCategoryInDb = await context.IncidentCategoryRepository.GetIncidentCategoryPageByFirstLevel(start, take);

                incidentCategoryInDb = incidentCategoryInDb.OrderByDescending(i => i.Oid).ToList();

                var response = new
                {
                    IncidentCategories = incidentCategoryInDb,
                    currentPage = start+1,
                    totalRows = await context.IncidentCategoryRepository.GetIncidentCategoryCount(0)
                };
                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, response);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }






        /// <summary>
        /// URl: tuso-api/incident-categorys/key/{key}
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentCategoryPageByLevel)]
        public async Task<ResponseDto> ReadIncidentCategoryPageByLevel(int key, int start, int take)
        {
            try
            {
                var incidentCategoryInDb = await context.IncidentCategoryRepository.GetIncidentCategoryPageByLevel(key, start, take);

                incidentCategoryInDb = incidentCategoryInDb.OrderByDescending(i => i.Oid).ToList();

                var response = new
                {
                    IncidentCategories = incidentCategoryInDb,
                    currentPage = start + 1,
                    totalRows = await context.IncidentCategoryRepository.GetIncidentCategoryCount(key)
                };

                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, response);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL : tuso-api/incident-category/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentCategoryBySingleKey)]
        public async Task<ResponseDto> GetIncidentCategoryBySingleKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.DuplicateError, null);

                var incidentCategoryInDb = await context.IncidentCategoryRepository.GetIncidentCategoryBySingleKey(key);



                return new ResponseDto(HttpStatusCode.OK, true, incidentCategoryInDb== null ? "Data Not Found" : string.Empty, incidentCategoryInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/incident-Category/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="incidentCategory">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIncidentCategory)]
        public async Task<ResponseDto> UpdateIncidentCategory(int key, IncidentCategory incidentCategory)
        {
            try
            {
                if (key != incidentCategory.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsIncidentCategoryDuplicate(incidentCategory) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                incidentCategory.DateModified = DateTime.Now;

                context.IncidentCategoryRepository.Update(incidentCategory);
                await context.SaveChangesAsync();


                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/incident-category/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIncidentCategory)]
        public async Task<ResponseDto> DeleteIncidentCategory(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var incidentCategoryInDb = await context.IncidentCategoryRepository.GetIncidentCategoryBySingleKey(key);
                var isExist = await context.IncidentCategoryRepository.GetIncidentCategoriesByKey(key);

                if (isExist.ToList().Count > 0)
                    return new ResponseDto(HttpStatusCode.MethodNotAllowed, false, MessageConstants.DependencyError, null);

                if (incidentCategoryInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                incidentCategoryInDb.IsDeleted = true;
                incidentCategoryInDb.DateModified = DateTime.Now;

                context.IncidentCategoryRepository.Update(incidentCategoryInDb);
                await context.SaveChangesAsync();
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.DeleteMessage, incidentCategoryInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        ///// <summary>
        ///// URL: tuso-api/incident-category/{key}
        ///// </summary>
        ///// <param name="key">Primary key of the table</param>
        ///// <returns>Deletes a row from the table.</returns>
        //[HttpDelete]
        //[Route(RouteConstants.DeleteIncidentCategory)]
        //public async Task<IActionResult> DeleteIncidentCategory(int key, int start, int take)
        //{
        //    try
        //    {
        //        if (key <= 0)
        //            return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

        //        var incidentCategoryInDb = await context.IncidentCategoryRepository.GetIncidentCategoryByKey(key);
        //        var isExist = await context.IncidentCategoryRepository.GetIncidentCategoriesByPage(key,   start,  take);

        //        if (isExist.ToList().Count > 0)
        //            return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);

        //        if (incidentCategoryInDb == null)
        //            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

        //        incidentCategoryInDb.IsDeleted = true;
        //        incidentCategoryInDb.DateModified = DateTime.Now;

        //        context.IncidentCategoryRepository.Update(incidentCategoryInDb);
        //        await context.SaveChangesAsync();

        //        return Ok(incidentCategoryInDb);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
        //    }
        //}


        /// <summary>
        /// Checks whether the institueCategory is duplicate?
        /// </summary>
        /// <param name="institueCategory">InstitueCategory object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsIncidentCategoryDuplicate(IncidentCategory incidentCategory)
        {
            try
            {
                var incidentCategoryInDb = await context.IncidentCategoryRepository.GetIncidentCategoryByName(incidentCategory.IncidentCategorys, incidentCategory.ParentId);

                if (incidentCategoryInDb != null)

                    if (incidentCategoryInDb.Oid != incidentCategory.Oid)
                        return true;

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}