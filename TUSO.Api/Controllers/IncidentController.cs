
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

namespace TUSO.Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly IConfiguration config;
        private readonly ILogger<IncidentController> logger;

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="context"></param>
        public IncidentController(IUnitOfWork context, IConfiguration config, ILogger<IncidentController> logger)
        {
            this.context = context;
            this.config = config;
            this.logger = logger;
        }

        /// <summary>
        /// URl: tuso-api/incident
        /// </summary>
        /// <param name="incident">Object to be saved in the table as a row.</param>
        /// <returns>Saved object</returns>
        [HttpPost]
        [Route(RouteConstants.CreateIncident)]
        public async Task<ResponseDto> CreateIncident(Incident incident)

        {
            try
            {
                incident.DateCreated = DateTime.Now;
                incident.IsDeleted = false;
                incident.DateReported= DateTime.Now;
                context.IncidentRepository.Add(incident);
                await context.SaveChangesAsync();
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.SaveMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

    }
}
