namespace TUSO.Domain.Dto
{
    public class FacilitiesPermissionDto
    {
        public int OID { get; set; }

        public int FacilityID { get; set; }

        public string FacilityName { get; set; }

        public long UserID { get; set; }

        public string UserName { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }
    }
}