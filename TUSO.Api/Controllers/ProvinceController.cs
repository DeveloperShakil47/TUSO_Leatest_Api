using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 20.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///Province Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ProvinceController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context"></param>
        public ProvinceController(IUnitOfWork context, ILogger<ProvinceController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/province
        /// </summary>
        /// <param name="entity">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateProvince)]
        public async Task<ResponseDto> CreateProvince(Province province)
        {
            try
            {
                if (await IsProvinceDuplicate(province) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                province.DateCreated = DateTime.Now;
                province.IsDeleted = false;

                context.ProvinceRepository.Add(province);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", province);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateProvince", "ProvinceController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/provinces
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinces)]
        public async Task<ResponseDto> ReadProvinces()
        {
            try
            {
                var province = await context.ProvinceRepository.GetProvinces();

                return new ResponseDto(HttpStatusCode.OK, true, province == null ? "Data Not Found" : "Successfully Get All Data", province);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadProvinces", "ProvinceController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinceByKey)]
        public async Task<ResponseDto> ReadProvinceByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var provinceInDb = await context.ProvinceRepository.GetProvinceByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, provinceInDb == null ? "Data Not Found" : "Successfully Get Data by Key", provinceInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadProvinceByKey", "ProvinceController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinceByCountry)]
        public async Task<ResponseDto> ReadProvinceByCountries(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var provinceInDb = await context.ProvinceRepository.GetProvinceByCountry(key);

                return new ResponseDto(HttpStatusCode.OK, true, provinceInDb == null ? "Data Not Found" : "Successfully Get Data by Key", provinceInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadProvinceByCountries", "ProvinceController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);

            }
        }


        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadProvinceByCountryPage)]
        public async Task<ResponseDto> ReadProvinceByCountryPage(int key, int start, int take)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var provinceInDb = await context.ProvinceRepository.GetProvincesByCountry(key, start, take);

                var response = new
                {
                    province = provinceInDb,
                    currentPage = start + 1,
                    totalRows = await context.ProvinceRepository.GetProvinceCount(key)
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadProvinceByCountryPage", "ProvinceController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the talbe</param>
        /// <param name="province">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateProvince)]
        public async Task<ResponseDto> UpdateProvince(int key, Province province)
        {
            try
            {
                if (key != province.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsProvinceDuplicate(province) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                province.DateModified = DateTime.Now;
                province.IsDeleted = false;

                context.ProvinceRepository.Update(province);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", province);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateProvince", "ProvinceController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteProvince)]
        public async Task<ResponseDto> DeleteProvince(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var provinceInDb = await context.ProvinceRepository.GetProvinceByKey(key);

                if (provinceInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                if (provinceInDb.Districts.Where(w => w.IsDeleted == false).ToList().Count > 0)
                    return new(HttpStatusCode.MethodNotAllowed, false, MessageConstants.DependencyError, null);

                provinceInDb.IsDeleted = true;
                provinceInDb.DateModified = DateTime.Now;

                context.ProvinceRepository.Update(provinceInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", provinceInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteProvince", "ProvinceController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the province is duplicate?
        /// </summary>
        /// <param name="province">Province object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsProvinceDuplicate(Province province)
        {
            try
            {
                var provinceInDb = await context.ProvinceRepository.GetProvinceByNameAndCountry(province.ProvinceName, province.CountryId);

                if (provinceInDb != null)
                    if (provinceInDb.CountryId != province.CountryId)
                        return true;

                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsProvinceDuplicate", "ProvinceController.cs", ex.Message);

                throw;
            }
        }
    }
}