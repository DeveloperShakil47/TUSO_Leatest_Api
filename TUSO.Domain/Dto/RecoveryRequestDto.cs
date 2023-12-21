/*
 * Created by: Bithy
 * Date created: 03.11.2022
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Domain.Dto
{
    public class RecoveryRequestDto
    {
        public string CellPhone { get; set; }

        public string? UserName { get; set; }

        public string CountryCode { get; set; }
    }
}