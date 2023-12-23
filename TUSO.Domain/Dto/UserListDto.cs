/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    public class UserDto
    {
        public long Oid { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string CountryCode { get; set; }

        public string Cellphone { get; set; }

        public bool IsAccountActive { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int? FacilityId { get; set; }

        public int? DistrictId { get; set; }

        public int? ProvinceId { get; set; }

        public int? DeviceTypeId { get; set; }

        public bool IsUserAlreadyUsed { get; set; }
    }

    public class UserListDto
    {
        public int TotalUser { get; set; }

        public int CurrentPage { get; set; }

        public List<UserDto> List { get; set; }
    }
}