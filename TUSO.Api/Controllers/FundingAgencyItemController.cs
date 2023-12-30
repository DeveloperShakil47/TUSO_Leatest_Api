using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

namespace TUSO.Api.Controllers
{
    /// <summary>
    ///FundingAgencyItem Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class FundingAgencyItemController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<FundingAgencyItemController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context"></param>
        public FundingAgencyItemController(IUnitOfWork context, ILogger<FundingAgencyItemController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URl: tuso-api/provinces
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentFundingAgencies)]
        public async Task<ResponseDto> ReadIncidentFundingAgencies()
        {
            try
            {
                var fundingItemInDb = await context.FundingAgencyItemRepository.GetFundingAgencyItems();

                return new ResponseDto(HttpStatusCode.OK, true, fundingItemInDb == null ? "Data Not Found" : "Successfully Get All Data", fundingItemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencyItems", "FundingAgencyItemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentFundingAgencyByKey)]
        public async Task<ResponseDto> ReadIncidentFundingAgencyByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var fundingItemInDb = await context.FundingAgencyItemRepository.GetFundingAgencyItemByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, fundingItemInDb == null ? "Data Not Found" : "Successfully Get Data by Key", fundingItemInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencyItemsByKey", "FundingAgencyItemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/district/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentFundingAgencyByIncident)]
        public async Task<ResponseDto> ReadIncidentFundingAgencyByIncident(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var fundingItemInDb = await context.FundingAgencyItemRepository.GetFundingAgencyItemByIncident(key);

                return new ResponseDto(HttpStatusCode.OK, true, fundingItemInDb == null ? "Data Not Found" : "Successfully Get All Data", fundingItemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadFundingAgencyItemByIncident", "FundingAgencyItemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}