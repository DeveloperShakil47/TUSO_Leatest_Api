/*
* Created by: Labib
* Date created: 31.08.2022
* Last modified: 06.11.2022
* Modified by:Bithy
*/
namespace TUSO.Utilities.Constants
{
    public static class RouteConstants
    {
        public const string BaseRoute = "tuso-api";

        #region Country
        public const string CreateCountry = "country";

        public const string ReadCountries = "countries";

        public const string ReadCountriesbyPage = "countrypage";

        public const string ReadCountryByKey = "country/key/{key}";

        public const string UpdateCountry = "country/{key}";

        public const string DeleteCountry = "country/{key}";
        #endregion

        #region Province
        public const string CreateProvince = "province";

        public const string ReadProvinces = "provinces";

        public const string ReadProvinceByKey = "province/key/{key}";

        public const string ReadProvincesByCountry = "provinces/country/{key}";

        public const string ReadProvinceByCountry = "province/country/{key}";

        public const string UpdateProvince = "province/{key}";

        public const string DeleteProvince = "province/{key}";
        #endregion

        #region Facility
        public const string CreateFacility = "facility";

        public const string ReadFacilities = "facilities";

        public const string ReadFacilityByKey = "facility/key/{key}";

        public const string ReadFacilitiesByDistrict = "facilities/district/{key}";

        public const string ReadFacilityByDistrict = "facility/district/{key}";

        public const string ReadFacilityByName = "facility/name/{name}";

        public const string UpdateFacility = "facility/{key}";

        public const string DeleteFacility = "facility/{key}";
        #endregion

        #region FundingAgency
        public const string CreateFundingAgency = "fundingAgency";

        public const string ReadFundingAgencies = "agencies";

        public const string ReadFundingAgencyByKey = "fundingAgency/key/{key}";

        public const string ReadFundingAgencyBySystem = "fundingAgency/system/{key}";

        public const string UpdateFundingAgency = "fundingAgency/{key}";

        public const string DeleteFundingAgency = "fundingAgency/{key}";
        #endregion

        #region ImplementingPartner
        public const string CreateImplementingPartner = "implementingPartner";

        public const string ReadImplementingPartners = "implementingPartners";

        public const string ReadImplementingPartnerByKey = "implementingPartner/key/{key}";

        public const string ReadImplementingPartnerBySystem = "implementingPartner/system/{key}";

        public const string UpdateImplementingPartner = "implementingPartner/{key}";

        public const string DeleteImplementingPartner = "implementingPartner/{key}";
        #endregion

        #region District
        public const string CreateDistrict = "district";

        public const string ReadDistrict = "districts";

        public const string ReadDistrictByKey = "district/key/{key}";

        public const string ReadDistrictByProvince = "district/province/{key}";

        public const string ReadDistrictsByProvince = "districts/province/{key}";

        public const string UpdateDistrict = "district/{key}";

        public const string DeleteDistrict = "district/{key}";
        #endregion

        #region Configuration
        public const string CreateConfiguration = "configuration";

        public const string ReadConfigurations = "configurations";

        public const string ReadConfigurationByKey = "configuration/key/{key}";

        public const string UpdateConfiguration = "configuration/{key}";

        public const string DeleteConfiguration = "configuration/delete/{key}";
        #endregion

        #region Roles
        public const string CreateUserRole = "user-role";

        public const string ReadUserRoles = "user-roles";

        public const string ReadUserRoleByKey = "user-role/key/{key}";

        public const string UpdateUserRole = "user-role/{key}";

        public const string DeleteUserRole = "user-role/{key}";
        #endregion

        #region UserType
        public const string CreateUserType = "user-type";

        public const string ReadUserTypes = "user-types";

        public const string ReadUserTypeByPage = "user-type-page";

        public const string ReadUserTypeByKey = "user-type/key/{key}";

        public const string UpdateUserType = "user-type/{key}";

        public const string DeleteUserType = "user-type/{key}";
        #endregion

        #region UserAccounts
        public const string CreateUserAccount = "user-account";

        public const string ReadUserAccounts = "user-accounts";

        public const string ReadUserCount = "user-accounts/count";

        public const string ReadUserAccountsByName = "user-accounts/name";

        public const string ReadUserAccountByKey = "user-account/key/{key}";

        public const string ReadClientAccountByKey = "user-account/client/key/{key}";

        public const string ReadUserAccountByRole = "user-account/role/{key}";

        public const string ReadUserAccountByExpert = "user-account/expert";

        public const string UpdateUserAccount = "user-account/{key}";

        public const string DeleteUserAccount = "user-account/{key}";

        public const string IsUniqueUserName = "user-account/unique/{key}";

        public const string IsUniqueCellphone = "user-account/unique-cellphone/{key}";

        public const string UserLogin = "user-account/login";

        public const string ReadUserByUserType = "user-account/usertype/{UsertypeID}";

        public const string ReadUserImage = "user-account/user-image";

        public const string ChangedPassword = "user-account/changepassword";

        public const string RecoveryPassword = "user-account/recovery-password";

        public const string ReadUsersByName = "user-account/search/{name}";
        #endregion

        #region Modules
        public const string CreateModule = "module";

        public const string ReadModules = "modules";

        public const string ReadModuleByKey = "module/key/{key}";

        public const string UpdateModule = "module/{key}";

        public const string DeleteModule = "module/{key}";
        #endregion

        #region IncidentCategory
        public const string CreateIncidentCategory = "incident-category";

        public const string ReadIncidentCategorys = "incident-categorys";

        public const string ReadIncidentCategorybyPage = "incident-categoryPage";

        public const string ReadIncidentCategorysTreeView = "incident-categorystreeview";

        public const string ReadIncidentCategoriesByKey = "incident-categories/key/{key}";

        public const string ReadIncidentCategoriesByPage = "incident-categoriespage/key/{key}";

        public const string ReadIncidentCategoryByKey = "incident-category/key/{key}";

        public const string ReadIncidentCategoryByPage = "incident-categorypage/key/{key}";

        public const string UpdateIncidentCategory = "incident-category/{key}";

        public const string DeleteIncidentCategory = "incident-category/{key}";
        #endregion

        #region IncidentPriority
        public const string CreateIncidentPriority = "incident-priority";

        public const string ReadIncidentPriorities = "incident-priorities";

        public const string ReadIncidentPriorityByKey = "incident-priority/key/{key}";

        public const string UpdateIncidentPriority = "incident-priority/{key}";

        public const string DeleteIncidentPriority = "incident-priority/{key}";
        #endregion

        #region FacilityPermission
        public const string CreateFacilityPermission = "facility-permission";

        public const string ReadFacilityUsers = "facility-users/key/{facilityID}";

        public const string ReadFacilitiesUsers = "facilities-users/key/{facilityID}";

        public const string ReadFacilityPermissionsByKey = "facility-permission/key/{key}";

        public const string ReadFacilityPermissions = "facility-permissions";

        public const string UpdateFacilityPermissions = "facility-permission/{key}";

        public const string DeleteFacilityPermission = "facility-permission/delete/{key}";

        #endregion

        #region Incident
        public const string CreateIncident = "incident";

        public const string ReadIncidents = "incidents";

        public const string ReadIncidentsByStatus = "incidents/status";

        public const string ReadIncidentsByClient = "incidents/client";

        public const string ReadIncidentsByKey = "incidents/key";

        public const string ReadIncidentsByExpert = "incidents/expart";

        public const string ReadIncidentsByExpertLeader = "incidents/expartleader";

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

        public const string ReadIncidentsByAdvancedSearch = "IncidentsList";

        public const string ReadWeeklyIncidentsByAdvancedSearch = "weeklyIncidentList";
        #endregion

        #region IncidentStatus
        public const string CreateIncidentStatus = "incident-status";

        public const string ReadIncidentStatuses = "incident-statuses";

        public const string ReadIncidentStatusByKey = "incident-status/key/{key}";

        public const string UpdateIncidentStatus = "incident-status/{key}";

        public const string DeleteIncidentStatus = "incident-status/{key}";
        #endregion        

        #region System
        public const string CreateSystem = "system";

        public const string ReadSystems = "systems";

        public const string ReadSystemDropdown = "system/drop-down";

        public const string ReadSystemByKey = "system/key/{key}";

        public const string UpdateSystem = "system/{key}";

        public const string DeleteSystem = "system/{key}";
        #endregion

        #region Member
        public const string CreateMember = "member";

        public const string ReadMembers = "members";

        public const string ReadMemberByKey = "member/key/{key}";

        public const string ReadMemberByUser = "member/user/{key}";

        public const string ReadMemberByTeam = "member/team/{key}";

        public const string ReadMembersByTeam = "members/team/{key}";

        public const string UpdateMember = "member/{key}";

        public const string DeleteMember = "member/{key}";
        #endregion

        #region Message
        public const string CreateMessage = "message";

        public const string ReadMessages = "messages";

        public const string ReadMessageByKey = "message/key/{key}";

        public const string UpdateMessage = "message/{key}";

        public const string DeleteMessage = "message/{key}";
        #endregion

        #region ProfilePicture
        public const string CreateProfilePicture = "profile-picture";

        public const string ReadProfilePictures = "profile-pictures";

        public const string ReadProfilePictureByKey = "profile-picture/key/{key}";

        public const string UpdateProfilePicture = "profile-picture/{key}";

        public const string DeleteProfilePicture = "profile-picture/{key}";
        #endregion

        #region ModulePermission
        public const string CreateModulePermission = "module-permission";

        public const string ReadModulePermissions = "module-permissions";

        public const string ReadModulePermissionByKey = "module-permission/key/{key}";

        public const string ReadModulePermissionByRole = "module-permission/role/{RoleID}";

        public const string ReadModulePermissionsByRole = "module-permissions/role/{RoleID}";

        public const string ReadModulePermissionByModule = "module-permission/module/{ModuleID}";

        public const string ReadModulePermission = "module-permission/key";

        public const string UpdateModulePermission = "module-permission/{key}";

        public const string DeleteModulePermission = "module-permission/{key}";
        #endregion

        #region IncidentPermission
        public const string CreateIncidentPermission = "incident-permission";

        public const string ReadIncidentPermissions = "incident-permissions";

        public const string ReadIncidentPermissionByKey = "incident-permission/key/{key}";

        public const string ReadIncidentPermissionByRole = "incident-permission/role/{RoleID}";

        public const string ReadIncidentPermissionByIncidentType = "incident-permission/incidenttype/{IncidentTypeID}";

        public const string ReadIncidentPermission = "incident-permission/key";

        public const string UpdateIncidentPermission = "incident-permission/{key}";

        public const string DeleteIncidentPermission = "incident-permission/{key}";
        #endregion

        #region SystemPermission
        public const string CreateSystemPermission = "system-permission";

        public const string ReadSystemPermissions = "system-permissions";

        public const string ReadSystemPermissionByKey = "system-permission/key/{key}";

        public const string ReadSystemPermissionByUser = "system-permission/user/{UserAccountID}";

        public const string ReadSystemPermissionByUserPage = "system-permission-page/user/{key}";

        public const string ReadSystemPermissionByProject = "system-permission/system/{SystemID}";

        public const string ReadSystemPermission = "system-permission/key";

        public const string UpdateSystemPermission = "system-permission/{key}";

        public const string DeleteSystemPermission = "system-permission/{key}";
        #endregion

        #region Team
        public const string CreateTeam = "team";

        public const string ReadTeams = "teams";

        public const string ReadTeamsbyPage = "teamspage";

        public const string ReadTeamByKey = "team/key/{key}";

        public const string UpdateTeam = "team/{key}";

        public const string DeleteTeam = "team/{key}";
        #endregion

        #region PictureAttachment
        public const string CreateScreenshot = "screenshot/{key}";

        public const string CreateScreenshotMultiple = "screenshot-multiple/{key}";

        public const string ReadScreenshots = "screenshots";

        public const string ReadScreenshotByKey = "screenshot/key/{key}";

        public const string UpdateScreenshot = "screenshot/{key}";

        public const string DeleteScreenshot = "screenshot/{key}";
        #endregion

        #region RecoverRequest
        public const string CreateRecoveryRequest = "recovery-request";

        public const string ReadRecoveryRequests = "recovery-requests";

        public const string ReadRecoveryRequestsByPage = "recovery-requests-page";

        public const string ReadRecoveryRequestByKey = "recovery-request/key/{key}";

        public const string UpdateRecoveryRequest = "recovery-request/{key}";

        public const string DeleteRecoveryRequest = "recovery-request/{key}";

        public const string SearchRecoveryByUserName = "recovery-request/username";

        #endregion

        #region EmailConfiguration
        public const string CreateEmailConfiguration = "emailConfiguration";

        public const string ReadEmailConfigurations = "emailConfigurations";

        public const string ReadEmailConfigurationByKey = "emailConfiguration/key/{key}";

        public const string UpdateEmailConfiguration = "emailConfiguration/{key}";

        public const string DeleteEmailConfiguration = "emailConfiguration/{key}";

        public const string TicketCreationEmail = "emailConfiguration/ticketCreationEmail";

        public const string TicketCloseEmail = "emailConfiguration/ticketCloseEmail";

        #endregion

        #region EmailTemplate
        public const string CreateEmailTemplate = "emailTemplate";

        public const string ReadEmailTemplates = "emailTemplates";

        public const string ReadEmailTemplateByKey = "emailTemplate/key/{key}";

        public const string UpdateEmailTemplate = "emailTemplate/{key}";

        public const string DeleteEmailTemplate = "emailTemplate/{key}";

        #endregion

        #region RDPServerInfo
        public const string RDPLogin = "rdpserver/login";
        #endregion

        #region Sync
        public const string CreateSync = "Sync";
        public const string ReadSyns = "Syncs";
        #endregion

        #region RemoteLogin
        public const string CreateRemoteLoginConcent = "remote-login-concent";
        public const string ReadRemoteLoginConcent = "remote-login-concents";
        public const string ReadRemoteLoginConcentByKey = "remote-login-concent/key/{key}";
        #endregion

        #region RDP
        public const string ReadDevices = "rdp-devices";

        public const string ReadDeviceByKey = "rdp-device/key/{key}";

        public const string GetDeviceActivity = "rdp-device/devices-log";

        public const string ReadDeviceUserByKey = "rdp-device/user/key/{key}";

        public const string UninstallDeviceByKey = "rdp-uninstall-device/key/{deviceID}";
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