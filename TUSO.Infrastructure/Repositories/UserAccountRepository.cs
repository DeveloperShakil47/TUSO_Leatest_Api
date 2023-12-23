using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

namespace TUSO.Infrastructure.Repositories
{
    public class UserAccountRepository : Repository<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(DataContext context) : base(context)
        {

        }

        public async Task<UserAccount?> GetUserAccountByKey(long key)
        {
     
            return await context.UserAccounts
                .Include(u => u.Roles) 
                .FirstOrDefaultAsync(u => u.Oid == key && !u.IsDeleted);
        }

        public async Task<UserListDto> GetUserAccountByRole(int key, int start, int take)
        {
            try
            {
                if (key == 0)
                {
                    return await GetUserList(i => i.IsDeleted == false, start, take);
                }
                else
                {
                    return await GetUserList(u => u.RoleId == key && u.IsDeleted == false, start, take);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccountByExpert()
        {
            try
            {
                return await QueryAsync(u => u.Roles.RoleName == "Expert" && u.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserAccount> GetUserAccountByName(string name)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.Username.ToLower().Trim() == name.ToLower().Trim() && u.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserAccount>> GetUserAccountByFullName(string name)
        {
            try
            {
                return await context.UserAccounts.Where(u => u.Name.ToLower().Trim().Contains(name.ToLower().Trim()) && u.RoleId != 1 && u.IsDeleted == false).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserAccount> GetUserAccountByCellphone(string cellphone)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.Cellphone.Trim() == cellphone.Trim() && u.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UserAccountCountDto> UserAccountCount()
        {
            try
            {
                UserAccountCountDto userAccountCount = new UserAccountCountDto();

                userAccountCount.TotalUser = await context.UserAccounts.CountAsync(x => !x.IsDeleted);
                userAccountCount.TotalClientUser = await context.UserAccounts.CountAsync(x => x.RoleId == 1 && !x.IsDeleted);
                userAccountCount.TotalAgentUser = await context.UserAccounts.CountAsync(x => x.RoleId == 2 && !x.IsDeleted);
                userAccountCount.TotalSuperUser = await context.UserAccounts.CountAsync(x => x.RoleId == 3 && !x.IsDeleted);
                userAccountCount.TotalExpertUser = await context.UserAccounts.CountAsync(x => x.RoleId == 4 && !x.IsDeleted);

                return userAccountCount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserListDto> GetUsers(int start, int take)
        {
            try
            {
                return await GetUserList(i => i.IsDeleted == false, start, take);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserListDto> GetUsersByName(string name, int start, int take)
        {
            try
            {
                var length = name.Length;
                return await GetUserList(i => i.IsDeleted == false && i.Name.Substring(0, length) == name, start, take);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserAccount> GetUserByUserNamePassword(string UserName, string Password)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.Username == UserName && u.Password == Password && u.IsDeleted == false, i => i.Roles);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccountBydevice(string username)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.Username == username);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserListDto> GetUserList(Expression<Func<UserAccount, bool>> predicate, int start, int take)
        {
            var data = context.UserAccounts
                .Where(predicate)
                .OrderByDescending(o => o.Oid)
                .Skip(start)
                .Take(take)
                .Join(context.Roles,
                    u => u.RoleId,
                    r => r.Oid,
                    (u, r) => new
                    {
                        u.Oid,
                        u.RoleId,
                        u.Name,
                        u.Surname,
                        u.Email,
                        u.Username,
                        u.Password,
                        u.CountryCode,
                        u.Cellphone,
                        u.IsAccountActive,
                        u.CreatedBy,
                        RoleName = r.RoleName
                    })
                .ToList();

            var dto = data.Select(i => new UserDto
            {
                Oid = i.Oid,
                Name = i.Name,
                Surname = i.Surname,
                Email = i.Email,
                Username = i.Username,
                Password = i.Password,
                CountryCode = i.CountryCode,
                Cellphone = i.Cellphone,
                IsAccountActive = i.IsAccountActive,
                RoleId = i.RoleId,
                RoleName = i.RoleName,
                IsUserAlreadyUsed = context.Members.Any(x => x.UserAccountId == i.Oid)
            }).ToList();

            var list = new UserListDto
            {
                List = dto,
                CurrentPage = start + 1,
                TotalUser = await context.UserAccounts.CountAsync(predicate)
            };

            return list;
        }



        public async Task<UserAccount?> GetUserByUsernameCellPhone(string Cellphone, string Username, string CountryCode)
        {
            if (string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Cellphone) && !string.IsNullOrEmpty(CountryCode))
            {
                return await context.UserAccounts.FirstOrDefaultAsync(x => x.Cellphone == Cellphone && x.CountryCode == CountryCode && !x.IsDeleted);
            }
            else if (string.IsNullOrEmpty(Cellphone) || string.IsNullOrEmpty(CountryCode))
            {
                return await context.UserAccounts.FirstOrDefaultAsync(x => x.Username == Username && !x.IsDeleted);
            }
            else if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Cellphone) && !string.IsNullOrEmpty(CountryCode))
            {
                return await context.UserAccounts.FirstOrDefaultAsync(x => x.Cellphone == Cellphone && x.Username == Username && x.CountryCode == CountryCode && !x.IsDeleted);
            }

            return null;
        }

        public async Task<int> TotalOpenTicketUnderClient(long OID)
        {
            try
            {
                return await context.Incidents.CountAsync(c => c.ReportedBy == OID && c.UserAccounts.RoleId == 1 && c.IsOpen == true && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserAccount>> GetAdminUser()
        {
            try
            {
                return await context.UserAccounts.Where(c => c.RoleId == 5 && c.IsDeleted == false).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> IsTeamLeader(long OID)
        {
            try
            {
                var result = await context.TeamLeads.FirstOrDefaultAsync(c => c.UserAccountId == OID  && c.IsDeleted == false);
                if (result != null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto?> GetClientAccountByKey(long userAccountId)
        {
            try
            {
                var userAccount = await (
                    from user in context.UserAccounts
                             .Where(u => u.Oid == userAccountId && !u.IsDeleted)
                    join usertype in context.DeviceTypes on user.DeviceTypeId equals usertype.Oid into userTypeInfo
                    from usertype in userTypeInfo.DefaultIfEmpty()
                    join userFacility in context.FacilityPermissions on user.Oid equals userFacility.UserId into facilityInfo
                    from userFacility in facilityInfo.DefaultIfEmpty()
                    join facility in context.Facilities on userFacility.FacilityId equals facility.Oid into facilities
                    from facility in facilities.DefaultIfEmpty()
                    join district in context.Districts on facility.DistrictId equals district.Oid into districtInfo
                    from district in districtInfo.DefaultIfEmpty()
                    join provinces in context.Provinces on district.ProvinceId equals provinces.Oid into provincesInfo
                    from provinces in provincesInfo.DefaultIfEmpty()
                    select new UserDto
                    {
                        Oid = user.Oid,
                        Name = user.Name,
                        Surname = user.Surname,
                        Cellphone = user.Cellphone,
                        Email = user.Email,
                        Username = user.Username,
                        Password = user.Password,
                        IsAccountActive = user.IsAccountActive,
                        CountryCode = user.CountryCode,
                        RoleId = user.RoleId, // consistent naming
                        FacilityId = facility.Oid,
                        DistrictId = district.Oid,
                        ProvinceId = provinces.Oid,
                        DeviceTypeId = user.DeviceTypeId, // consistent naming
                        IsUserAlreadyUsed = context.Members.Any(x => x.UserAccountId == user.Oid)
                    }
                ).FirstOrDefaultAsync();

                return userAccount;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<UserAccount>> GetUserByUsertype(int UsertypeID)
        {
            try
            {
                return await QueryAsync(u => u.DeviceTypeId == UsertypeID && u.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
