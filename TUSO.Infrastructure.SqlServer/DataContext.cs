using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.SqlServer
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        /// <summary>
        /// Represents Countries entity.
        /// </summary>
        public DbSet<Country> Countries { get; set; }

        /// <summary>
        /// Represents Provinces entity.
        /// </summary>
        public DbSet<Province> Provinces { get; set; }

        /// <summary>
        /// Represents Districts entity.
        /// </summary>
        public DbSet<District> Districts { get; set; }

        /// <summary>
        /// Represents Facilities entity.
        /// </summary>
        public DbSet<Facility> Facilities { get; set; }

        /// <summary>
        /// Represents FacilityPermissions entity.
        /// </summary>
        public DbSet<FacilityPermission> FacilityPermissions { get; set; }

        /// <summary>
        /// Represents Roles entity.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Represents UserAccounts entity.
        /// </summary>
        public DbSet<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Represents UserAccounts entity.
        /// </summary>
        public DbSet<DeviceType> DeviceTypes { get; set; }

        /// <summary>
        /// Represents ProfilePicture entity.
        /// </summary>
        public DbSet<ProfilePicture> ProfilePictures { get; set; }

        /// <summary>
        /// Represents Modules entity.
        /// </summary>
        public DbSet<Module> Modules { get; set; }

        /// <summary>
        /// Represents ModulePermissions entity.
        /// </summary>
        public DbSet<ModulePermission> ModulePermissions { get; set; }

        /// <summary>
        /// Represents Incidents entity.
        /// </summary>
        public DbSet<Incident> Incidents { get; set; }

        /// <summary>
        /// Represents IncidentActionLog entity.
        /// </summary>
        public DbSet<IncidentActionLog> IncidentActionLogs { get; set; }

        /// <summary>
        /// Represents IncidentAdminActionLog entity.
        /// </summary>
        public DbSet<IncidentAdminActionLog> IncidentAdminActionLogs { get; set; }

        /// <summary>
        /// Represents IncidentCategories entity.
        /// </summary>
        public DbSet<IncidentCategory> IncidentCategories { get; set; }

        /// <summary>
        /// Represents IncidentPriorities entity.
        /// </summary>
        public DbSet<IncidentPriority> IncidentPriorities { get; set; }

        /// <summary>
        /// Represents Members entity.
        /// </summary>
        public DbSet<Member> Members { get; set; }

        /// <summary>
        /// Represents Messages entity.
        /// </summary>
        public DbSet<Message> Messages { get; set; }

        /// <summary>
        /// Represents Screenshots entity.
        /// </summary>
        public DbSet<Screenshot> Screenshots { get; set; }

        /// <summary>
        /// Represents Systems entity.
        /// </summary>
        public DbSet<Project> Projects{ get; set; }

        /// <summary>
        /// Represents SystemPermission entity.
        /// </summary>
        public DbSet<SystemPermission> SystemPermissions { get; set; }

        /// <summary>
        /// Represents Projects entity.
        /// </summary>
        public DbSet<Team> Teams { get; set; }

        /// <summary>
        /// Represents RecoveryRequests entity.
        /// </summary>
        public DbSet<RecoveryRequest> RecoveryRequests { get; set; }

        /// <summary>
        /// Represents FundingAgency entity.
        /// </summary>
        public DbSet<FundingAgency> FundingAgencies { get; set; }

        /// <summary>
        /// Represents Implementing Partners entity.
        /// </summary>
        public DbSet<ImplementingPartner> ImplementingPartners { get; set; }


        /// <summary>
        /// Represents RdpServerInfo entity.
        /// </summary>
        public DbSet<RdpServerInfo> RdpServerInfos { get; set; }

        /// <summary>
        /// Represents RDPDeviceInfo entity.
        /// </summary>
        public DbSet<RDPDeviceInfo> RDPDeviceInfoes { get; set; }

        /// <summary>
        /// Represents EmailConfigurations entity.
        /// </summary>
        public DbSet<EmailConfiguration> EmailConfigurations { get; set; }

        /// <summary>
        /// Represents Syncs entity.
        /// </summary>
        public DbSet<DeviceControl> DeviceControls  { get; set; }

        /// <summary>
        /// Represents EmailTemplates entity.
        /// </summary>
        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        /// <summary>
        /// All Configuration Related Entity
        /// </summary>
        public DbSet<EmailControl> EmailControls  { get; set; }

        /// <summary>
        /// Represents RemoteLoginConcents entity.
        /// </summary>
        public DbSet<RemoteLoginConcent> RemoteLoginConcents { get; set; }
        /// <summary>
        /// Represents TeamLeads entity.
        /// </summary>
        public DbSet<TeamLead> TeamLeads { get; set; }

        /// <summary>
        /// Represents FundingAgenciesItems entity.
        /// </summary>
        public DbSet<IncidendtFundingAgency> IncidendtFundingAgencies  { get; set; }

        /// <summary>
        /// Represents ImplemenentingItems entity.
        /// </summary>
        public DbSet<IncidentImplemenentingPartner> incidentImplemenentingPartners { get; set; }
    }
}