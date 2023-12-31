namespace TUSO.Domain.Dto
{
    public class Drife
    {
        public int driveType { get; set; }

        public string rootDirectory { get; set; }

        public string name { get; set; }

        public string driveFormat { get; set; }

        public int freeSpace { get; set; }

        public int totalSize { get; set; }

        public string volumeLabel { get; set; }
    }

    public class Device
    {
        public string agentVersion { get; set; }

        public object alerts { get; set; }

        public object alias { get; set; }

        public double cpuUtilization { get; set; }

        public string currentUser { get; set; }

        public object deviceGroup { get; set; }

        public string deviceGroupID { get; set; }

        public string deviceName { get; set; }

        public List<Drife> drives { get; set; }

        public string id { get; set; }

        public bool is64Bit { get; set; }

        public bool isOnline { get; set; }

        public string OnlineStatus { get; set; }

        public DateTime lastOnline { get; set; }

        public object notes { get; set; }

        public string organizationID { get; set; }

        public int osArchitecture { get; set; }

        public string osDescription { get; set; }

        public string platform { get; set; }

        public int processorCount { get; set; }

        public string publicIP { get; set; }

        public string serverVerificationToken { get; set; }

        public string tags { get; set; }

        public double totalMemory { get; set; }

        public int totalStorage { get; set; }

        public double usedMemory { get; set; }

        public double usedMemoryPercent { get; set; }

        public int usedStorage { get; set; }

        public double usedStoragePercent { get; set; }

        public int webRtcSetting { get; set; }

        public TimeSpan offlineHours { get; set; }

        public TimeSpan onlineHours { get; set; }

        public int onlineHoursInDigit { get; set; }

        public int offlineHoursInDigit { get; set; }
    }
}