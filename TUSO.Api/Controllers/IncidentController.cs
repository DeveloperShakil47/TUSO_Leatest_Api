using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using System.Text;
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
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class IncidentController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly IConfiguration config;
        private readonly ILogger<IncidentController> logger;
        private readonly EmailConfigurationController _emailConfigController;
        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="context"></param>
        public IncidentController(IUnitOfWork context, IConfiguration config, ILogger<IncidentController> logger, EmailConfigurationController emailConfigController)
        {
            this.context = context;
            this.config = config;
            this.logger = logger;
            _emailConfigController = emailConfigController;
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
                incident.DateReported = DateTime.Now;

                context.IncidentRepository.Add(incident);
                await context.SaveChangesAsync();

                if (incident.FundingAgencyList != null)
                {
                    foreach (var item in incident.FundingAgencyList)
                    {
                        IncidendtFundingAgency  fundingAgencyItem = new IncidendtFundingAgency();

                        fundingAgencyItem.FundingAgencyId = item;
                        fundingAgencyItem.IncidentId = incident.Oid;
                        fundingAgencyItem.DateCreated = DateTime.Now;
                        fundingAgencyItem.IsDeleted = false;
                        context.FundingAgencyItemRepository.Add(fundingAgencyItem);
                        await context.SaveChangesAsync();
                    }
                }

                if (incident.ImplementingList != null)
                {
                    foreach (var item in incident.ImplementingList)
                    {
                        IncidentImplemenentingPartner implemenentingItem  = new IncidentImplemenentingPartner();

                        implemenentingItem.ImplementingId = item;
                        implemenentingItem.IncidentId = incident.Oid;
                        implemenentingItem.DateCreated = DateTime.Now;
                        implemenentingItem.IsDeleted = false;
                        context.ImplementingItemRepository.Add(implemenentingItem);
                        await context.SaveChangesAsync();
                    }
                }

                var incidentDb = await context.IncidentRepository.GetIncidentDataByKey(incident.Oid);
                
                var result = _emailConfigController.SendTicketCreationEmail(incidentDb);

                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.SaveMessage, incidentDb);
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
        public async Task<ResponseDto> ReadIncidentsByKey(long key, long userAccountId, int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByKey(key, userAccountId, start, take, status);
                
                return new ResponseDto(HttpStatusCode.OK, true, incidentInDb== null ? "Data Not Found" : string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/status
        /// </summary>
        /// <returns>List of incident status.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByStatus)]
        public async Task<ResponseDto> ReadIncidentsByStatus(bool key, int start, int take)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByStatus(key, start, take);
                
                return new ResponseDto(HttpStatusCode.OK, true, incidentInDb== null ? "Data Not Found" : string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/expart
        /// </summary>
        /// <returns>List of Expart incident.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByExpert)]
        public async Task<ResponseDto> GetIncidentsByExpart(long key, int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByExpart(key, start, take, status);

                return new ResponseDto(HttpStatusCode.OK, true, incidentInDb== null ? "Data Not Found" : string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/expartleader
        /// </summary>
        /// <returns>List of ExpertLeader incident.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByExpertLeader)]
        public async Task<ResponseDto> GetIncidentsByExpartLeader(int key, int assignedTo, int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByExpartLeader(key, assignedTo, start, take, status);

                return new ResponseDto(HttpStatusCode.OK, true, incidentInDb== null ? "Data Not Found" : string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/search
        /// </summary>
        /// <param name="search"></param>
        /// <returns>List of incident search.</returns>
        [HttpPost]
        [Route(RouteConstants.ReadIncidentsBySearch)]
        public async Task<ResponseDto> ReadIncidentsBySearch(IncidentSearchDto search, int start, int take, bool status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsBySearch(search, start, take);

                return new ResponseDto(HttpStatusCode.OK, true, incidentInDb== null ? "Data Not Found" : string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incident/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Instance of the table object</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentByKey)]
        public async Task<ResponseDto> ReadIncidentByKey(long key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                IncidentDto dto = new();

                dto = await GetIncident(key);

                return new ResponseDto(HttpStatusCode.OK, true, dto== null ? "Data Not Found" : string.Empty, dto);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/incident/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="incident">Object to be updated</param>
        /// <returns>Update row in the table</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateIncident)]
        public async Task<ResponseDto> UpdateIncident(long key, Incident incident)
        {
            try
            {
                long? previousAssignUserId;

                if (key != incident.Oid)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                var incidentInDb = await context.IncidentRepository.GetIncidentDataByKey(key);

                previousAssignUserId = incidentInDb.AssignedTo;

                if (incidentInDb.IsReassigned == true && incidentInDb.AssignedTo != incidentInDb.AssignedToState)
                {
                    incidentInDb.ReassignedTo = incident.AssignedTo;
                    incidentInDb.AssignedTo = incident.AssignedTo;
                    incidentInDb.ReassignDate = DateTime.Now;
                }

                incidentInDb.DateModified = DateTime.Now;
                incidentInDb.FirstLevelCategoryId = incident.FirstLevelCategoryId;
                incidentInDb.SecondLevelCategoryId = incident.SecondLevelCategoryId;
                incidentInDb.ThirdLevelCategoryId = incident.ThirdLevelCategoryId;
                incidentInDb.TeamId = incident.TeamId;
                incidentInDb.TicketTitle = incident.TicketTitle;
                incidentInDb.Description = incident.Description;
                incidentInDb.ModifiedBy = incident.ModifiedBy;
                incidentInDb.DateModified = DateTime.Now;
                incidentInDb.PriorityId = incident.PriorityId;
                incidentInDb.AssignedTo = incident.AssignedTo;
                incidentInDb.CallerCellphone  = incident.CallerCellphone;
                incidentInDb.CallerEmail = incident.CallerEmail;
                incidentInDb.CallerCountryCode= incident.CallerCountryCode;
                incidentInDb.CallerCellphone = incident.CallerCellphone;
                incidentInDb.CallerJobTitle  = incident.CallerJobTitle;
                incidentInDb.CallerName = incident.CallerName;

                context.IncidentRepository.Update(incidentInDb);
                await context.SaveChangesAsync();

                IncidentActionLog dbCurrentIncidentActionLog = new();

                dbCurrentIncidentActionLog = await context.IncidentActionLogRepository.GetIncidentActionByIncidentID(incidentInDb.Oid);

                if (dbCurrentIncidentActionLog is null)
                {
                    IncidentActionLog incidentAction = new IncidentActionLog();

                    incidentAction.IncidentId = incidentInDb.Oid;
                    incidentAction.CreatedBy = incidentInDb.CreatedBy;
                    incidentAction.DateCreated =  DateTime.Now;
                    incidentAction.IsDeleted =  false;

                    if (incident.AgentId>0)
                    {
                        incidentAction.AgentId =incident.AgentId;
                        incidentAction.AgentDateModified =  DateTime.Now;
                    }
                    if (incident.AdminId>0)
                    {
                        incidentAction.AdminId =incident.AdminId;
                        incidentAction.AdminDateModified =  DateTime.Now;

                        if (incidentInDb.AssignedTo is not null)
                            await AdminLogSave(key, incident, dbCurrentIncidentActionLog, (long)incidentInDb?.AssignedTo);
                    }
                    if (incident.ExpertId>0)
                    {
                        incidentAction.ExpertId =incident.ExpertId;
                        incidentAction.ExpertDateModified =  DateTime.Now;
                    }
                    if (incident.SupervisedId>0)
                    {
                        incidentAction.SupervisedId = incident.SupervisedId;
                        incidentAction.SupervisedDateModified = DateTime.Now;
                    }

                    context.IncidentActionLogRepository.Add(incidentAction);
                    await context.SaveChangesAsync();
                }
                else
                {
                    if (incident.AgentId>0)
                    {
                        dbCurrentIncidentActionLog.AgentId =incident.AgentId;
                        dbCurrentIncidentActionLog.AgentDateModified =  DateTime.Now;
                    }
                    else if (incident.ExpertId>0)
                    {
                        dbCurrentIncidentActionLog.ExpertId =incident.ExpertId;
                        dbCurrentIncidentActionLog.ExpertDateModified =  DateTime.Now;
                    }
                    else if (incident.SupervisedId>0)
                    {
                        dbCurrentIncidentActionLog.SupervisedId =incident.SupervisedId;
                        dbCurrentIncidentActionLog.SupervisedDateModified =  DateTime.Now;
                    }
                    else if (incident.TeamLeadId>0)
                    {
                        dbCurrentIncidentActionLog.TeamLeadId =incident.TeamLeadId;
                        dbCurrentIncidentActionLog.TeamLeadDateModified =  DateTime.Now;
                    }
                    else if (incident.AdminId>0)
                    {
                        dbCurrentIncidentActionLog.AdminId =incident.AdminId;
                        dbCurrentIncidentActionLog.AdminDateModified =  DateTime.Now;

                        if (previousAssignUserId is not null)
                            await AdminLogSave(key, incident, dbCurrentIncidentActionLog, (long)previousAssignUserId);
                    }
                    else
                    {
                        dbCurrentIncidentActionLog.ModifiedBy = incident.ModifiedBy;
                        dbCurrentIncidentActionLog.DateModified = DateTime.Now;
                    }

                    context.IncidentActionLogRepository.Update(dbCurrentIncidentActionLog);
                    await context.SaveChangesAsync();
                }

                if (incident.FundingAgencyList != null)
                {
                    foreach (var item in incident.FundingAgencyList)
                    {
                        IncidendtFundingAgency fundingAgencyItem = new IncidendtFundingAgency();

                        fundingAgencyItem.FundingAgencyId = item;
                        fundingAgencyItem.IncidentId = incident.Oid;
                        fundingAgencyItem.DateCreated = DateTime.Now;
                        fundingAgencyItem.IsDeleted = false;
                        context.FundingAgencyItemRepository.Add(fundingAgencyItem);
                        await context.SaveChangesAsync();
                    }
                }

                if (incident.ImplementingList != null)
                {
                    foreach (var item in incident.ImplementingList)
                    {
                        IncidentImplemenentingPartner implemenentingItem = new IncidentImplemenentingPartner();

                        implemenentingItem.ImplementingId = item;
                        implemenentingItem.IncidentId = incident.Oid;
                        implemenentingItem.DateCreated = DateTime.Now;
                        implemenentingItem.IsDeleted = false;
                        context.ImplementingItemRepository.Add(implemenentingItem);
                        await context.SaveChangesAsync();
                    }
                }
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        private async Task AdminLogSave(long key, Incident incident, IncidentActionLog? dbCurrentIncidentActionLog, long? assignTo)
        {
            try
            {
                IncidentAdminActionLog incidentAdminActionLog = await context.IncidentAdminActionLogRepository.GetIncidentAdminActionByIncidentID(key);

                if (incidentAdminActionLog is null)
                {
                    incidentAdminActionLog = new IncidentAdminActionLog();

                    incidentAdminActionLog.IncidentId = key;
                    incidentAdminActionLog.ModifiedBy = incident.CreatedBy;
                    incidentAdminActionLog.DateModified = DateTime.Now;
                    string assignUserName = "Not Assign";

                    if (assignTo is not null)
                    {
                        assignUserName = context.UserAccountRepository.GetUserAccountByKey((long)assignTo).Result.Username;
                    }

                    incidentAdminActionLog.ChangeHistory=$"[Modified Date = {dbCurrentIncidentActionLog?.AdminDateModified}, Assigne User ={assignUserName}]";
                   
                    context.IncidentAdminActionLogRepository.Add(incidentAdminActionLog);
                    await context.SaveChangesAsync();
                }
                else
                {
                    incidentAdminActionLog.ModifiedBy = incident.ModifiedBy;
                    incidentAdminActionLog.DateModified = DateTime.Now;

                    string previousAssignUserName = "Not Assign";
                    if (assignTo is not null)
                    {
                        previousAssignUserName = context.UserAccountRepository.GetUserAccountByKey((long)assignTo).Result.Username;
                    }
                    string reassignUserName = context.UserAccountRepository.GetUserAccountByKey((long)(incident.AssignedTo)).Result.Username;

                    StringBuilder stringBuilder = new StringBuilder();

                    stringBuilder.Append(incidentAdminActionLog.ChangeHistory);
                    stringBuilder.Append($"\n[Admin Last Modified Date = {dbCurrentIncidentActionLog?.AdminDateModified}, Admin New Modified Date = {DateTime.Now}, Previous Assigne User ={previousAssignUserName}, Reassign User = {reassignUserName}]");

                    incidentAdminActionLog.ChangeHistory=  stringBuilder.ToString();

                    context.IncidentAdminActionLogRepository.Update(incidentAdminActionLog);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// URl: tuso-api/incident/close/{key}
        /// <param name="key">Primary key of the table</param>
        /// <returns>Update row in the table</returns>
        [HttpDelete]
        [Route(RouteConstants.CloseIncident)]
        public async Task<ResponseDto> CloseIncidentsByKey(CloseIncidentDto closeIncident)
        {
            try
            {
                var incident = await context.IncidentRepository.GetIncidentDataByKey(closeIncident.Oid);

                if (incident == null)
                    return new ResponseDto(HttpStatusCode.NoContent, false, MessageConstants.NoMatchFoundError, null);

                incident.IsOpen = false;
                incident.IsResolved = true;
                incident.DateResolved= DateTime.Now;

                context.IncidentRepository.Update(incident);
                await context.SaveChangesAsync();

                IncidentActionLog dbCurrentIncidentActionLog = await context.IncidentActionLogRepository.GetIncidentActionByIncidentID(incident.Oid);

                if (dbCurrentIncidentActionLog is null)
                {
                    IncidentActionLog incidentAction = new IncidentActionLog();

                    incidentAction.IncidentId = closeIncident.Oid;
                    incidentAction.DateModified = DateTime.Now;

                    if (closeIncident.ExpertId>0)
                    {
                        incidentAction.ExpertId = closeIncident.ExpertId;
                        incidentAction.ExpertDateModified = DateTime.Now;
                    }
                    if (closeIncident.UserId>0)
                    {
                        incidentAction.CloseUserAccountId = closeIncident.UserId;
                        incidentAction.DateClosed = DateTime.Now;
                    }

                    context.IncidentActionLogRepository.Add(incidentAction);
                    await context.SaveChangesAsync();
                }
                else
                {
                    IncidentActionLog incidentAction = context.IncidentActionLogRepository.GetIncidentActionsByIncidentID(closeIncident.Oid).Result.FirstOrDefault();
                    
                    incidentAction.DateModified = DateTime.Now;
                    if (closeIncident.ExpertId>0)
                    {
                        incidentAction.ExpertId = closeIncident.ExpertId;
                        incidentAction.ExpertDateModified = DateTime.Now;
                    }
                    if (closeIncident.UserId>0)
                    {
                        incidentAction.CloseUserAccountId = closeIncident.UserId;
                        incidentAction.DateClosed = DateTime.Now;
                    }

                    context.IncidentActionLogRepository.Update(incidentAction);
                    await context.SaveChangesAsync();
                }

                var result = _emailConfigController.SendTicketCloseEmail(closeIncident.Oid);
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/incident/{key}
        /// </summary>
        /// <param name="key>Primary key of the table</param>
        /// <returns>Object to be deleted.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteIncident)]
        public async Task<ResponseDto> DeleteIncident(long key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var incidentInDb = await context.IncidentRepository.GetIncidentDataByKey(key);

                if (incidentInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                incidentInDb.IsDeleted = true;
                incidentInDb.DateModified = DateTime.Now;

                context.IncidentRepository.Update(incidentInDb);
                await context.SaveChangesAsync();
                
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.DeleteMessage, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URl: tuso-api/incidents/incidentList
        /// </summary>
        /// <returns> Single incident </returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByAdvancedSearch)]
        public async Task<ResponseDto> ReadIncidentsByAdvancedSearch(int start, int take, int? status, DateTime? fromDate, DateTime? toDate, int? ticketNo, int? faciltyId, int? provinceId, int? districtId, int? systemId)
        {
            try
            {
                var Incident = await context.IncidentRepository.GetIncidentBySearch(start, take, status, fromDate, toDate, ticketNo, faciltyId, provinceId, districtId, systemId);

                return new ResponseDto(HttpStatusCode.OK, true, Incident== null ? "Data Not Found" : string.Empty, Incident);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/weeklyIncidentList
        /// </summary>
        /// <returns> Single incident </returns>
        [HttpGet]
        [Route(RouteConstants.ReadWeeklyIncidentsByAdvancedSearch)]
        public async Task<ResponseDto> ReadWeeklyIncidentsByAdvancedSearch(int start, int take, int? status, int? ticketNo, int? faciltyId, int? provinceId, int? districtId, int? systemId)
        {
            try
            {
                var Incident = await context.IncidentRepository.GetWeeklyIncidentBySearch(start, take, status, ticketNo, faciltyId, provinceId, districtId, systemId);

                return new ResponseDto(HttpStatusCode.OK, true, Incident== null ? "Data Not Found" : string.Empty, Incident);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/username
        /// </summary>
        /// <returns>List of incident.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentsByUserName)]
        public async Task<ResponseDto> ReadIncidentsByUserName(string UserName, int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByUserName(UserName, start, take, status);
               
                return new ResponseDto(HttpStatusCode.OK, true, incidentInDb== null ? "Data Not Found" : string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        [HttpGet]
        [Route(RouteConstants.GetIncidentsByAssignUserName)]
        public async Task<ResponseDto> GetIncidentsByAssignUserName(string userName, int start, int take, int status)
        {
            try
            {
                var incidentInDb = await context.IncidentRepository.GetIncidentsByAssignUserName(userName, start, take, status);
               
                return new ResponseDto(HttpStatusCode.OK, true, incidentInDb== null ? "Data Not Found" : string.Empty, incidentInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URl: tuso-api/incidents/incidentCount
        /// </summary>
        /// <returns> Total,Resolved and Unresolved incidents count</returns>
        [HttpGet]
        [Route(RouteConstants.ReadIncidentCount)]
        public async Task<ResponseDto> ReadIncidentCount(string? userName)
        {
            try
            {
                var incidentCountDto = await context.IncidentRepository.IncidentCount(userName);

                return new ResponseDto(HttpStatusCode.OK, true, incidentCountDto== null ? "Data Not Found" : string.Empty, incidentCountDto);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadIncidentClientCount)]
        public async Task<ResponseDto> ReadClientIncidentCount(string? userName)
        {
            try
            {
                ClientIncidentCountDto clientIncidentCount = new ClientIncidentCountDto();

                clientIncidentCount = await context.IncidentRepository.IncidentClientCount(userName);

                return new ResponseDto(HttpStatusCode.OK, true, clientIncidentCount== null ? "Data Not Found" : string.Empty, clientIncidentCount);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        private async Task<IncidentDto> GetIncident(long key)
        {
            IncidentDto dto = new IncidentDto();

            dto.Incidents = await context.IncidentRepository.GetIncidentByKey(key);
            dto.Messages = await context.MessageRepository.GetMessageByIncedent(key);
            dto.Messages = dto.Messages.OrderByDescending(o => o.Oid);

            return dto;
        }
    }
}