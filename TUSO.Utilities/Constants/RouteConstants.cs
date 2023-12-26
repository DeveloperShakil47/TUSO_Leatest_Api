/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Utilities.Constants
{
    public static class RouteConstants
    {
        public const string BaseRoute = "tuso-api";

        #region Country
        public const string CreateCountry = "country";

        public const string ReadCountries = "countries";

        public const string ReadCountriesbyPage = "countries-pagination";

        public const string ReadCountryByKey = "country/key/{key}";

        public const string UpdateCountry = "country/{key}";

        public const string DeleteCountry = "country/{key}";

        #endregion

        #region Province
        public const string CreateProvince = "province";

        public const string ReadProvinces = "provinces";

        public const string ReadProvinceByKey = "province/key/{key}";

        public const string ReadProvinceByCountryPage = "provinces-pagination/country/{key}";

        public const string ReadProvinceByCountry = "province/country/{key}";

        public const string UpdateProvince = "province/{key}";

        public const string DeleteProvince = "province/{key}";
        #endregion

        #region District
        public const string CreateDistrict = "district";

        public const string ReadDistrict = "districts";

        public const string ReadDistrictByKey = "district/key/{key}";

        public const string ReadDistrictByProvince = "district/province/{key}";

        public const string ReadDistrictByProvincePage = "districts-pagination/province/{key}";

        public const string UpdateDistrict = "district/{key}";

        public const string DeleteDistrict = "district/{key}";
        #endregion

        #region Facility
        public const string CreateFacility = "facility";

        public const string ReadFacilities = "facilities";

        public const string ReadFacilityByKey = "facility/key/{key}";

        public const string ReadFacilitieByDistrictPage = "facilities-pagination/district/{key}";

        public const string ReadFacilityByDistrict = "facility/district/{key}";

        public const string ReadFacilityByName = "facility/name/{name}";

        public const string UpdateFacility = "facility/{key}";

        public const string DeleteFacility = "facility/{key}";
        #endregion

        #region FacilityPermission
        public const string CreateFacilityPermission = "facility-permission";

        public const string ReadFacilityPermission = "facility-permission/key/{facilityId}";

        public const string ReadFacilitiePermissionPage = "facilities-permission-pagination/key/{facilityId}";

        public const string ReadFacilityPermissionsByKey = "facility-permission/key/{key}";

        public const string ReadFacilityPermissions = "facility-permissions";

        public const string UpdateFacilityPermissions = "facility-permission/{key}";

        public const string DeleteFacilityPermission = "facility-permission/{key}";

        #endregion

        #region System
        public const string CreateSystem = "system";

        public const string ReadSystems = "systems";

        public const string ReadSystemsPagination = "sysyems-pagination";

        public const string ReadSystemByKey = "system/key/{key}";

        public const string UpdateSystem = "system/{key}";

        public const string DeleteSystem = "system/{key}";
        #endregion

        #region SystemPermission
        public const string CreateSystemPermission = "system-permission";

        public const string ReadSystemPermissions = "system-permissions";

        public const string ReadSystemPermissionByKey = "system-permission/key/{key}";

        public const string ReadSystemPermissionByUser = "system-permission/user/{userAccountId}";

        public const string ReadSystemPermissionByUserPage = "system-permission-pagination/user/{key}";

        public const string ReadSystemPermissionByProject = "system-permission/system/{systemId}";

        public const string ReadSystemPermission = "system-permission/key";

        public const string UpdateSystemPermission = "system-permission/{key}";

        public const string DeleteSystemPermission = "system-permission/{key}";
        #endregion

        #region FundingAgency
        public const string CreateFundingAgency = "funding-agency";

        public const string ReadFundingAgencies = "funding-agencies";

        public const string ReadFundingAgenciesPage = "funding-agencies/pagination";

        public const string ReadFundingAgencyByKey = "funding-agency/key/{key}";

        public const string ReadFundingAgencyBySystem = "funding-agency/system/{key}";

        public const string UpdateFundingAgency = "fundingAgency/{key}";

        public const string DeleteFundingAgency = "fundingAgency/{key}";
        #endregion

        #region ImplementingPartner
        public const string CreateImplementingPartner = "implementing-partner";

        public const string ReadImplementingPartners = "implementing-partners";

        public const string ReadImplementingPartnersPage = "implementing-partners/pagination";

        public const string ReadImplementingPartnerByKey = "implementing-partner/key/{key}";

        public const string ReadImplementingPartnerBySystem = "implementing-partner/system/{key}";

        public const string UpdateImplementingPartner = "implementing-partner/{key}";

        public const string DeleteImplementingPartner = "implementing-partner/{key}";
        #endregion

        #region Roles
        public const string CreateUserRole = "user-role";

        public const string ReadUserRoles = "user-roles";

        public const string ReadUserRolesPage = "user-roles/pagination";

        public const string ReadUserRoleByKey = "user-role/key/{key}";

        public const string UpdateUserRole = "user-role/{key}";

        public const string DeleteUserRole = "user-role/{key}";
        #endregion

        #region UserAccounts
        public const string CreateUserAccount = "user-account";

        public const string ReadUserAccountPage = "user-accounts/pagination";

        public const string ReadUserAccount = "user-accounts/count";

        public const string ReadUserAccountsByName = "user-accounts/name";

        public const string ReadUserAccountByKey = "user-account/key/{key}";


        public const string ReadUserAccountByRole = "user-account/role/{key}";

        public const string ReadUserAccountByExpert = "user-account/expert";

        public const string UpdateUserAccount = "user-account/{key}";

        public const string DeleteUserAccount = "user-account/{key}";

        public const string IsUniqueUserName = "user-account/unique/{key}";

        public const string IsUniqueCellphone = "user-account/unique-cellphone/{key}";

        public const string UserLogin = "user-account/login";

        public const string ReadUserByDeviceType = "user-account/device/{devicetypeId}";

        public const string ReadUserImage = "user-account/user-image";

        public const string ChangedPassword = "user-account/changepassword";

        public const string RecoveryPassword = "user-account/recovery-password";

        public const string ReadUsersByName = "user-account/search/{name}";
        #endregion

        #region ProfilePicture
        public const string CreateProfilePicture = "profile-picture";

        public const string ReadProfilePictures = "profile-pictures";

        public const string ReadProfilePictureByKey = "profile-picture/key/{key}";

        public const string UpdateProfilePicture = "profile-picture/{key}";

        public const string DeleteProfilePicture = "profile-picture/{key}";
        #endregion

        #region DeviceType
        public const string CreateDeviceType = "device-type";

        public const string ReadDeviceTypes = "device-types";

        public const string ReadDeviceTypesPage = "device-types/pagination";

        public const string ReadDeviceTypeByPage = "device-type-page";

        public const string ReadDeviceTypeByKey = "device-type/key/{key}";

        public const string UpdateDeviceType = "device-type/{key}";

        public const string DeleteDeviceType = "device-type/{key}";
        #endregion

        #region DeviceControl
        public const string UpdateDeviceControl = "device-control/{key}";

        public const string ReadDeviceControl = "device-controls";
        #endregion

        #region EmailControl
        public const string CreateEmailControl = "email-control";

        public const string ReadEmailControls = "email-controls";

        public const string ReadEmailControlByKey = "email-control/key/{key}";

        public const string UpdateEmailControl = "email-control/{key}";

        public const string DeleteEmailControl = "email-control/{key}";
        #endregion

        #region EmailConfiguration
        public const string CreateEmailConfiguration = "email-configuration";

        public const string ReadEmailConfigurations = "email-configurations";

        public const string ReadEmailConfigurationByKey = "email-configuration/key/{key}";

        public const string UpdateEmailConfiguration = "email-configuration/{key}";

        public const string DeleteEmailConfiguration = "email-configuration/{key}";

        public const string TicketCreationEmail = "email-configuration/ticket-creationemail";

        public const string TicketCloseEmail = "email-configuration/ticket-closeemail";

        #endregion

        #region EmailTemplate
        public const string CreateEmailTemplate = "emailTemplate";

        public const string ReadEmailTemplates = "emailTemplates";

        public const string ReadEmailTemplateByKey = "emailTemplate/key/{key}";

        public const string UpdateEmailTemplate = "emailTemplate/{key}";

        public const string DeleteEmailTemplate = "emailTemplate/{key}";

        #endregion

        #region RecoverRequest
        public const string CreateRecoveryRequest = "recovery-request";

        public const string ReadRecoveryRequests = "recovery-requests";

        public const string ReadRecoveryRequestsByPage = "recovery-requests/pagination";

        public const string ReadRecoveryRequestByKey = "recovery-request/key/{key}";

        public const string UpdateRecoveryRequest = "recovery-request/{key}";

        public const string DeleteRecoveryRequest = "recovery-request/{key}";

        public const string SearchRecoveryByUserName = "recovery-request/username";

        #endregion

        #region Modules
        public const string CreateModule = "module-option";

        public const string ReadModules = "module-options";

        public const string ReadModulesByPage = "modules-option/pagination";

        public const string ReadModuleByKey = "module-option/key/{key}";

        public const string UpdateModule = "module-option/{key}";

        public const string DeleteModule = "module-option/{key}";
        #endregion

        #region ModulePermission
        public const string CreateModulePermission = "module-permission";

        public const string ReadModulePermissions = "module-permissions";

        public const string ReadModulePermissionByKey = "module-permission/key/{key}";

        public const string ReadModulePermissionByRole = "module-permission/role/{roleId}";

        public const string ReadModulePermissionByRolePage = "module-permissions-pagination/role/{roleId}";

        public const string ReadModulePermissionByModule = "module-permission/module/{moduleId}";

        public const string ReadModulePermission = "module-permission/key";

        public const string UpdateModulePermission = "module-permission/{key}";

        public const string DeleteModulePermission = "module-permission/{key}";
        #endregion

        #region Team
        public const string CreateTeam = "team";

        public const string ReadTeams = "teams";

        public const string ReadTeamsbyPage = "teams/pagination";

        public const string ReadTeamByKey = "team/key/{key}";

        public const string UpdateTeam = "team/{key}";

        public const string DeleteTeam = "team/{key}";
        #endregion

        #region TeamLead
        public const string CreateTeamLead = "team/lead";

        public const string ReadTeamLeads = "team/leads";

        public const string ReadTeamLeadbyPage = "team-lead/pagination";

        public const string ReadTeamLeadByKey = "team-lead/key/{key}";

        public const string UpdateTeamLead = "team-lead/{key}";

        public const string DeleteTeamLead = "team-lead/{key}";
        #endregion

        #region Member
        public const string CreateMember = "member";

        public const string ReadMembers = "members";

        public const string ReadMemberByKey = "member/key/{key}";

        public const string ReadMemberByUser = "member/user/{key}";

        public const string ReadTeamMemberByKey = "member/team/{key}";

        public const string ReadMemberByTeamPage = "members-pagination/team/{key}";

        public const string UpdateMember = "member/{key}";

        public const string DeleteMember = "member/{key}";
        #endregion

        #region Incident
        public const string CreateIncident = "incident";

        public const string ReadIncidents = "incidents";

        public const string ReadIncidentsByStatus = "incidents/status";

        public const string ReadIncidentsByClient = "incidents/client";

        public const string ReadIncidentsByKey = "incidents/key";

        public const string ReadIncidentsByExpert = "incidents/expart";

        public const string ReadIncidentsByExpertLeader = "incidents/expart-leader";

        public const string ReadIncidentsByAgent = "incidents/agent/{key}";

        public const string ReadIncidentsBySearch = "incidents/search";

        public const string ReadIncidentByKey = "incident/key/{key}";

        public const string ReadIncidentsByUserName = "incidents/username";

        public const string GetIncidentsByAssignUserName = "incidents/assign-username";

        public const string ReadIncidentCount = "incidents/incidentCount";

        public const string ReadIncidentClientCount = "incidents/incidentCount/client";

        public const string UpdateIncident = "incident/{key}";

        public const string CloseIncident = "incident/close";

        public const string DeleteIncident = "incident/{key}";

        public const string ReadIncidentsByAdvancedSearch = "Incidents/advanced-search";

        public const string ReadWeeklyIncidentsByAdvancedSearch = "weeklyIncident/weekly-report";
        #endregion

        #region IncidentCategory
        public const string CreateIncidentCategory = "incident-category";

        public const string ReadIncidentCategories = "incident-categories";

        public const string ReadIncidentCategoryByKeyParent = "incident-category/parent/key/{key}";

        public const string ReadIncidentCategoryBySingleKey = "incident-category-single/key/{key}";

        public const string ReadIncidentCategoryPageByFirstLevel = "incident-category/pagination-firstlevel";

        public const string ReadIncidentCategoryPageByLevel = "incident-category/pagination-level/key/{key}";

        public const string UpdateIncidentCategory = "incident-category/{key}";

        public const string DeleteIncidentCategory = "incident-category/{key}";
        #endregion

        #region IncidentPriority
        public const string CreateIncidentPriority = "incident-priority";

        public const string ReadIncidentPriorities = "incident-priorities";

        public const string ReadIncidentPrioritiesPage = "incident-priorities/pagination";

        public const string ReadIncidentPriorityByKey = "incident-priority/key/{key}";

        public const string UpdateIncidentPriority = "incident-priority/{key}";

        public const string DeleteIncidentPriority = "incident-priority/{key}";
        #endregion

        #region Message
        public const string CreateMessage = "message";

        public const string ReadMessages = "messages";

        public const string ReadMessageByKey = "message/key/{key}";

        public const string UpdateMessage = "message/{key}";

        public const string DeleteMessage = "message/{key}";
        #endregion

        #region ScreenshotAttachment
        public const string CreateScreenshot = "screenshot/{key}";

        public const string CreateScreenshotMultiple = "screenshot-multiple/{key}";

        public const string ReadScreenshots = "screenshots";

        public const string ReadScreenshotByKey = "screenshot/key/{key}";

        public const string UpdateScreenshot = "screenshot/{key}";

        public const string DeleteScreenshot = "screenshot/{key}";
        #endregion

        #region RDP
        public const string ReadDevicesType = "rdp-devices";

        public const string ReadDeviceByKey = "rdp-device/key/{key}";

        public const string GetDeviceActivity = "rdp-device/devices-log";

        public const string ReadDeviceUserByKey = "rdp-device/user/key/{key}";

        public const string UninstallDeviceByKey = "rdp-uninstall-device/key/{deviceId}";
        #endregion

        #region RDPServerInfo
        public const string RDPLogin = "rdpserver/login";
        #endregion

        #region RDPLogin
        public const string CreateRemoteLoginConcent = "remote-login-concent";
        public const string ReadRemoteLoginConcent = "remote-login-concents";
        public const string ReadRemoteLoginConcentByKey = "remote-login-concent/key/{key}";
        #endregion

        #region RDPDeviceInfo
        public const string CreateRDPDeviceInfo = "rdp-device-info";

        public const string ReadRDPDeviceInfoes = "rdp-device-infoes";

        public const string ReadRDPDeviceInfoList = "rdp-device-info-list";

        public const string ReadRDPDeviceInfoByKey = "rdp-device-infoes/key/{key}";

        public const string ReadRDPDeviceInfoByName = "rdp-device-info/key/{username}";

        public const string UpdateRDPDeviceInfo = "rdp-device-info/{key}";

        public const string UpdateRDPDeviceInfoByUsername = "rdp-device-info-byusername/{username}";

        public const string DeleteRDPDeviceInfo = "rdp-device-info/{key}";

        public const string DeleteRDPDeviceInfoByUsername = "rdp-device-info-byusername/{username}";

        public const string GetFacilitiesByDeviceId = "rdp-deviceinfobydeviceid/{deviceId}";

        public const string GetFacilitiesByDevice = "rdp-deviceinfoesbydeviceid";
        #endregion
    }
}