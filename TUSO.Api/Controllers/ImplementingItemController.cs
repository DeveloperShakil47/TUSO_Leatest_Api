using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Authorization;
using TUSO.Domain.Dto;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

namespace TUSO.Api.Controllers
{
    /// <summary>
    ///ImplementingItem Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class ImplementingItemController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<ImplementingItemController> logger;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="context"></param>
        public ImplementingItemController(IUnitOfWork context, ILogger<ImplementingItemController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URl: tuso-api/provinces
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentImplementingPartners)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadIncidentImplementingPartners()
        {
            try
            {
                var implementingItemInDb = await context.ImplementingItemRepository.GetImplemenentingItems();

                return new ResponseDto(HttpStatusCode.OK, true, implementingItemInDb == null ? "Data Not Found" : "Successfully Get All Data", implementingItemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImplementingItems", "ImplementingItemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL : tuso-api/province/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentImplementingPartnerByKey)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadIncidentImplementingPartnerByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var implementingItemInDb = await context.ImplementingItemRepository.GetImplemenentingItemByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, implementingItemInDb == null ? "Data Not Found" : "Successfully Get Data by Key", implementingItemInDb);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImplementingItemByKey", "ImplementingItemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/district/province/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Countries</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentImplementingPartnerByIncident)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadIncidentImplementingPartnerByIncident(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var implementingItemInDb = await context.ImplementingItemRepository.GetImplemenentingItemByIncident(key);

                return new ResponseDto(HttpStatusCode.OK, true, implementingItemInDb == null ? "Data Not Found" : "Successfully Get All Data", implementingItemInDb);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadImplementingItemByIncident", "ImplementingItemController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}