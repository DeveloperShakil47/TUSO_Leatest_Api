using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IUnitOfWork
    {
        ICountryRepository CountryRepository { get; }

        IProvinceRepository ProvinceRepository { get; }

        IDistrictRepository DistrictRepository { get; }

        IFacilityRepository FacilityRepository { get; }

        IFacilityPermissionRepository FacilityPermissionRepository { get; }

        ISystemRepository SystemRepository { get; }

        ISystemPermissionRepository SystemPermissionRepository { get; }

        ITeamRepository TeamRepository { get; }

        IMemberRepository MemberRepository { get; }

        IIncidentCategoryRepository IncidentCategoryRepository { get; }

        IIncidentPriorityRepository IncidentPriorityRepository { get; }

        IIncidentRepository IncidentRepository { get; }

        ILeadMemberRepository LeadMemberRepository { get; }

        IFundingAgencyRepository FundingAgencyRepository { get; }

        IImplementingPartnerRepository ImplementingPartnerRepository { get; }
        
        IDeviceControlRepository DeviceControlRepository { get; }

        IEmailControlRepository EmailControlRepository { get; }

        IEmailConfigurationRepository EmailConfigurationRepository { get; }

        IEmailTemplateRepository EmailTemplateRepository { get; }

        IRecoveryRequestRepository RecoveryRequestRepository { get; }

        IRoleRepository RoleRepository { get; }

        IUserAccountRepository UserAccountRepository { get; }

        IDeviceTypeRepository DeviceTypeRepository { get; }

        IMessageRepository MessageRepository { get; }

        IIncidentActionLogRepository IncidentActionLogRepository { get; }

        IIncidentAdminActionLogRepository IncidentAdminActionLogRepository { get; }

        IModuleRepository ModuleRepository { get; }

        IFundingAgencyItemRepository FundingAgencyItemRepository { get; }

        IImplementingItemRepository ImplementingItemRepository { get; }

        Task<int> SaveChangesAsync();

    }
}