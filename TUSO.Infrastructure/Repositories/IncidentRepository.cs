using Abp.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Stephan
 * Date created: 05.09.2022
 * Last modified: 06.09.2022, 4.10.2022
 * Modified by: Stephan
 */
namespace TUSO.Infrastructure.Repositories
{
    public class IncidentRepository : Repository<Incident>, IIncidentRepository
    {
        public IncidentRepository(DataContext context) : base(context)
        {

        }

        public async Task<IncidentListDto> GetIncidentByKey(long key)
        {
            try
            {
                return GetSingleIncident(i => i.Oid == key && i.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Incident> GetIncidentDataByKey(long key)
        {
            try
            {
                return await FirstOrDefaultAsync(i => i.Oid == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentListReturnDto> GetIncidentsByKey(long key, long UserAccountId, int start, int take, int status)
        {
            try
            {
                Expression<Func<Incident, bool>> predicate = w => w.IsDeleted == false;

                var role = (from u in context.UserAccounts.Where(w => w.Oid == UserAccountId)
                            join m in context.Members on u.Oid equals m.UserAccountId into mm
                            from m in mm.DefaultIfEmpty()
                            select new { RoleName = u.Roles.RoleName, u.Oid, TeamId = m == null ? 0 : m.TeamId, Leader = m == null ? false : true }).FirstOrDefault();

                if (role.RoleName == "Client")
                {
                    predicate = predicate.And(w => w.ReportedBy == role.Oid);
                }
                else if (role.RoleName == "Expert")
                {
                    if (role.Leader)
                    {
                        predicate = predicate.Or(w => w.TeamId == role.TeamId).And(w => w.AssignedTo != null);
                    }
                    else
                    {
                        predicate = predicate.And(w => w.AssignedTo == role.Oid);
                    }
                }

                var length = key.ToString().Length;

                bool st = false;

                if (status == 0)
                {
                    predicate = predicate.And(i => i.Oid.ToString().Substring(0, length) == key.ToString() && i.IsDeleted == false);

                }
                else
                {
                    st = status == 1 ? true : false;
                    predicate = predicate.And(i => i.Oid.ToString().Substring(0, length) == key.ToString() && i.IsDeleted == false && i.IsOpen == st);

                }

                return GetIncidentList(predicate, start, take);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<ClientIncidentCountDto> IncidentClientCount(string? UserName)
        {
            try
            {
                ClientIncidentCountDto clientIncident = new ClientIncidentCountDto();
                UserAccount userAccount = context.UserAccounts.FirstOrDefault(x => x.Username == UserName && x.IsDeleted == false);

                if (userAccount is not null && userAccount.RoleId == 1)
                {
                    clientIncident.TotalTickets = context.Incidents.Count(x => x.CreatedBy == userAccount.Oid && x.IsDeleted == false);

                    clientIncident.TotalOpenTickets = context.Incidents.Count(x => x.CreatedBy == userAccount.Oid && x.IsOpen == true);

                    clientIncident.TotalCloseTickets = context.Incidents.Count(x => x.CreatedBy == userAccount.Oid && x.IsDeleted == false && x.IsOpen == false);


                    clientIncident.LastMonthTotalTickets = context.Incidents
                                .Where(p => p.DateCreated.Value.Year == DateTime.Now.Year && p.CreatedBy == userAccount.Oid)
                                .GroupBy(p => p.DateCreated.Value.Month)
                                .Select(p => new LastMonthTotalTicket { Month = p.Key == 1 ? "January" : p.Key == 2 ? "February" : p.Key == 3 ? "March" : p.Key == 4 ? "April" : p.Key == 5 ? "May" : p.Key == 6 ? "June" : p.Key == 7 ? "July" : p.Key == 8 ? "August" : p.Key == 9 ? "September" : p.Key == 10 ? "October" : p.Key == 11 ? "November" : "December", Count = p.Count() }).ToList();

                    clientIncident.LastMonthTotalTickets = clientIncident.LastMonthTotalTickets.TakeLast(5).ToList();
                }

                return await Task.Run(() => clientIncident);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<IncidentListReturnDto> GetIncidentsByClient(long key, int start, int take, int status)
        {
            try
            {
                bool st = false;

                if (status == 0)
                {
                    return GetIncidentList(i => i.ReportedBy == key && i.IsDeleted == false, start, take);
                }
                else
                {
                    st = status == 1 ? true : false;

                    return GetIncidentList(i => i.ReportedBy == key && i.IsDeleted == false && i.IsOpen == st, start, take);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentListReturnDto> GetIncidentsByExpart(long key, int start, int take, int status)
        {
            try
            {
                bool st = false;
                if (status == 0)
                {
                    return GetIncidentList(i => i.AssignedTo == key && i.IsDeleted == false, start, take);
                }
                else
                {
                    st = status == 1 ? true : false;

                    return GetIncidentList(i => i.AssignedTo == key && i.IsDeleted == false && i.IsOpen == st, start, take);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentListReturnDto> GetIncidentsByExpartLeader(int key, int assignedTo, int start, int take, int status)
        {
            try
            {
                bool st = false;
                if (status == 0)
                {
                    return await Task.Run(() => GetIncidentList(i => ((i.TeamId == key || i.AssignedTo == assignedTo) && i.IsDeleted == false), start, take));
                }
                else
                {
                    st = status == 1 ? true : false;

                    return await Task.Run(() => GetIncidentList(i => ((i.TeamId == key || i.AssignedTo == assignedTo) && i.IsDeleted == false && i.IsOpen == st), start, take));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentListReturnDto> GetIncidentsBySearch(IncidentSearchDto search, int start, int take)
        {
            try
            {
                Expression<Func<Incident, bool>> predicate = w => w.IsDeleted == false;

                if (search.SystemId != null && (int)search.SystemId > 0)
                {
                    predicate = predicate.And(w => w.SystemId == search.SystemId);
                }
                if (search.Status != null && (int)search.Status > 0)
                {
                    var sts = search.Status == 2 ? false : true;

                    predicate = predicate.And(w => w.IsOpen == sts);
                }
                if (search.DateFrom != null)
                {
                    predicate = predicate.And(w => w.DateReported.AddDays(1) > (DateTime)search.DateFrom);
                }
                if (search.DateTo != null)
                {
                    predicate = predicate.And(w => w.DateReported.AddDays(-1) < (DateTime)search.DateTo);
                }
                if (search.ProvinceId != null && (int)search.ProvinceId > 0)
                {
                    predicate = predicate.And(p => p.Facilities.Districts.ProvinceId == search.ProvinceId);
                }
                if (search.DistrictId != null && (int)search.DistrictId > 0)
                {
                    predicate = predicate.And(w => w.Facilities.DistrictId == search.DistrictId);
                }
                if (search.FacilityId != null && (int)search.FacilityId > 0)
                {
                    predicate = predicate.And(w => w.FacilityId == search.FacilityId);
                }
                if (search.FirstLevelCategoryId != null && (int)search.FirstLevelCategoryId > 0)
                {
                    predicate = predicate.And(w => w.FirstLevelCategoryId == search.FirstLevelCategoryId);
                }
                if (search.SecondLevelCategoryId != null && (int)search.SecondLevelCategoryId > 0)
                {
                    predicate = predicate.And(w => w.SecondLevelCategoryId == search.SecondLevelCategoryId);
                }
                if (search.ThirdLevelCategoryId != null && (int)search.ThirdLevelCategoryId > 0)
                {
                    predicate = predicate.And(w => w.ThirdLevelCategoryId == search.ThirdLevelCategoryId);
                }
                if (search.TeamId != null && (int)search.TeamId > 0)
                {
                    predicate = predicate.And(w => w.TeamId == search.TeamId);
                }
                else
                {
                    var role = context.Roles.Where(w => w.Oid == search.RoleId).Select(s => s.RoleName).FirstOrDefault();

                    if (role == "Client")
                    {
                        predicate = predicate.And(w => w.ReportedBy == search.UserAccountId);
                    }
                    else if (role == "Expert")
                    {
                        if ((int)search.TeamId > 0)
                        {
                            predicate = predicate.And(w => w.TeamId == search.TeamId);
                        }
                        else
                        {
                            predicate = predicate.And(w => w.AssignedTo == search.UserAccountId);
                        }

                    }
                }

                return GetIncidentList(predicate, start, take);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentListReturnDto> GetIncidents(int start, int take, int status)
        {
            try
            {
                bool st = false;

                if (status == 0)
                {
                    return await Task.Run(() => GetIncidentList(i => i.IsDeleted == false, start, take));
                }
                else
                {
                    st = status == 1 ? true : false;

                    return await Task.Run(() => GetIncidentList(i => i.IsDeleted == false && i.IsOpen == st, start, take));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentListReturnDto> GetIncidentsByStatus(bool key, int start, int take)
        {
            try
            {
                return await Task.Run(() => GetIncidentList(i => i.IsDeleted == false && i.IsOpen == key, start, take));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IncidentListReturnDto GetIncidentList(Expression<Func<Incident, bool>> predicate, int start, int take)
        {
            var data = (from i in context.Incidents.Where(predicate)
                        join f in context.Facilities on i.FacilityId equals f.Oid
                        join d in context.Districts on f.DistrictId equals d.Oid
                        join pv in context.Provinces on d.ProvinceId equals pv.Oid
                        join p in context.Projects on i.SystemId equals p.Oid
                        join u in context.UserAccounts on i.ReportedBy equals u.Oid
                        join t in context.Teams on i.TeamId equals t.Oid into tt
                        from t in tt.DefaultIfEmpty()
                        join m in context.UserAccounts on i.AssignedTo equals m.Oid into mm
                        from m in mm.DefaultIfEmpty()
                        join s in context.Screenshots on i.Oid equals s.Oid into ss
                        from s in ss.DefaultIfEmpty()
                        select new IncidentListDto
                        {
                            Oid = i.Oid,
                            FacilityId = i.FacilityId,
                            FacilityName = f.FacilityName,
                            DistrictName=d.DistrictName,
                            ProvincName=pv.ProvinceName,
                            PriorityId = i.PriorityId,
                            ReportedBy = i.ReportedBy,
                            Description = i.Description,
                            TicketTitle = i.TicketTitle,
                            FullName = u.Name,
                            PhoneNumber = u.CountryCode + u.Cellphone,
                            SystemId = i.SystemId,
                            Projectname = p.Title,
                            FirstLevelCategoryId = i.FirstLevelCategoryId,
                            SecondLevelCategoryId = i.SecondLevelCategoryId,
                            ThirdLevelCategoryId = i.ThirdLevelCategoryId,
                            FirstLevelCategory = i.FirstLevelCategoryId != null ? context.IncidentCategories
                                 .Where(IncidentCategories => IncidentCategories.Oid == i.FirstLevelCategoryId)
                                 .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                 .FirstOrDefault() : null,
                            SecondLevelCategory = i.SecondLevelCategoryId != null ? context.IncidentCategories
                                 .Where(IncidentCategories => IncidentCategories.Oid == i.SecondLevelCategoryId)
                                 .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                 .FirstOrDefault() : null,
                            ThirdLevelCategory = i.ThirdLevelCategoryId != null ? context.IncidentCategories
                                 .Where(IncidentCategories => IncidentCategories.Oid == i.ThirdLevelCategoryId)
                                 .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                 .FirstOrDefault() : null,
                            FundingAgencyName = i.Projects.FundingAgencies
                                  .Where(fundingAgency => fundingAgency.IsDeleted == false)
                                  .Select(agnecy => agnecy.FundingAgencyName).FirstOrDefault(),
                            ImplementingPartnerName = i.Projects.ImplementingPartners
                                  .Where(implementingPartner => implementingPartner.IsDeleted == false)
                                  .Select(partner => partner.ImplementingPartnerName).FirstOrDefault(),
                            DateReported = i.DateReported,
                            DateResolved = i.DateResolved,
                            AssignedTo = i.AssignedTo,
                            IsOpen = i.IsOpen,
                            TeamId = i.TeamId,
                            IsResolved = i.IsResolved,
                            IsReassigned = i.IsReassigned,
                            CallerName = i.CallerName,
                            CallerCountryCode = i.CallerCountryCode,
                            CallerCellphone = i.CallerCellphone,
                            CallerEmail = i.CallerEmail,
                            CallerJobTitle = i.CallerJobTitle,
                            DateOfIncident = i.DateOfIncident,
                            TeamName = t == null ? "" : t.Title,
                            AssignedName = m == null ? "" : m.Name,
                            HasImg = s == null ? false : true
                        }).OrderByDescending(o => o.Oid).Skip(start).Take(take).ToList();

            List<IncidentListDto> dto = new List<IncidentListDto>();
            dto = data;


            IncidentListReturnDto list = new IncidentListReturnDto
            {
                List = dto,
                CurrentPage = start + 1,
                TotalIncident = context.Incidents.Where(predicate).Count()
            };
            return list;
        }

        public IncidentListDto GetSingleIncident(Expression<Func<Incident, bool>> predicate)
        {
            var data = (from i in context.Incidents.Where(predicate)
                        join f in context.Facilities on i.FacilityId equals f.Oid
                        join d in context.Districts on f.DistrictId equals d.Oid
                        join pv in context.Provinces on d.ProvinceId equals pv.Oid
                        join p in context.Projects on i.SystemId equals p.Oid
                        join u in context.UserAccounts on i.ReportedBy equals u.Oid
                        join t in context.Teams on i.TeamId equals t.Oid into tt
                        from t in tt.DefaultIfEmpty()
                        join m in context.UserAccounts on i.AssignedTo equals m.Oid into mm
                        from m in mm.DefaultIfEmpty()
                        join s in context.Screenshots on i.Oid equals s.Oid into ss
                        from s in ss.DefaultIfEmpty()
                        select new
                        {
                            i.Oid,
                            i.FacilityId,
                            f.FacilityName,
                            d.DistrictName,
                            pv.ProvinceName,
                            i.PriorityId,
                            i.ReportedBy,
                            i.Description,
                            i.TicketTitle,
                            u.Name,
                            PhoneNumber = u.CountryCode + u.Cellphone,
                            i.SystemId,
                            p.Title,
                            i.FirstLevelCategoryId,
                            i.SecondLevelCategoryId,
                            i.ThirdLevelCategoryId,
                            i.DateReported,
                            i.CreatedBy,
                            i.DateResolved,
                            i.AssignedTo,
                            i.DateCreated,
                            i.DateModified,
                            i.ModifiedBy,
                            i.IsOpen,
                            i.TeamId,
                            i.IsResolved,
                            i.CallerName,
                            i.CallerCountryCode,
                            i.CallerCellphone,
                            i.CallerEmail,
                            i.CallerJobTitle,
                            i.DateOfIncident,
                            FundingAgencyName = i.Projects.FundingAgencies
                                  .Where(fundingAgency => fundingAgency.IsDeleted == false)
                                  .Select(agnecy => agnecy.FundingAgencyName).FirstOrDefault(),
                            ImplementingPartnerName = i.Projects.ImplementingPartners
                                  .Where(implementingPartner => implementingPartner.IsDeleted == false)
                                  .Select(partner => partner.ImplementingPartnerName).FirstOrDefault(),
                            TeamName = t == null ? "" : t.Title,
                            AssaignName = m == null ? "" : m.Name,
                            HasImg = s == null ? false : true
                        }).FirstOrDefault();

            IncidentListDto dto = new IncidentListDto();

            if (data != null)
            {
                dto.AssignedTo = data.AssignedTo;
                dto.DateReported = data.DateReported;
                dto.DateResolved = data.DateResolved;
                dto.DateOfIncident = data.DateOfIncident;
                dto.DateCreated = data.DateCreated;
                dto.DateModified = data.DateModified;
                dto.CreatedBy = data.CreatedBy;
                dto.ModifiedBy = data.ModifiedBy;
                dto.Description = data.Description;
                dto.FacilityId = data.FacilityId;
                dto.FacilityName = data.FacilityName;
                dto.DistrictName= data.DistrictName;
                dto.ProvincName = data.ProvinceName;
                dto.FirstLevelCategoryId = data.FirstLevelCategoryId;
                dto.FullName = data.Name;
                dto.IsOpen = data.IsOpen;
                dto.IsResolved = data.IsResolved;
                dto.Oid = data.Oid;
                dto.TicketTitle = data.TicketTitle;
                dto.PhoneNumber = data.PhoneNumber;
                dto.PriorityId = data.PriorityId;
                dto.Projectname = data.Title;
                dto.ReportedBy = data.ReportedBy;
                dto.SecondLevelCategoryId = data.SecondLevelCategoryId;
                dto.SystemId = data.SystemId;
                dto.TeamId = data.TeamId;
                dto.ThirdLevelCategoryId = data.ThirdLevelCategoryId;
                dto.HasImg = data.HasImg;
                dto.TeamName = data.TeamName;
                dto.AssignedName = data.AssaignName;
                dto.HasImg = data.HasImg;
                dto.CallerName = data.CallerName;
                dto.CallerCellphone = data.CallerCellphone;
                dto.CallerCountryCode = data.CallerCountryCode;
                dto.CallerEmail = data.CallerEmail;
                dto.CallerJobTitle = data.CallerJobTitle;
                dto.FundingAgencyName = data.FundingAgencyName;
                dto.ImplementingPartnerName = data.ImplementingPartnerName;
            }

            return dto;
        }

        public async Task<IncidentLifeCycleListDto> GetIncidentBySearch(int start, int take, int? status, DateTime? FromDate, DateTime? ToDate, int? TicketNo, int? Facilty, int? Province, int? District, int? SystemId)
        {
            try
            {
                Expression<Func<Incident, bool>> predicate = x => x.IsDeleted == false;

                bool ticketStatus = false;

                List<int> HandlingTimeList = new List<int>();

                if (status != null && status != 0)
                {
                    ticketStatus = status == 1 ? true : false;
                    predicate = predicate.And(i => i.IsOpen == ticketStatus);
                }
                if (FromDate != null)
                {
                    predicate = predicate.And(i => i.DateOfIncident.Value.Date >= FromDate.Value.Date);
                    if (ToDate == null)
                    {
                        ToDate = DateTime.Now;
                    }
                }

                if (ToDate != null)
                    predicate = predicate.And(i => i.DateOfIncident.Value.Date <= ToDate.Value.Date);

                if (TicketNo != null)
                    predicate = predicate.And(x => x.Oid == TicketNo);

                if (SystemId != null)
                    predicate = predicate.And(x => x.SystemId == SystemId);

                if (Province != null)
                    predicate = predicate.And(x => x.Facilities.Districts.Provinces.Oid == Province);

                if (District != null)
                    predicate = predicate.And(x => x.Facilities.Districts.Oid == District);

                if (Facilty != null)
                    predicate = predicate.And(x => x.FacilityId == Facilty);


                var incident = context.Incidents.Where(predicate).Include(Incident => Incident.Facilities)

                         .Include(T => T.Teams).Join(context.UserAccounts,
                          userAccount => userAccount.ReportedBy,
                          UserInRole => UserInRole.Oid, (u, uir) => new { u, uir }).OrderByDescending(x => x.u.Oid)
                         .Join(context.Roles, r => r.uir.RoleId, ro => ro.Oid, (r, ro) => new { r, ro })
                         .Select(x => new IncidentLifeCycleDto
                         {
                             TicketNo = x.r.u.Oid.ToString(),

                             TicketTitle = x.r.u.TicketTitle,

                             SystemName = x.r.u.Projects.Title,

                             FundingAgencyName = x.r.u.Projects.FundingAgencies.Where(fundingAgency => fundingAgency.IsDeleted == false).Select(agnecy => agnecy.FundingAgencyName).FirstOrDefault(),

                             ImplementingPartnerName = x.r.u.Projects.ImplementingPartners.Where(implementingPartner => implementingPartner.IsDeleted == false).Select(partner => partner.ImplementingPartnerName).FirstOrDefault(),

                             FacilityName = x.r.u.Facilities.FacilityName,

                             DistrictName = context.Districts
                                      .Where(districts => districts.Oid == x.r.u.Facilities.DistrictId)
                                      .Count() > 0 ? context.Districts
                                      .Where(districts => districts.Oid == x.r.u.Facilities.DistrictId)
                                      .Select(district => district.DistrictName).FirstOrDefault() : null,

                             ProvinceName = context.Districts.Join(context.Facilities, Districts => Districts.Oid,

                                 Facilities => Facilities.DistrictId, (districts, facilities) => new { districts, facilities })
                                      .Join(context.Provinces, FaclitiesInDistrict => FaclitiesInDistrict.districts.ProvinceId,

                                Provinces => Provinces.Oid, (FacilitiesInDistrict, Provinces) => new { FacilitiesInDistrict, Provinces })
                                      .Where(ProvincesInDistrict => ProvincesInDistrict.FacilitiesInDistrict.districts.Oid == x.r.u.Facilities.DistrictId)
                                      .Count() > 0 ? context.Districts.Join(context.Facilities,

                                Districts => Districts.Oid, Facilities => Facilities.DistrictId, (districts, facilities) => new { districts, facilities })
                                     .Join(context.Provinces, FaclitiesInDistrict => FaclitiesInDistrict.districts.ProvinceId,

                                Provinces => Provinces.Oid, (FacilitiesInDistrict, Provinces) => new { FacilitiesInDistrict, Provinces })
                                     .Where(ProvincesInDistrict => ProvincesInDistrict.FacilitiesInDistrict.districts.Oid == x.r.u.Facilities.DistrictId)
                                     .Select(province => province.Provinces.ProvinceName).FirstOrDefault() : null,

                             Description = x.r.u.Description,
                             StartDate = x.r.u.DateReported,
                             TicketOpenedBy = x.r.u.UserAccounts.Name,
                             UserCellphone = x.r.u.UserAccounts.CountryCode + x.r.u.UserAccounts.Cellphone,
                             UserEmail = x.r.u.UserAccounts.Email,
                             DateOfIncident = x.r.u.DateOfIncident,

                             FirstLevelCategory = x.r.u.FirstLevelCategoryId != null ? context.IncidentCategories
                                     .Where(IncidentCategories => IncidentCategories.Oid == x.r.u.FirstLevelCategoryId)
                                     .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                     .FirstOrDefault() : null,

                             SecondLevelCategory = x.r.u.SecondLevelCategoryId != null ? context.IncidentCategories
                                     .Where(IncidentCategories => IncidentCategories.Oid == x.r.u.SecondLevelCategoryId)
                                     .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                     .FirstOrDefault() : null,

                             ThirdLevelCategory = x.r.u.ThirdLevelCategoryId != null ? context.IncidentCategories
                                     .Where(IncidentCategories => IncidentCategories.Oid == x.r.u.ThirdLevelCategoryId)
                                     .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                     .FirstOrDefault() : null,


                             ExpertLeadName = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.UserAccountsTeamLeads.Name)
                                           .FirstOrDefault(),

                             ExpertName = x.r.u.AssignedTo != null ? context.UserAccounts
                                            .Where(UserAccounts => UserAccounts.Oid == x.r.u.AssignedTo)
                                            .Select(userAccount => userAccount.Name)
                                            .FirstOrDefault() : null,

                             AssignedToState = x.r.u.AssignedToState != null ? context.UserAccounts
                             .Where(UserAccount => UserAccount.Oid == x.r.u.AssignedToState)
                             .Select(UserAccount => UserAccount.Name)
                             .FirstOrDefault() : null,


                             AdminName = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.UserAccountAdmins.Name)
                                           .FirstOrDefault(),


                             AgentName = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.UserAccountAgents.Name)
                                           .FirstOrDefault(),

                             OpenBySupervisor = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.SupervisedDateModified)
                                           .FirstOrDefault(),

                             OpenByAgent = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.AgentDateModified)
                                           .FirstOrDefault(),

                             OpenByAdmin = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.AdminDateModified)
                                           .FirstOrDefault(),

                             OpenByExpert = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.ExpertDateModified)
                                           .FirstOrDefault(),

                             OpenByExpertLead = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.TeamLeadDateModified)
                                           .FirstOrDefault(),

                             SupervisorName = context.IncidentActionLogs
                                           .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                           .Select(agent => agent.UserAccountsSuperviseds.Name)
                                           .FirstOrDefault(),

                             ClientName = x.r.uir.Roles.Oid == 1 ? x.r.u.UserAccounts.Name : null,

                             CallerName = x.r.u.CallerName,

                             CallerCellphone = x.r.u.CallerCellphone,

                             CallerEmail = x.r.u.CallerEmail,

                             CallerJobTitle = x.r.u.CallerJobTitle,

                             TicketClosed = x.r.u.DateResolved,


                             Status = x.r.u.IsOpen == true ? "Open" : "Close",


                             ReassignedTo = x.r.u.ReassignedTo != null ? context.UserAccounts
                                    .Where(user => user.Oid == x.r.u.ReassignedTo)
                                    .Select(reassignedUser => reassignedUser.Name)
                                    .FirstOrDefault() : null,

                             ReassignDate = x.r.u.ReassignDate,


                             Priority = x.r.u.PriorityId != null ? context.IncidentPriorities
                                .Where(IncidentPriorities => IncidentPriorities.Oid == x.r.u.PriorityId)
                                .Select(IncidentPriority => IncidentPriority.Priority)
                                .FirstOrDefault() : null

                         }).Skip(start).Take(take).ToList();

                foreach (var ticket in incident)
                {
                    if (ticket.TicketClosed != null)
                    {
                        TimeSpan? handlingTimeDifference = (ticket.TicketClosed - ticket.StartDate);
                        int handlingTimeDifferenceInSeconds = Convert.ToInt32(handlingTimeDifference.Value.TotalSeconds);
                        HandlingTimeList.Add(handlingTimeDifferenceInSeconds);

                        var hours = (handlingTimeDifferenceInSeconds / 3600) < 10 ? "0" + (handlingTimeDifferenceInSeconds / 3600) : (handlingTimeDifferenceInSeconds / 3600).ToString();
                        var minutes = ((handlingTimeDifferenceInSeconds % 3600) / 60) < 10 ? "0" + ((handlingTimeDifferenceInSeconds % 3600) / 60) : ((handlingTimeDifferenceInSeconds % 3600) / 60).ToString();
                        var seconds = (handlingTimeDifferenceInSeconds % 60) < 10 ? "0" + (handlingTimeDifferenceInSeconds % 60) : (handlingTimeDifferenceInSeconds % 60).ToString();

                        // ticket.TotalTime = hours + ":" + minutes + ":" + seconds;
                        TimeSpan timeSpan = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), Convert.ToInt32(seconds));

                        // Calculate hours, days, weeks, and months
                        double totalHours = timeSpan.TotalHours;
                        int totalDays = (int)timeSpan.TotalDays;
                        int totalWeeks = totalDays / 7;
                        int totalMonths = (int)(totalDays / 30.44);
                        if (totalMonths > 0)
                        {
                            string monthString = totalMonths == 1 ? "Month" : "Months";
                            ticket.TotalTime = $"{totalMonths} {monthString}";
                        }
                        else if (totalWeeks > 0)
                        {
                            string weekString = totalWeeks == 1 ? "Week" : "Weeks";
                            ticket.TotalTime = $"{totalWeeks} {weekString}";
                        }
                        else if (totalDays>0)
                        {
                            string dayString = totalDays == 1 ? "Day" : "Days";
                            ticket.TotalTime = $"{totalDays} {dayString}";
                        }
                        else
                        {
                            ticket.TotalTime = hours + ":" + minutes + ":" + seconds;
                        }
                    }

                    DateTime? lastTicketOpenedDate;
                    if (ticket.StartDate != null)
                    {
                        if (ticket.OpenByExpert != null && DateTime.Compare((DateTime)ticket.OpenByExpert, (DateTime)ticket.StartDate) != 0)
                            lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByExpert ? null : ticket.OpenByExpert;

                        else if (ticket.OpenByExpertLead != null && DateTime.Compare((DateTime)ticket.OpenByExpertLead, (DateTime)ticket.StartDate) != 0)
                            lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByExpertLead ? null : ticket.OpenByExpertLead;

                        else if (ticket.OpenBySupervisor != null && DateTime.Compare((DateTime)ticket.OpenBySupervisor, (DateTime)ticket.StartDate) != 0)
                            lastTicketOpenedDate = DateTime.MinValue == ticket.OpenBySupervisor ? null : ticket.OpenBySupervisor;

                        else if (ticket.OpenByAgent != null && DateTime.Compare((DateTime)ticket.OpenByAgent, (DateTime)ticket.StartDate) != 0)
                            lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByAgent ? null : ticket.OpenByAgent;

                        else if (ticket.OpenByAdmin != null && DateTime.Compare((DateTime)ticket.OpenByAdmin, (DateTime)ticket.StartDate) != 0)
                            lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByAdmin ? null : ticket.OpenByAdmin;

                        else
                        {
                            lastTicketOpenedDate = null;
                        }
                        if (lastTicketOpenedDate != null)
                        {
                            TimeSpan? totalPendingTimeDifference = (lastTicketOpenedDate - ticket.StartDate);

                            var pendingTimeDifferenceInSeconds = Convert.ToInt32(totalPendingTimeDifference.Value.TotalSeconds);
                            var hours = (pendingTimeDifferenceInSeconds / 3600) < 10 ? "0" + (pendingTimeDifferenceInSeconds / 3600) : (pendingTimeDifferenceInSeconds / 3600).ToString();
                            var minutes = ((pendingTimeDifferenceInSeconds % 3600) / 60) < 10 ? "0" + ((pendingTimeDifferenceInSeconds % 3600) / 60) : ((pendingTimeDifferenceInSeconds % 3600) / 60).ToString();
                            var seconds = (pendingTimeDifferenceInSeconds % 60) < 10 ? "0" + (pendingTimeDifferenceInSeconds % 60) : (pendingTimeDifferenceInSeconds % 60).ToString();


                            TimeSpan timeSpan = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), Convert.ToInt32(seconds));

                            // Calculate hours, days, weeks, and months
                            double totalHours = timeSpan.TotalHours;
                            int totalDays = (int)timeSpan.TotalDays;
                            int totalWeeks = totalDays / 7;
                            int totalMonths = (int)(totalDays / 30.44);
                            if (totalMonths > 0)
                            {
                                string monthString = totalMonths == 1 ? "Month" : "Months";
                                ticket.TotalPendingTime = $"{totalMonths} {monthString}";
                            }
                            else if (totalWeeks > 0)
                            {
                                string weekString = totalWeeks == 1 ? "Week" : "Weeks";
                                ticket.TotalPendingTime = $"{totalWeeks} {weekString}";
                            }
                            else if (totalDays>0)
                            {
                                string dayString = totalDays == 1 ? "Day" : "Days";
                                ticket.TotalPendingTime = $"{totalDays} {dayString}";
                            }
                            else
                            {
                                ticket.TotalPendingTime = hours + ":" + minutes + ":" + seconds;
                            }


                        }
                        else
                        {
                            ticket.TotalPendingTime = null;
                        }
                    }
                }

                int AvgHandlingTime = HandlingTimeList.Count > 0 ? Convert.ToInt32(HandlingTimeList.Average()) : 0;
                int MinHandlingTime = HandlingTimeList.Count > 0 ? HandlingTimeList.Min() : 0;
                int MaxHandlingTime = HandlingTimeList.Count > 0 ? HandlingTimeList.Max() : 0;

                string AvgHandlingDuration = AvgHandlingTime > 0 ? GetFormattedTime(AvgHandlingTime) : null;
                string MinHandlingDuration = MinHandlingTime > 0 ? GetFormattedTime(MinHandlingTime) : null;
                string MaxHandlingDuration = MaxHandlingTime > 0 ? GetFormattedTime(MaxHandlingTime) : null;

                IncidentLifeCycleListDto incidentLifeCycleListDto = new IncidentLifeCycleListDto()
                {
                    TotalIncident = context.Incidents.Where(predicate).Count(),
                    CurrentPage = start + 1,
                    AvgHandlingDuration = AvgHandlingDuration,
                    MinHandlingDuration = MinHandlingDuration,
                    MaxHandlingDuration = MaxHandlingDuration,
                    List = incident
                };

                return incidentLifeCycleListDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IncidentLifeCycleListDto> GetWeeklyIncidentBySearch(int start, int take, int? status, int? TicketNo, int? Facilty, int? Province, int? District, int? SystemId)
        {
            Expression<Func<Incident, bool>> predicate = x => x.IsDeleted == false;
            bool ticketStatus = false;

            List<int> HandlingTimeList = new List<int>();

            DateTime? FromDate = DateTime.Today.AddDays(-6);
            DateTime? ToDate = DateTime.Today.AddDays(1);

            if (status != null && status != 0)
            {
                ticketStatus = status == 1 ? true : false;
                predicate = predicate.And(i => i.IsOpen == ticketStatus);
            }

            predicate = predicate.And(i => i.DateOfIncident.Value.Date >= FromDate.Value.Date);

            predicate = predicate.And(i => i.DateOfIncident.Value.Date < ToDate.Value.Date);

            if (TicketNo != null)
                predicate = predicate.And(x => x.Oid == TicketNo);

            if (SystemId != null)
                predicate = predicate.And(x => x.SystemId == SystemId);

            if (Province != null)
                predicate = predicate.And(x => x.Facilities.Districts.Provinces.Oid == Province);

            if (District != null)
                predicate = predicate.And(x => x.Facilities.Districts.Oid == District);

            if (Facilty != null)
                predicate = predicate.And(x => x.FacilityId == Facilty);


            var incident = context.Incidents.Where(predicate).Include(Incident => Incident.Facilities)
                     .Include(T => T.Teams).Join(context.UserAccounts,
                      userAccount => userAccount.ReportedBy,
                      UserInRole => UserInRole.Oid, (u, uir) => new { u, uir }).OrderByDescending(x => x.u.Oid)
                     .Join(context.Roles, r => r.uir.RoleId, ro => ro.Oid, (r, ro) => new { r, ro })
                     .Select(x => new IncidentLifeCycleDto
                     {
                         TicketNo = x.r.u.Oid.ToString(),

                         TicketTitle = x.r.u.TicketTitle,

                         SystemName = x.r.u.Projects.Title,

                         FundingAgencyName = x.r.u.Projects.FundingAgencies.Where(fundingAgency => fundingAgency.IsDeleted == false).Select(agnecy => agnecy.FundingAgencyName).FirstOrDefault(),

                         ImplementingPartnerName = x.r.u.Projects.ImplementingPartners.Where(implementingPartner => implementingPartner.IsDeleted == false).Select(partner => partner.ImplementingPartnerName).FirstOrDefault(),

                         FacilityName = x.r.u.Facilities.FacilityName,

                         DistrictName = context.Districts
                                  .Where(districts => districts.Oid == x.r.u.Facilities.DistrictId)
                                  .Count() > 0 ? context.Districts
                                  .Where(districts => districts.Oid == x.r.u.Facilities.DistrictId)
                                  .Select(district => district.DistrictName).FirstOrDefault() : null,

                         ProvinceName = context.Districts.Join(context.Facilities, Districts => Districts.Oid,

                         Facilities => Facilities.DistrictId, (districts, facilities) => new { districts, facilities })
                                  .Join(context.Provinces, FaclitiesInDistrict => FaclitiesInDistrict.districts.ProvinceId,

                         Provinces => Provinces.Oid, (FacilitiesInDistrict, Provinces) => new { FacilitiesInDistrict, Provinces })
                                  .Where(ProvincesInDistrict => ProvincesInDistrict.FacilitiesInDistrict.districts.Oid == x.r.u.Facilities.DistrictId)
                                  .Count() > 0 ? context.Districts.Join(context.Facilities,

                         Districts => Districts.Oid, Facilities => Facilities.DistrictId, (districts, facilities) => new { districts, facilities })
                                 .Join(context.Provinces, FaclitiesInDistrict => FaclitiesInDistrict.districts.ProvinceId,

                         Provinces => Provinces.Oid, (FacilitiesInDistrict, Provinces) => new { FacilitiesInDistrict, Provinces })
                                 .Where(ProvincesInDistrict => ProvincesInDistrict.FacilitiesInDistrict.districts.Oid == x.r.u.Facilities.DistrictId)
                                 .Select(province => province.Provinces.ProvinceName).FirstOrDefault() : null,

                         Description = x.r.u.Description,
                         StartDate = x.r.u.DateReported,
                         TicketOpenedBy = x.r.u.UserAccounts.Name,
                         UserCellphone = x.r.u.UserAccounts.CountryCode + x.r.u.UserAccounts.Cellphone,
                         UserEmail = x.r.u.UserAccounts.Email,
                         DateOfIncident = x.r.u.DateOfIncident,

                         FirstLevelCategory = x.r.u.FirstLevelCategoryId != null ? context.IncidentCategories
                                 .Where(IncidentCategories => IncidentCategories.Oid == x.r.u.FirstLevelCategoryId)
                                 .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                 .FirstOrDefault() : null,

                         SecondLevelCategory = x.r.u.SecondLevelCategoryId != null ? context.IncidentCategories
                                 .Where(IncidentCategories => IncidentCategories.Oid == x.r.u.SecondLevelCategoryId)
                                 .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                 .FirstOrDefault() : null,

                         ThirdLevelCategory = x.r.u.ThirdLevelCategoryId != null ? context.IncidentCategories
                                 .Where(IncidentCategories => IncidentCategories.Oid == x.r.u.ThirdLevelCategoryId)
                                 .Select(IncidentCategory => IncidentCategory.IncidentCategorys)
                                 .FirstOrDefault() : null,

                         ExpertLeadName = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.UserAccountsTeamLeads.Name)
                                       .FirstOrDefault(),

                         ExpertName = x.r.u.AssignedTo != null ? context.UserAccounts
                                        .Where(UserAccounts => UserAccounts.Oid == x.r.u.AssignedTo)
                                        .Select(userAccount => userAccount.Name)
                                        .FirstOrDefault() : null,

                         AssignedToState = x.r.u.AssignedToState != null ? context.UserAccounts
                             .Where(UserAccount => UserAccount.Oid == x.r.u.AssignedToState)
                             .Select(UserAccount => UserAccount.Name)
                             .FirstOrDefault() : null,

                         AdminName = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.UserAccountAdmins.Name)
                                       .FirstOrDefault(),


                         AgentName = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.UserAccountAgents.Name)
                                       .FirstOrDefault(),

                         OpenBySupervisor = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.SupervisedDateModified)
                                       .FirstOrDefault(),

                         OpenByAgent = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.AgentDateModified)
                                       .FirstOrDefault(),

                         OpenByAdmin = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.AdminDateModified)
                                       .FirstOrDefault(),

                         OpenByExpert = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.ExpertDateModified)
                                       .FirstOrDefault(),

                         OpenByExpertLead = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.TeamLeadDateModified)
                                       .FirstOrDefault(),

                         SupervisorName = context.IncidentActionLogs
                                       .Where(incidentActionLog => incidentActionLog.IncidentId == x.r.u.Oid)
                                       .Select(agent => agent.UserAccountsSuperviseds.Name)
                                       .FirstOrDefault(),

                         ClientName = x.r.uir.Roles.Oid == 1 ? x.r.u.UserAccounts.Name : null,

                         CallerName = x.r.u.CallerName,


                         CallerCellphone = x.r.u.CallerCellphone,

                         CallerEmail = x.r.u.CallerEmail,

                         CallerJobTitle = x.r.u.CallerJobTitle,

                         TicketClosed = x.r.u.DateResolved,

                         Status = x.r.u.IsOpen == true ? "Open" : "Close",

                         ReassignedTo = x.r.u.ReassignedTo != null ? context.UserAccounts
                                .Where(user => user.Oid == x.r.u.ReassignedTo)
                                .Select(reassignedUser => reassignedUser.Name)
                                .FirstOrDefault() : null,

                         ReassignDate = x.r.u.ReassignDate,

                         Priority = x.r.u.PriorityId != null ? context.IncidentPriorities
                            .Where(IncidentPriorities => IncidentPriorities.Oid == x.r.u.PriorityId)
                            .Select(IncidentPriority => IncidentPriority.Priority)
                            .FirstOrDefault() : null

                     }).Skip(start).Take(take).ToList();

            foreach (var ticket in incident)
            {
                if (ticket.TicketClosed != null)
                {
                    TimeSpan? handlingTimeDifference = (ticket.TicketClosed - ticket.StartDate);
                    int handlingTimeDifferenceInSeconds = Convert.ToInt32(handlingTimeDifference.Value.TotalSeconds);
                    HandlingTimeList.Add(handlingTimeDifferenceInSeconds);

                    var hours = (handlingTimeDifferenceInSeconds / 3600) < 10 ? "0" + (handlingTimeDifferenceInSeconds / 3600) : (handlingTimeDifferenceInSeconds / 3600).ToString();
                    var minutes = ((handlingTimeDifferenceInSeconds % 3600) / 60) < 10 ? "0" + ((handlingTimeDifferenceInSeconds % 3600) / 60) : ((handlingTimeDifferenceInSeconds % 3600) / 60).ToString();
                    var seconds = (handlingTimeDifferenceInSeconds % 60) < 10 ? "0" + (handlingTimeDifferenceInSeconds % 60) : (handlingTimeDifferenceInSeconds % 60).ToString();

                    //  ticket.TotalTime = hours + ":" + minutes + ":" + seconds;
                    TimeSpan timeSpan = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), Convert.ToInt32(seconds));

                    // Calculate hours, days, weeks, and months
                    double totalHours = timeSpan.TotalHours;
                    int totalDays = (int)timeSpan.TotalDays;
                    int totalWeeks = totalDays / 7;
                    int totalMonths = (int)(totalDays / 30.44);
                    if (totalMonths > 0)
                    {
                        string monthString = totalMonths == 1 ? "Month" : "Months";
                        ticket.TotalTime = $"{totalMonths} {monthString}";
                    }
                    else if (totalWeeks > 0)
                    {
                        string weekString = totalWeeks == 1 ? "Week" : "Weeks";
                        ticket.TotalTime = $"{totalWeeks} {weekString}";
                    }
                    else if (totalDays>0)
                    {
                        string dayString = totalDays == 1 ? "Day" : "Days";
                        ticket.TotalTime = $"{totalDays} {dayString}";
                    }
                    else
                    {
                        ticket.TotalTime = hours + ":" + minutes + ":" + seconds;
                    }
                }

                DateTime? lastTicketOpenedDate;
                if (ticket.StartDate != null)
                {
                    if (ticket.OpenByExpert != null && DateTime.Compare((DateTime)ticket.OpenByExpert, (DateTime)ticket.StartDate) != 0)
                        lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByExpert ? null : ticket.OpenByExpert;

                    else if (ticket.OpenByExpertLead != null && DateTime.Compare((DateTime)ticket.OpenByExpertLead, (DateTime)ticket.StartDate) != 0)
                        lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByExpertLead ? null : ticket.OpenByExpertLead;

                    else if (ticket.OpenBySupervisor != null && DateTime.Compare((DateTime)ticket.OpenBySupervisor, (DateTime)ticket.StartDate) != 0)
                        lastTicketOpenedDate = DateTime.MinValue == ticket.OpenBySupervisor ? null : ticket.OpenBySupervisor;

                    else if (ticket.OpenByAgent != null && DateTime.Compare((DateTime)ticket.OpenByAgent, (DateTime)ticket.StartDate) != 0)
                        lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByAgent ? null : ticket.OpenByAgent;

                    else if (ticket.OpenByAdmin != null && DateTime.Compare((DateTime)ticket.OpenByAdmin, (DateTime)ticket.StartDate) != 0)
                        lastTicketOpenedDate = DateTime.MinValue == ticket.OpenByAdmin ? null : ticket.OpenByAdmin;

                    else
                    {
                        lastTicketOpenedDate = null;
                    }
                    if (lastTicketOpenedDate != null)
                    {
                        TimeSpan? totalPendingTimeDifference = (lastTicketOpenedDate - ticket.StartDate);

                        var pendingTimeDifferenceInSeconds = Convert.ToInt32(totalPendingTimeDifference.Value.TotalSeconds);
                        var hours = (pendingTimeDifferenceInSeconds / 3600) < 10 ? "0" + (pendingTimeDifferenceInSeconds / 3600) : (pendingTimeDifferenceInSeconds / 3600).ToString();
                        var minutes = ((pendingTimeDifferenceInSeconds % 3600) / 60) < 10 ? "0" + ((pendingTimeDifferenceInSeconds % 3600) / 60) : ((pendingTimeDifferenceInSeconds % 3600) / 60).ToString();
                        var seconds = (pendingTimeDifferenceInSeconds % 60) < 10 ? "0" + (pendingTimeDifferenceInSeconds % 60) : (pendingTimeDifferenceInSeconds % 60).ToString();

                        //ticket.TotalPendingTime = hours + ":" + minutes + ":" + seconds;

                        TimeSpan timeSpan = new TimeSpan(Convert.ToInt32(hours), Convert.ToInt32(minutes), Convert.ToInt32(seconds));

                        // Calculate hours, days, weeks, and months
                        double totalHours = timeSpan.TotalHours;
                        int totalDays = (int)timeSpan.TotalDays;
                        int totalWeeks = totalDays / 7;
                        int totalMonths = (int)(totalDays / 30.44);
                        if (totalMonths > 0)
                        {
                            string monthString = totalMonths == 1 ? "Month" : "Months";
                            ticket.TotalPendingTime = $"{totalMonths} {monthString}";
                        }
                        else if (totalWeeks > 0)
                        {
                            string weekString = totalWeeks == 1 ? "Week" : "Weeks";
                            ticket.TotalPendingTime = $"{totalWeeks} {weekString}";
                        }
                        else if (totalDays>0)
                        {
                            string dayString = totalDays == 1 ? "Day" : "Days";
                            ticket.TotalPendingTime = $"{totalDays} {dayString}";
                        }
                        else
                        {
                            ticket.TotalPendingTime = hours + ":" + minutes + ":" + seconds;
                        }
                    }
                    else
                    {
                        ticket.TotalPendingTime = null;
                    }
                }
            }

            int AvgHandlingTime = HandlingTimeList.Count > 0 ? Convert.ToInt32(HandlingTimeList.Average()) : 0;
            int MinHandlingTime = HandlingTimeList.Count > 0 ? HandlingTimeList.Min() : 0;
            int MaxHandlingTime = HandlingTimeList.Count > 0 ? HandlingTimeList.Max() : 0;

            string AvgHandlingDuration = AvgHandlingTime > 0 ? GetFormattedTime(AvgHandlingTime) : null;
            string MinHandlingDuration = MinHandlingTime > 0 ? GetFormattedTime(MinHandlingTime) : null;
            string MaxHandlingDuration = MaxHandlingTime > 0 ? GetFormattedTime(MaxHandlingTime) : null;

            IncidentLifeCycleListDto incidentLifeCycleListDto = new IncidentLifeCycleListDto()
            {
                TotalIncident = context.Incidents.Where(predicate).Count(),
                CurrentPage = start + 1,
                AvgHandlingDuration = AvgHandlingDuration,
                MinHandlingDuration = MinHandlingDuration,
                MaxHandlingDuration = MaxHandlingDuration,
                List = incident
            };
            return incidentLifeCycleListDto;
        }


        public async Task<IEnumerable<Incident>> GetDeletedExpertIncidents(long Oid)
        {
            try
            {
                return await QueryAsync(Incident => Incident.AssignedTo == Oid && Incident.IsOpen == true && Incident.IsDeleted == false, i => i.DateModified);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentListReturnDto> GetIncidentsByUserName(string UserName, int start, int take, int status)
        {
            try
            {
                Expression<Func<Incident, bool>> predicate = w => w.IsDeleted == false;

                var userId = context.UserAccounts.FirstOrDefault(x => x.Username == UserName)?.Oid;

                if (userId is null)
                {
                    return new IncidentListReturnDto();
                }
                var role = (from u in context.UserAccounts.Where(w => w.Username == UserName)
                            join m in context.Members on u.Oid equals m.UserAccountId into mm
                            from m in mm.DefaultIfEmpty()
                            select new { RoleName = u.Roles.RoleName, u.Oid, TeamId = m == null ? 0 : m.TeamId, Leader = m == null ? false : true })
                            .FirstOrDefault();

                if (userId is not null)
                    predicate = predicate.And(w => w.CreatedBy == userId);

                if (role?.RoleName == "Client")
                    predicate = predicate.And(w => w.ReportedBy == role.Oid);

                else if (role?.RoleName == "Expert")
                {
                    if (role.Leader)
                        predicate = predicate.And(w => w.TeamId == role.TeamId);

                    else
                        predicate = predicate.And(w => w.AssignedTo == role.Oid);

                }
                bool st = false;

                if (status == 0)
                {
                    predicate = predicate.And(i => i.IsDeleted == false);
                }
                else
                {
                    st = status == 1 ? true : false;
                    predicate = predicate.And(i => i.IsDeleted == false && i.IsOpen == st);
                }

                return GetIncidentList(predicate, start, take);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IncidentListReturnDto> GetIncidentsByAssignUserName(string UserName, int start, int take, int status)
        {
            try
            {
                Expression<Func<Incident, bool>> predicate = w => w.IsDeleted == false;

                var role = (from u in context.UserAccounts.Where(w => w.Username == UserName)
                            join m in context.Members on u.Oid equals m.UserAccountId into mm
                            from m in mm.DefaultIfEmpty()
                            select new { RoleName = u.Roles.RoleName, u.Oid, TeamId = m == null ? 0 : m.TeamId, Leader = m == null ? false : true })
                            .FirstOrDefault();



                if (role != null)
                {
                    predicate = predicate.And(w => w.AssignedTo == role.Oid);
                }



                bool st = false;

                if (status == 0)
                    predicate = predicate.And(i => i.IsDeleted == false);
                else
                {
                    st = status == 1 ? true : false;
                    predicate = predicate.And(i => i.IsDeleted == false && i.IsOpen == st);
                }

                return GetIncidentList(predicate, start, take);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IncidentCountDto> IncidentCount(string? UserName)
        {
            try
            {
                var role = (from u in context.UserAccounts.Where(w => w.Username == UserName)
                            join m in context.Members on u.Oid equals m.UserAccountId into mm
                            from m in mm.DefaultIfEmpty()
                            select new { RoleName = u.Roles.RoleName, u.Oid, TeamId = m == null ? 0 : m.TeamId, Leader = m == null ? false :true })
                           .FirstOrDefault();

                Expression<Func<Incident, bool>> TotalIncidentsPredicate = w => w.IsDeleted == false;
                Expression<Func<Incident, bool>> TotalUrgentPredicate = w => w.PriorityId == 1 && w.IsDeleted == false;
                Expression<Func<Incident, bool>> UnresolvedIncidentsPredicate = w => w.IsOpen == true && w.IsDeleted == false;
                Expression<Func<Incident, bool>> ResolvedIncidentsPredicate = w => w.IsOpen == false && w.IsDeleted == false;
                Expression<Func<Incident, bool>> TotalAssignedPredicate = w => ((w.AssignedTo != null && w.IsReassigned == false) || w.ReassignedTo != null) && w.IsDeleted == false;
                Expression<Func<Incident, bool>> TotalUnassignedPredicate = w => w.AssignedTo == null && w.IsDeleted == false;
                Expression<Func<Incident, bool>> TopProvincesByIncidentsPredicate = w => w.IsDeleted == false && w.Facilities.Districts.Provinces.IsDeleted == false;

                if (role?.RoleName == "Expert" && role?.Leader == true)
                {
                    TotalIncidentsPredicate = TotalIncidentsPredicate.And(w => w.TeamId == role.TeamId);
                    TotalUrgentPredicate = TotalUrgentPredicate.And(w => w.TeamId == role.TeamId && w.PriorityId == 1);
                    UnresolvedIncidentsPredicate = UnresolvedIncidentsPredicate.And(w => w.TeamId == role.TeamId);
                    ResolvedIncidentsPredicate = ResolvedIncidentsPredicate.And(w => w.TeamId == role.TeamId);
                    TotalAssignedPredicate = TotalAssignedPredicate.And(w => w.TeamId == role.TeamId);
                    TotalUnassignedPredicate = TotalUnassignedPredicate.And(w => w.TeamId == role.TeamId);
                    TopProvincesByIncidentsPredicate = TopProvincesByIncidentsPredicate.And(w => w.TeamId == role.TeamId);
                }

                if (role?.RoleName == "Expert" && role?.Leader == false)
                {
                    TotalIncidentsPredicate = TotalIncidentsPredicate.And(w => ((w.AssignedTo == role.Oid && w.IsReassigned == false) || w.ReassignedTo == role.Oid));
                    UnresolvedIncidentsPredicate = UnresolvedIncidentsPredicate.And(w => (w.AssignedTo == role.Oid && w.IsReassigned == false) || w.ReassignedTo == role.Oid);
                    ResolvedIncidentsPredicate = ResolvedIncidentsPredicate.And(w => ((w.AssignedTo == role.Oid && w.IsReassigned == false) || w.ReassignedTo == role.Oid));
                    TopProvincesByIncidentsPredicate = TopProvincesByIncidentsPredicate.And(w => ((w.AssignedTo == role.Oid && w.IsReassigned == false) || w.ReassignedTo == role.Oid));
                }

                IncidentCountDto incidentCountDto = new IncidentCountDto();
                incidentCountDto.IncidentInfoPerDays = new List<IncidentInfoPerDay>();
                DateTime? FromDate = DateTime.Today.AddDays(-6);

                incidentCountDto.TotalIncidents = context.Incidents.Where(TotalIncidentsPredicate).Count();
                incidentCountDto.TotalUrgent = context.Incidents.Where(TotalUrgentPredicate).Count();
                incidentCountDto.UnresolvedIncidents = context.Incidents.
                                                     Where(UnresolvedIncidentsPredicate).Count();
                incidentCountDto.ResolvedIncidents = context.Incidents.
                                                     Where(ResolvedIncidentsPredicate).Count();
                incidentCountDto.TotalUsers = context.UserAccounts.Count();
                incidentCountDto.TopProvincesByIncidents = context.Incidents.
                                                     Where(TopProvincesByIncidentsPredicate)
                                                     .GroupBy(i => i.Facilities.Districts.Provinces.Oid).OrderByDescending
                                                     (g => g.Count()).Select(p => new TopProvincesByIncident
                                                     {
                                                         ProviceName = p.Select
                                                     (x => x.Facilities.Districts.Provinces.ProvinceName).
                                                     FirstOrDefault(),
                                                         IncidentCount = p.Count()
                                                     }).Take(5).ToList();
                incidentCountDto.LastMonthTotalTickets = context.Incidents
                            .Where(p => p.DateCreated.Value.Year == DateTime.Now.Year)
                            .GroupBy(p => p.DateCreated.Value.Month)
                            .Select(p => new LastMonthTotalTicket { Month = p.Key == 1 ? "January" : p.Key == 2 ? "February" : p.Key == 3 ? "March" : p.Key == 4 ? "April" : p.Key == 5 ? "May" : p.Key == 6 ? "June" : p.Key == 7 ? "July" : p.Key == 8 ? "August" : p.Key == 9 ? "September" : p.Key == 10 ? "October" : p.Key == 11 ? "November" : "December", Count = p.Count() }).ToList();

                incidentCountDto.LastMonthTotalTickets = incidentCountDto.LastMonthTotalTickets.TakeLast(5).ToList();
                incidentCountDto.TotalAssigned = context.Incidents.Where(TotalAssignedPredicate).Count();
                incidentCountDto.TotalUnassigned = context.Incidents.Where(TotalUnassignedPredicate).Count();
                //incidentCountDto.TotalUrgent = context.Incidents.Where(i => i.PriorityId == 1 && i.IsDeleted == false).Count();

                incidentCountDto.TotalPendingRecoveryRequest = context.RecoveryRequests.Where(r => r.IsRequestOpen == true && r.IsDeleted == false).Count();

                incidentCountDto.TopSystemByIncidents = context.Incidents.
                                                    Where(incident => incident.IsDeleted == false
                                                    && incident.Projects.IsDeleted == false)
                                                    .GroupBy(i => i.SystemId).OrderByDescending
                                                    (g => g.Count()).Select(p => new TopSystemByIncident
                                                    {
                                                        SystemName = p.Select
                                                    (x => x.Projects.Title).
                                                    FirstOrDefault(),
                                                        IncidentCount = p.Count()
                                                    }).Take(5).ToList();

                incidentCountDto.TopTeamByUnresolvedIncidents = context.Incidents.
                                                    Where(incident => incident.IsDeleted == false
                                                    && incident.IsOpen == true
                                                    && incident.Teams.IsDeleted == false)
                                                    .GroupBy(i => i.TeamId).OrderByDescending
                                                    (g => g.Count()).Select(p => new TopTeamByUnresolvedIncident
                                                    {
                                                        TeamName = p.Select
                                                    (x => x.Teams.Title).
                                                    FirstOrDefault(),
                                                        IncidentCount = p.Count()
                                                    }).Take(5).ToList();

                for (int i = 0; i <= 6; i++)
                {
                    DateTime? incidentDate = FromDate.Value.AddDays(i);

                    var incidentInfoPerDay = new IncidentInfoPerDay();

                    incidentInfoPerDay.IncidentDate = incidentDate;
                    Expression<Func<Incident, bool>> TotalOpenIncidentInDayPredicate = w => w.DateOfIncident.Value.Date == incidentDate.Value.Date && w.IsOpen == true && w.IsDeleted == false;
                    Expression<Func<Incident, bool>> TotalClosedIncidentInDayPredicate = w => w.DateOfIncident.Value.Date == incidentDate.Value.Date && w.IsOpen == false && w.IsDeleted == false;

                    if (role?.RoleName == "Expert" && role?.Leader == true)
                    {
                        TotalOpenIncidentInDayPredicate = TotalOpenIncidentInDayPredicate.And(w => w.TeamId == role.TeamId);
                        TotalClosedIncidentInDayPredicate = TotalClosedIncidentInDayPredicate.And(w => w.TeamId == role.TeamId);
                    }

                    if (role?.RoleName == "Expert" && role?.Leader == false)
                    {
                        TotalOpenIncidentInDayPredicate = TotalOpenIncidentInDayPredicate.And(w => ((w.AssignedTo == role.Oid && w.IsReassigned == false) || w.ReassignedTo == role.Oid));
                        TotalClosedIncidentInDayPredicate = TotalClosedIncidentInDayPredicate.And(w => ((w.AssignedTo == role.Oid && w.IsReassigned == false) || w.ReassignedTo == role.Oid));
                    }

                    incidentInfoPerDay.TotalOpenIncident = context.Incidents.Where(TotalOpenIncidentInDayPredicate).Count();
                    incidentInfoPerDay.TotalClosedIncident = context.Incidents.Where(TotalClosedIncidentInDayPredicate).Count();
                    incidentCountDto.IncidentInfoPerDays.Add(incidentInfoPerDay);
                }
                return incidentCountDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetFormattedTime(int timeDuration)
        {
            var hours = (timeDuration / 3600) < 10 ? "0" + (timeDuration / 3600) : (timeDuration / 3600).ToString();
            var minutes = ((timeDuration % 3600) / 60) < 10 ? "0" + ((timeDuration % 3600) / 60) : ((timeDuration % 3600) / 60).ToString();
            var seconds = (timeDuration % 60) < 10 ? "0" + (timeDuration % 60) : (timeDuration % 60).ToString();

            return hours + ":" + minutes + ":" + seconds;
        }
    }
}