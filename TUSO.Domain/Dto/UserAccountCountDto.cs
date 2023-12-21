namespace TUSO.Domain.Dto
{
    public class UserAccountCountDto
    {
        public int TotalUser { get; set; }

        public int TotalClientUser { get; set; }

        public int TotalAgentUser { get; set; }

        public int TotalSuperUser { get; set; }

        public int TotalExpertUser { get; set; }

        public int TotalOfflineUser { get; set; }

        public int TotalOnlineUser { get; set; }
    }
}
