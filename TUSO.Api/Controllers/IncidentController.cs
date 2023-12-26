
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



        /// <summary>
        /// URl: tuso-api/incidents
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidents)]
        public async Task<ResponseDto> ReadIncidents(int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidents(start, take, status);
                return new ResponseDto(HttpStatusCode.OK, true, string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/key
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByKey)]
        public async Task<IActionResult> ReadIncidentsByKey(long key, long UserAccountID, int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByKey(key, UserAccountID, start, take, status);
                return Ok(incidentInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/status
        /// </summary>
        /// <returns>List of incident status.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByStatus)]
        public async Task<IActionResult> ReadIncidentsByStatus(bool key, int start, int take)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByStatus(key, start, take);
                return Ok(incidentInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/expart
        /// </summary>
        /// <returns>List of Expart incident.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByExpert)]
        public async Task<IActionResult> GetIncidentsByExpart(long key, int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByExpart(key, start, take, status);

                return Ok(incidentInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


    }
}
