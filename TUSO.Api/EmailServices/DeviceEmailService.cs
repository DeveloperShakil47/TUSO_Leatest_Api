//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;
//using TUSO.Domain.Dto;
//using TUSO.Domain.Entities;
//using TUSO.Infrastructure.Contracts;

//namespace TUSO.Api.BGService
//{
//    public class DeviceEmailService
//    {
//        private readonly ILogger<DeviceEmailService> _logger;
//        private readonly IUnitOfWork context;

//        public DeviceEmailService(IUnitOfWork context, ILogger<DeviceEmailService> logger)
//        {
//            this.context = context;
//            _logger = logger;
//        }

//        public async Task DoSomethingAsync(int millisecond = 100)
//        {
//            await Task.Delay(millisecond);
//            await CreateMail();

//            _logger.LogInformation(
//                "Mail Sending Done.");
//        }

//        private async Task CreateMail()
//        {
//            IEnumerable<Device> devices = await context.RDPRepository.GetRootobject();
//            Sync syncs = await context.SyncRepository.FirstOrDefaultAsync(x => x.IsDeleted == false);

//            List<Device> offlineDevices = new();
//            List<Device> unhealthyDevices = new();


//            List<UserAccount> offlineUsers = new();
//            List<UserAccount> unHealthyUsers = new();

//            foreach (var device in devices)
//            {
//                if (device.isOnline == false)
//                {
//                    offlineDevices.Add(device);

//                    var userWithDevice = await context.RDPDeviceInfoRepository.GetByDeviceId(device.id);

//                    if (!string.IsNullOrEmpty(userWithDevice?.UserName))
//                    {
//                        var tusoUser = await context.UserAccountRepository.GetUserAccountByName(userWithDevice.UserName);

//                        if (tusoUser is not null)
//                            offlineUsers.Add(tusoUser);
//                    }
//                }

//                if (syncs is not null)
//                {
//                    decimal deviceMemoryPercent = (decimal)device.usedMemoryPercent * 100;
//                    decimal deciceCPUPercent = (decimal)(device.cpuUtilization * 100);

//                    if (deviceMemoryPercent > syncs?.MemoryUses || deciceCPUPercent > syncs?.CPUUses)
//                    {
//                        unhealthyDevices.Add(device);

//                        var userWithDevice = await context.RDPDeviceInfoRepository.GetByDeviceId(device.id);

//                        if (!string.IsNullOrEmpty(userWithDevice?.UserName))
//                        {
//                            var tusoUser = await context.UserAccountRepository.GetUserAccountByName(userWithDevice.UserName);

//                            if (tusoUser is not null)
//                                unHealthyUsers.Add(tusoUser);
//                        }
//                    }
//                }

//            }

//            var userEmailTemplateForOfflineDevice = await context.EmailTemplateRepository.GetEmailTemplateByBodyType(4);
//            var userEmailTemplateForUnhealthyDevice = await context.EmailTemplateRepository.GetEmailTemplateByBodyType(5);

//            if (userEmailTemplateForOfflineDevice != null)
//            {
//                foreach (var user in offlineUsers)
//                {
//                    if (user.Email is not null)
//                    {
//                        string? itExpertMail = string.Empty;
//                        if (user.FacilityID is not null)
//                        {
//                            itExpertMail = context.FacilityPermissionRepository.GetFacilityPermissionByKey((int)user.FacilityID).Result.User?.Email;

//                        }

//                        await MailTemplateConfiguration(userEmailTemplateForOfflineDevice, "\\EmailTemplate\\Deviceofflinemailtemplate.htm", user, itExpertMail);
//                    }
//                }
//            }

//            if (userEmailTemplateForUnhealthyDevice != null)
//            {
//                foreach (var user in unHealthyUsers)
//                {
//                    if (user.Email is not null)
//                    {
//                        string? itExpertMail = string.Empty;

//                        if (user.FacilityID is not null)
//                        {
//                            itExpertMail = context.FacilityPermissionRepository.GetFacilityPermissionByKey((int)user.FacilityID).Result.User?.Email;

//                        }
//                        await MailTemplateConfiguration(userEmailTemplateForUnhealthyDevice, "\\EmailTemplate\\Deviceunhealthymailtemplate.htm", user, itExpertMail);
//                    }
//                }
//            }
//        }

//        private async Task MailTemplateConfiguration(EmailTemplate userEmailTemplateData, string templateUrl, UserAccount user, string? itExpertEmailAddress)
//        {
//            EmailModelDto emailModel = new EmailModelDto();

//            userEmailTemplateData.Subject = userEmailTemplateData.Subject;
//            emailModel.Subject = userEmailTemplateData.Subject;

//            string FilePath = Directory.GetCurrentDirectory() + templateUrl;
//            StreamReader str = new StreamReader(FilePath);
//            string MailText = str.ReadToEnd();
//            str.Close();

//            MailText = MailText.Replace("[MailBody]", userEmailTemplateData.MailBody);
//            MailText = MailText.Replace("[Client_Name]", user.Username);

//            var builder = new BodyBuilder();

//            builder.HtmlBody = MailText;
//            emailModel.Body = builder.ToMessageBody();
//            emailModel.ToMail = user.Email;

//            if (itExpertEmailAddress is not null && itExpertEmailAddress.Length > 0)
//            {
//                emailModel.Bcc.Add(itExpertEmailAddress);
//            }

//            await SendEmail(emailModel);
//        }

//        private async Task SendEmail(EmailModelDto emailModel)
//        {
//            try
//            {
//                var emailConfigurations = await context.EmailConfigurationRepository.GetEmailConfigurations();
//                var emailConfiguration = emailConfigurations.FirstOrDefault();

//                if (emailConfiguration != null)
//                {
//                    string smail = emailConfiguration.EmailAddress;
//                    string authToken = emailConfiguration.Password;

//                    var email = new MimeMessage();

//                    email.From.Add(new MailboxAddress("TUSO", smail));
//                    email.To.Add(MailboxAddress.Parse(emailModel.ToMail));
//                    email.Subject = emailModel.Subject;
//                    email.Body = emailModel.Body;

//                    if (emailConfiguration.Auditmails is not null)
//                    {
//                        emailConfiguration.Auditmails.Split(",").ToList().ForEach(x => email.Bcc.Add(MailboxAddress.Parse(x)));
//                    }
//                    if (emailModel.Bcc.Count() > 0)
//                    {
//                        emailModel.Bcc.ForEach(x => email.Bcc.Add(MailboxAddress.Parse(x)));
//                    }

//                    var fromEmailPassword = authToken;
//                    var smtp = new SmtpClient();

//                    smtp.Connect(emailConfiguration.SMTPServer, Convert.ToInt32(emailConfiguration.Port), SecureSocketOptions.StartTls);
//                    smtp.Authenticate(smail, fromEmailPassword);

//                    await smtp.SendAsync(email);
//                    smtp.Disconnect(true);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("Smtp gmail error occurred:" + ex.ToString());
//                throw;
//            }
//        }
//    }
//}