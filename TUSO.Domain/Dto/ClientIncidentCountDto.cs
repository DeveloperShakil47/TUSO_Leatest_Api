namespace TUSO.Domain.Dto
{
    public class ClientIncidentCountDto
    {
        public int TotalTickets { get; set; }

        public int TotalCloseTickets { get; set; }

        public int TotalOpenTickets { get; set; }

        public List<LastMonthTotalTicket> LastMonthTotalTickets { get; set; }
    }

    public class LastMonthTotalTicket
    {
        public string Month { get; set; }

        public int Count { get; set; }
    }
}