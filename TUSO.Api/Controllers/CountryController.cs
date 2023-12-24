using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateCountry(Country country)
        {
            try
            {
                if (await IsCountryDuplicate(country) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                country.DateCreated = DateTime.Now;
                country.IsDeleted = false;

                context.CountryRepository.Add(country);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadCountryByKey", new { key = country.Oid }, country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}",DateTime.Now, "BusinessLayer", "CreateCountry", "CountryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/countries
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountries)]
        public async Task<IActionResult> ReadCountries()
        {
            try
            {
                var country = await context.CountryRepository.GetCountries();

                return Ok(country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountries", "CountryController.cs", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/countrypage
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountriesbyPage)]
        public async Task<IActionResult> ReadCountriesbyPage(int start, int take)
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountriesbyPage", "CountryController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/country/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadCountryByKey)]
        public async Task<IActionResult> ReadCountryByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var country = await context.CountryRepository.GetCountryByKey(key);

                if (country == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(country);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadCountryByKey", "CountryController.cs", ex.Message);

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
        [Route(RouteConstants.UpdateCountry)]
        public async Task<IActionResult> UpdateCountry(int key, Country country)
        {
            try
            {
                if (key != country.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsCountryDuplicate(country) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                country.DateModified = DateTime.Now;
                country.IsDeleted = false;

                context.CountryRepository.Update(country);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateCountry", "CountryController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/country/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteCountry)]
        public async Task<IActionResult> DeleteCountry(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var countryInDb = await context.CountryRepository.GetCountryByKey(key);

                if (countryInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (countryInDb.Provinces.Where(w => w.IsDeleted == false).ToList().Count > 0)
                    return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);

                countryInDb.IsDeleted = true;
                countryInDb.DateModified = DateTime.Now;

                context.CountryRepository.Update(countryInDb);
                await context.SaveChangesAsync();

                return Ok(countryInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "DeleteCountry", "CountryController.cs", ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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