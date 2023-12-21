﻿using TUSO.Domain.Entities;

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

        IFundingAgencyRepository FundingAgencyRepository { get; }

        IImplementingPartnerRepository ImplementingPartnerRepository { get; }
        Task<int> SaveChangesAsync();

    }
}