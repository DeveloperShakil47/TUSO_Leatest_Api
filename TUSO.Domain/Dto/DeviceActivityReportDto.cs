namespace TUSO.Domain.Dto
{
    public class DeviceActivityReportDto
    {
        public string DeviceID { get; set; }

        public string DeviceName { get; set; }

        public string OnlineHours { get; set; }

        public string OfflineHours { get; set; }

        public int Sl { get; set; }

        public string UserName { get; set; }

        public string ProvinceName { get; set; }

        public string DistrictName { get; set; }

        public string FacilityName { get; set; }

        public string ItExpertName { get; set; }

        public string UserType { get; set; }

        public string Platform { get; set; }

        public string Processors { get; set; }

        public double UsedStorage { get; set; }

        public string PublicIP { get; set; }

        public string PrivateIP { get; set; }

        public string MacAddress { get; set; }

        public string MotherboardSerial { get; set; }

        public int OnlineHoursDigit { get; set; }

        public int OfflineHoursDigit { get; set; }
    }
}