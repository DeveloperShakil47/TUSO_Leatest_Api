﻿using TUSO.Domain.Entities;

namespace TUSO.Domain.Dto
{
    public class RDPDeviceInfoDto
    {
        public string UserName { get; set; }

        public string FacilityName { get; set; }

        public string DistrictName { get; set; }

        public string UserTypes { get; set; }

        public string ItExpertName { get; set; }

        public string System { get; set; }

        public string ProviceName { get; set; }

        public string DeviceId { get; set; }

        public string PrivateIp { get; set; }

        public string MACAddress { get; set; }

        public string MotherBoardSerial { get; set; }

        public string PublicIp { get; set; }

        public bool IsDeleted { get; set; }

        public List<RDPDeviceInfo> deviceInfos { get; set; }
    }
}