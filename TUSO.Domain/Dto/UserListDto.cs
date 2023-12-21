/*
 * Created by: Rakib
 * Date created: 27.09.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    public class UserDto
    {
        public long OID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string CountryCode { get; set; }

        public string Cellphone { get; set; }

        public bool IsAccountActive { get; set; }

        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public int? FacilityID { get; set; }

        public int? DistrictID { get; set; }

        public int? ProvinceID { get; set; }

        public int? UsertypeID { get; set; }

        public bool IsUserAlreadyUsed { get; set; }
    }

    public class UserListDto
    {
        public int TotalUser { get; set; }

        public int CurrentPage { get; set; }

        public List<UserDto> List { get; set; }
    }
}