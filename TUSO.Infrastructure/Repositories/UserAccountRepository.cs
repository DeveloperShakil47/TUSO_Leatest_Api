using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<UserAccount> GetUserAccountByKey(long key)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.Oid == key && u.IsDeleted == false, i => i.Roles);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserListDto> GetUserAccountByRole(int key, int start, int take)
        {
            try
            {
                if (key == 0)
                {
                    return GetUserList(i => i.IsDeleted == false, start, take);
                }
                else
                {
                    return GetUserList(u => u.RoleId == key && u.IsDeleted == false, start, take);
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
                return GetUserList(i => i.IsDeleted == false, start, take);
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
                return GetUserList(i => i.IsDeleted == false && i.Name.Substring(0, length) == name, start, take);
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

        public UserListDto GetUserList(Expression<Func<UserAccount, bool>> predicate, int start, int take)
        {
            var data = (from i in context.UserAccounts.Where(predicate)
                        join f in context.Roles on i.RoleId equals f.Oid
                        select new
                        {
                            i.Oid,
                            i.RoleId,
                            i.Name,
                            i.Surname,
                            i.Email,
                            i.Username,
                            i.Password,
                            i.CountryCode,
                            i.Cellphone,
                            i.IsAccountActive,
                            i.CreatedBy,
                            f.RoleName
                        }).OrderByDescending(o => o.Oid).Skip(start).Take(take).ToList();

            List<UserDto> dto = new List<UserDto>();
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    dto.Add(new UserDto
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
                        IsUserAlreadyUsed = context.Members.FirstOrDefault(x => x.UserAccountId == i.Oid) == null ? false : true
                    });
                }
            }

            UserListDto list = new UserListDto
            {
                List = dto,
                CurrentPage = start + 1,
                TotalUser = context.UserAccounts.Where(predicate).Count()
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

        public async Task<UserDto> GetClientAccountByKey(long userAccountId)
        {
            try
            {
                Expression<Func<UserAccount, bool>> predicate = u => u.Oid == userAccountId && u.IsDeleted == false;

                var userAccount = (from user in context.UserAccounts.Where(predicate)
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
                                       Cellphone = user.Cellphone,
                                       Surname = user.Surname,
                                       Email = user.Email,
                                       Username = user.Username,
                                       Password = user.Password,
                                       IsAccountActive = user.IsAccountActive,
                                       CountryCode = user.CountryCode,
                                       RoleId = user.RoleId, // Fixed property name
                                       FacilityId = facility.Oid,
                                       DistrictId = district.Oid,
                                       ProvinceId = provinces.Oid,
                                       DeviceTypeId = user.DeviceTypeId, // Assuming DeviceTypeId is equivalent to UsertypeID
                                       IsUserAlreadyUsed = context.Members.Any(x => x.UserAccountId == user.Oid) // Simplified the check
                                   }
                        ).FirstOrDefault();

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
