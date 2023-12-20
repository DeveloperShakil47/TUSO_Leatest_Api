using TUSO.Domain.Entities;

/*
 * Created by: Labib
 * Date created: 31.08.2022
 * Last modified: 06.11.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IUnitOfWork
    {
        ICountryRepository CountryRepository { get; }
        IFacilityRepository FacilityRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IModulePermissionRepository ModulePermissionRepository { get; }
        ISystemPermissionRepository SystemPermissionRepository { get; }
        ISystemRepository SystemRepository { get; }
        IMemberRepository MemberRepository { get; }
        IMessageRepository MessageRepository { get; }
        ITeamRepository TeamRepository { get; }
        IScreenshotRepository ScreenshotRepository { get; }
        ISyncRepository SyncRepository { get; }
        IEmailConfigurationRepository EmailConfigurationRepository { get; }
        IEmailTemplateRepository EmailTemplateRepository { get; }
        Task<int> SaveChangesAsync();
    }
}