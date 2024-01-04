using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Authorization;
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
    ///Country Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork context;

        private readonly ILogger<CountryController> logger;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public CountryController(IUnitOfWork context, ILogger<CountryController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/country
        /// </summary>
        /// <param name="country">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateCountry)]
        [CustomAuthorization]
        public async Task<ResponseDto> CreateCountry(Country country)
        {
            try
            {
                if (await IsCountryDuplicate(country) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                country.DateCreated = DateTime.Now;
                country.IsDeleted = false;

                context.CountryRepository.Add(country);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true , "Data Create Successfully",country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}",DateTime.Now, "BusinessLayer", "CreateCountry", "CountryController.cs", ex.Message);
               
                return new ResponseDto(HttpStatusCode.InternalServerError, false,  MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/countries
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountries)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadCountries()
        {
            try
            {
                var country = await context.CountryRepository.GetCountries();

                return new ResponseDto(HttpStatusCode.OK, true, country == null ? "Data Not Found" : "Successfully Get All Data", country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountries", "CountryController.cs", ex.Message);
               
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError,null);
            }
        }

        /// <summary>
        /// URL: tuso-api/countrypage
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountriesbyPage)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadCountriesbyPage(int start, int take)
        {
            try
            {
                var country = await context.CountryRepository.GetCountrybyPage(start, take);
                var response = new
                {
                    country = country,
                    currentPage = start + 1,
                    totalRows = await context.CountryRepository.GetCountryCount()
                };

                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found":"Successfully Get All Data", response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountriesbyPage", "CountryController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/country/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountryByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadCountryByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var country = await context.CountryRepository.GetCountryByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, country == null ? "Data Not Found" : "Successfully Get Data by Key",country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountryByKey", "CountryController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false,  MessageConstants.GenericError,null);
            }
        }

        /// <summary>
        /// URL: tuso-api/country/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="country">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateCountry)]
        [CustomAuthorization]
        public async Task<ResponseDto> UpdateCountry(int key, Country country)
        {
            try
            {
                if (key != country.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsCountryDuplicate(country) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                country.DateModified = DateTime.Now;
                country.IsDeleted = false;

                context.CountryRepository.Update(country);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateCountry", "CountryController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/country/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCountry)]
        [CustomAuthorization]
        public async Task<ResponseDto> DeleteCountry(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var countryInDb = await context.CountryRepository.GetCountryByKey(key);

                if (countryInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                if (countryInDb.Provinces.Where(w => w.IsDeleted == false).ToList().Count > 0)
                    return new(HttpStatusCode.MethodNotAllowed, false,  MessageConstants.DependencyError, null);

                countryInDb.IsDeleted = true;
                countryInDb.DateModified = DateTime.Now;

                context.CountryRepository.Update(countryInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Delete Successfully", countryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCountry", "CountryController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the country name is duplicate? 
        /// </summary>
        /// <param name="country">Country object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsCountryDuplicate(Country country)
        {
            try
            {
                var countryInDb = await context.CountryRepository.GetCountryByName(country.CountryName);

                if (countryInDb != null)
                {
                    if (countryInDb.Oid != country.Oid)
                        return true;
                }

                return false;
            }
            catch(Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsCountryDuplicate", "CountryController.cs", ex.Message);

                throw;
            }
        }
    }
}