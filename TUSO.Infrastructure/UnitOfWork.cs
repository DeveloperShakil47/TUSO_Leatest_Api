using Microsoft.Extensions.Options;
using TUSO.Domain.Dto;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.Repositories;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DataContext context;
        private readonly IOptions<RemoteDeviceSettings> _remoteDeviceSettings;
        public UnitOfWork(DataContext context, IOptions<RemoteDeviceSettings> options)
        {
            this.context = context;
            this._remoteDeviceSettings = options;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        #region CountryRepository
        private ICountryRepository countryRepository;
        public ICountryRepository CountryRepository
        {
            get
            {
                if (countryRepository == null)
                    countryRepository = new CountryRepository(context);

                return countryRepository;
            }
        }
        #endregion

        #region ProvinceRepository
        private IProvinceRepository provinceRepository;
        public IProvinceRepository ProvinceRepository
        {
            get
            {
                if (provinceRepository == null)
                    provinceRepository = new ProvinceRepository(context);

                return provinceRepository;
            }
        }
        #endregion

        #region DistrictRepository 
        private IDistrictRepository districtRepository;
        public IDistrictRepository DistrictRepository
        {
            get
            {
                if (districtRepository == null)
                    districtRepository = new DistrictRepository(context);

                return districtRepository;
            }
        }
        #endregion

        #region Facility
        private IFacilityRepository facilityRepository;
        public IFacilityRepository FacilityRepository
        {
            get
            {
                if (facilityRepository == null)
                    facilityRepository = new FacilityRepository(context);

                return facilityRepository;
            }
        }
        #endregion

        #region FacilityPermission
        private IFacilityPermissionRepository facilityPermissionRepository;
        public IFacilityPermissionRepository FacilityPermissionRepository
        {
            get
            {
                if (facilityPermissionRepository == null)
                    facilityPermissionRepository = new FacilityPermissionRepository(context);

                return facilityPermissionRepository;
            }
        }

        #endregion

        #region SystemRepository
        private ISystemRepository systemRepository;
        public ISystemRepository SystemRepository
        {
            get
            {
                if (systemRepository == null)
                    systemRepository = new SystemRepository(context);

                return systemRepository;
            }
        }
        #endregion

        #region FundingAgencyRepository
        private IFundingAgencyRepository fundingAgencyRepository;
        public IFundingAgencyRepository FundingAgencyRepository
        {
            get
            {
                if (fundingAgencyRepository == null)
                    fundingAgencyRepository = new FundingAgencyRepository(context);

                return fundingAgencyRepository;
            }
        }
        #endregion

        #region ImplementingPartnerRepository
        private IImplementingPartnerRepository implementingpartnerRepository;
        public IImplementingPartnerRepository ImplementingPartnerRepository
        {
            get
            {
                if (implementingpartnerRepository == null)
                    implementingpartnerRepository = new ImplementingPartnerRepository(context);

                return implementingpartnerRepository;
            }
        }
        #endregion

        #region SystemPermissionRepository
        private ISystemPermissionRepository systemPermissionRepository;
        public ISystemPermissionRepository SystemPermissionRepository
        {
            get
            {
                if (systemPermissionRepository == null)
                    systemPermissionRepository = new SystemPermissionRepository(context);

                return systemPermissionRepository;
            }
        }
        #endregion

        #region TeamRepository
        private ITeamRepository teamRepository;
        public ITeamRepository TeamRepository
        {
            get
            {
                if (teamRepository == null)
                    teamRepository = new TeamRepository(context);

                return teamRepository;
            }
        }
        #endregion

        #region MemberRepository
        private IMemberRepository memberRepository;
        public IMemberRepository MemberRepository
        {
            get
            {
                if (memberRepository == null)
                    memberRepository = new MemberRepository(context);

                return memberRepository;
            }
        }
        #endregion

        #region LeadMemberRepository
        private ILeadMemberRepository leadMemberRepository;
        public ILeadMemberRepository LeadMemberRepository
        {
            get
            {
                if (leadMemberRepository == null)
                    leadMemberRepository = new LeadMemberRepository(context);

                return leadMemberRepository;
            }
        }
        #endregion

        #region DeviceControlRepository
        private IDeviceControlRepository deviceControlRepository;
        public IDeviceControlRepository DeviceControlRepository 
        {
            get
            {
                if (deviceControlRepository == null)
                    deviceControlRepository = new DeviceControlRepository(context);

                return deviceControlRepository;
            }
        }
        #endregion

        #region EmailControlRepository
        private IEmailControlRepository emailControlRepository;
        public IEmailControlRepository EmailControlRepository
        {
            get
            {
                if (emailControlRepository == null)
                    emailControlRepository = new EmailControlRepository(context);

                return emailControlRepository;
            }
        }
        #endregion

        #region EmailConfigurtionRepository
        private IEmailConfigurationRepository emailConfigurationRepository;
        public IEmailConfigurationRepository EmailConfigurationRepository
        {
            get
            {
                if (emailConfigurationRepository == null)
                    emailConfigurationRepository = new EmailConfigurationRepository(context);

                return emailConfigurationRepository;
            }
        }
        #endregion

        #region EmailTemplateRepository
        private IEmailTemplateRepository emailTemplateRepository;
        public IEmailTemplateRepository EmailTemplateRepository
        {
            get
            {
                if (emailTemplateRepository == null)
                    emailTemplateRepository = new EmailTemplateRepository(context);

                return emailTemplateRepository;
            }
        }
        #endregion

        #region RecoveryRequestRepository
        private IRecoveryRequestRepository recoveryRequest;
        public IRecoveryRequestRepository RecoveryRequestRepository
        {
            get
            {
                if (recoveryRequest == null)
                    recoveryRequest = new RecoveryRequestRepository(context);

                return recoveryRequest;
            }
        }
        #endregion

        #region RoleRepository
        private IRoleRepository roleRepository;
        public IRoleRepository RoleRepository
        {
            get
            {
                if (roleRepository == null)
                    roleRepository = new RoleRepository(context);

                return roleRepository;
            }
        }
        #endregion

        #region ModuleRepository
        private IModuleRepository moduleRepository;
        public IModuleRepository ModuleRepository 
        {
            get
            {
                if (moduleRepository == null)
                    moduleRepository = new ModuleRepository(context);

                return moduleRepository;
            }
        }
        #endregion

        #region UserRepository
        private IUserAccountRepository userAccountRepository ;
        public IUserAccountRepository UserAccountRepository 
        {
            get
            {
                if (userAccountRepository == null)
                    userAccountRepository = new UserAccountRepository(context);

                return userAccountRepository;
            }
        }
        #endregion

        #region UserTypeRepository
        private IDeviceTypeRepository deviceTypeRepository;
        public IDeviceTypeRepository DeviceTypeRepository 
        {
            get
            {
                if (deviceTypeRepository == null)
                    deviceTypeRepository = new DeviceTypeRepository(context);

                return deviceTypeRepository;
            }
        }
        #endregion

        #region IncidentPriorityRepository
        private IIncidentPriorityRepository incidentPriorityRepository;
        public IIncidentPriorityRepository IncidentPriorityRepository
        {
            get
            {
                if(incidentPriorityRepository == null)
                    incidentPriorityRepository = new IncidentPriorityRepository(context);

                return incidentPriorityRepository;
            }
        }
        #endregion

        #region IncidentRepository
        private IIncidentRepository incidentRepository;
        public IIncidentRepository IncidentRepository
        {
            get
            {
                if (incidentRepository == null)
                    incidentRepository = new IncidentRepository(context);
                return incidentRepository;
            }
        }
        #endregion

        #region IncidentCategoryRepository
        private IIncidentCategoryRepository incidentCategoryRepository;
        public IIncidentCategoryRepository IncidentCategoryRepository
        {
            get
            {
                if (incidentCategoryRepository == null)
                    incidentCategoryRepository = new IncidentCategoryRepository(context);
                return incidentCategoryRepository;
            }
        }
        #endregion

        #region IncidentActionLogRepository
        public IIncidentActionLogRepository incidentActionLogRepository;
        public IIncidentActionLogRepository IncidentActionLogRepository
        {
            get
            {
                if (incidentActionLogRepository == null)
                    incidentActionLogRepository = new IncidentActionLogRepository(context);

                return incidentActionLogRepository;
            }

        }

        #endregion

        #region IncidentAdminActionLogRepository
        public IIncidentAdminActionLogRepository incidentAdminActionLogRepository;
        public IIncidentAdminActionLogRepository IncidentAdminActionLogRepository
        {
            get
            {
                if (incidentAdminActionLogRepository == null)
                    incidentAdminActionLogRepository = new IncidentAdminActionLogRepository(context);

                return incidentAdminActionLogRepository;
            }

        }

        #endregion

        #region MessageRepository
        private IMessageRepository messageRepository;
        public IMessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new MessageRepository(context);
                return messageRepository;
            }
        }
        #endregion

        #region FundingAgencyItemRepository
        private IFundingAgencyItemRepository fundingAgencyItemRepository;
        public IFundingAgencyItemRepository FundingAgencyItemRepository
        {
            get
            {
                if (fundingAgencyItemRepository == null)
                    fundingAgencyItemRepository = new FundingAgencyItemRepository(context);
                return fundingAgencyItemRepository;
            }
        }
        #endregion

        #region ImplementingItemRepository
        private IImplementingItemRepository implementingItemRepository;
        public IImplementingItemRepository ImplementingItemRepository
        {
            get
            {
                if (implementingItemRepository == null)
                    implementingItemRepository = new ImplemenentingItemRepository(context);
                return implementingItemRepository;
            }
        }
        #endregion

        #region RDPDeviceInfo
        private IRDPDeviceInfoRepository iRdpDeviceInfoRepository;
        public IRDPDeviceInfoRepository RDPDeviceInfoRepository
        {
            get
            {
                if (iRdpDeviceInfoRepository == null)
                    iRdpDeviceInfoRepository = new RDPDeviceInfoRepository(context);

                return iRdpDeviceInfoRepository;
            }
        }
        #endregion

     
        #region RDP
        private IRDPRepository iRDPRepository;
        public IRDPRepository RDPRepository
        {
            get
            {
                if (iRDPRepository == null)
                    iRDPRepository = new RDPRepository(context, _remoteDeviceSettings);

                return iRDPRepository;
            }
        }
        #endregion
    }
}