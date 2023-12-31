namespace TUSO.Domain.Dto
{
    public class RdpDeviceActivityDto
    {
        public string DeviceID { get; set; }

        public string DeviceName { get; set; }

        public string OnlineHours { get; set; }

        public string OfflineHours { get; set; }

        public double UsedStorage { get; set; }

        public string Platform { get; set; }

        public string Processor { get; set; }
    }
}