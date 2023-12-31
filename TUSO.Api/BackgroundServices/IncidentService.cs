using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;

namespace TUSO.Api.BGService
{
    public class IncidentService
    {
        private readonly ILogger<Incident> _logger;
        private readonly IUnitOfWork context;

        public IncidentService(IUnitOfWork context, ILogger<Incident> logger)
        {
            this.context = context;
            _logger = logger;
        }

        public async Task DoSomethingAsync(int millisecond = 100)
        {
            await Task.Delay(millisecond);
           // await CreateMail();

            _logger.LogInformation(
                "Mail Sending Done.");
        }



       

    }
}