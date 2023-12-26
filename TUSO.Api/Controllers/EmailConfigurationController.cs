using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 28.02.2023
 * Last modified: 31.10.2023
 * Modified by: Stephan
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///EmailConfiguration Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class EmailConfigurationController : ControllerBase
    {
        private readonly IUnitOfWork context;
        private readonly ILogger<EmailConfigurationController> logger;
        private readonly IConfiguration config;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public EmailConfigurationController(IUnitOfWork context, ILogger<EmailConfigurationController> logger, IConfiguration config)
        {
            this.context = context;
            this.logger = logger;
            this.config = config;
        }

        /// <summary>
        /// URL: tuso-api/emailConfiguration
        /// </summary>
        /// <param name="emailConfiguration">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateEmailConfiguration)]
        public async Task<ResponseDto> CreateEmailConfiguration(EmailConfiguration emailConfiguration)
        {
            try
            {
                emailConfiguration.DateCreated = DateTime.Now;
                emailConfiguration.IsDeleted = false;

                context.EmailConfigurationRepository.Add(emailConfiguration);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", emailConfiguration);


            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailConfigurations
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailConfigurations)]
        public async Task<ResponseDto> ReadEmailConfigurations()
        {
            try
            {
                var emailConfigurations = await context.EmailConfigurationRepository.GetEmailConfigurations();

                return new ResponseDto(HttpStatusCode.OK, true, emailConfigurations == null ? "Data Not Found" : "Successfully Get All Data", emailConfigurations);

            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailConfiguration/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table EmailConfigurations</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadEmailConfigurationByKey)]
        public async Task<ResponseDto> ReadEmailConfigurationByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError);

                var emailConfiguration = await context.EmailConfigurationRepository.GetEmailConfigurationByKey(key);

                if (emailConfiguration == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError);

                return new ResponseDto(HttpStatusCode.OK, true, "Successfully Get by Key",emailConfiguration);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailConfiguration/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="emailConfiguration">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateEmailConfiguration)]
        public async Task<ResponseDto> UpdateEmailConfiguration(int key, EmailConfiguration emailConfiguration)
        {
            try
            {
                if (key != emailConfiguration.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError,null);

                emailConfiguration.DateModified = DateTime.Now;

                context.EmailConfigurationRepository.Update(emailConfiguration);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK,true,"Email Update Successfully", emailConfiguration);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailConfiguration/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteEmailConfiguration)]
        public async Task<IActionResult> DeleteEmailConfiguration(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var emailConfigurationInDb = await context.EmailConfigurationRepository.GetEmailConfigurationByKey(key);

                if (emailConfigurationInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                emailConfigurationInDb.IsDeleted = true;
                emailConfigurationInDb.DateModified = DateTime.Now;

                context.EmailConfigurationRepository.Update(emailConfigurationInDb);
                await context.SaveChangesAsync();

                return Ok(emailConfigurationInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailConfiguration/ticketCreationEmail
        /// </summary>
        /// <param name="incident">A data row of incident table</param>
        /// <returns>Sends an email to caller or user on ticket creation.</returns>
        [HttpPost]
        [Route(RouteConstants.TicketCreationEmail)]
        public async Task<ResponseDto> SendTicketCreationEmail(Incident incident)
        {
            try
            {
                bool isEmailSend = context.EmailControlRepository.GetEmailControlByKey(1).Result.IsEmailSendForIncidentCreate; 

                if(!isEmailSend)
                    return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError,null);

                if (!string.IsNullOrEmpty(incident.CallerEmail))
                {
                    EmailModelDto emailModel = new EmailModelDto();

                    var callerEmailTemplateData = await context.EmailTemplateRepository.GetEmailTemplateByBodyType(3);

                    if (callerEmailTemplateData == null)
                        return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                    callerEmailTemplateData.Subject = callerEmailTemplateData.Subject.Replace("[TicketNo]", incident.Oid.ToString());
                    emailModel.Subject = callerEmailTemplateData.Subject;

                    string FilePath = Directory.GetCurrentDirectory() + "\\EmailTemplate\\Callermailtemplate.htm";
                    StreamReader str = new(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    MailText = MailText.Replace("[MailBody]", callerEmailTemplateData.MailBody);
                    DateTime incidentDate = (DateTime)incident.DateCreated;

                    MailText = MailText.Replace("[Username]", incident.CallerName).Replace("[TicketNo]", incident.Oid.ToString()).Replace("[TicketTitle]", incident.TicketTitle).Replace("[createdDate]", incidentDate.Day + " of " + incidentDate.ToString("MMMM") + " " + incidentDate.Year);

                    var builder = new BodyBuilder();

                    builder.HtmlBody = MailText;
                    emailModel.Body = builder.ToMessageBody();
                    emailModel.ToMail = incident.CallerEmail;

                    await SendEmail(emailModel);
                }

                var user = await context.UserAccountRepository.GetUserAccountByKey(incident.ReportedBy);

                if (user == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    var userEmailTemplateData = await context.EmailTemplateRepository.GetEmailTemplateByBodyType(1);

                    if (userEmailTemplateData == null)
                        return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                    EmailModelDto emailModel = new EmailModelDto();

                    userEmailTemplateData.Subject = userEmailTemplateData.Subject.Replace("[TicketNo]", incident.Oid.ToString());
                    emailModel.Subject = userEmailTemplateData.Subject;

                    string FilePath = Directory.GetCurrentDirectory() + "\\EmailTemplate\\Usermailtemplate.htm";
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    MailText = MailText.Replace("[MailBody]", userEmailTemplateData.MailBody);
                    DateTime incidentDate = (DateTime)incident.DateCreated;
                    MailText = MailText.Replace("[Username]", user.Username).Replace("[TicketNo]", incident.Oid.ToString()).Replace("[TicketTitle]", incident.TicketTitle).Replace("[createdDate]", incidentDate.Day + " of " + incidentDate.ToString("MMMM") + " " + incidentDate.Year);

                    var builder = new BodyBuilder();

                    builder.HtmlBody = MailText;

                    emailModel.Body = builder.ToMessageBody();
                    emailModel.ToMail = user.Email;

                    await SendEmail(emailModel);
                }

                return new ResponseDto(HttpStatusCode.OK, true, "Ticket Creation Email Successfully Send", null);
            }
            catch (Exception ex)
            {
                logger.LogError("Smtp gmail error occurred:" + ex.ToString());
                logger.LogError("Smtp gmail error occurred:" + ex.Message.ToString());

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/emailConfiguration/ticketCloseEmail
        /// </summary>
        /// <param name="key">Primary key of incident table</param>
        /// <returns>Sends an email to user on ticket close.</returns>
        [HttpPost]
        [Route(RouteConstants.TicketCloseEmail)]
        public async Task<ResponseDto> SendTicketCloseEmail(long key)
        {
            try
            {
                bool isEmailSend = context.EmailControlRepository.GetEmailControlByKey(1).Result.IsEmailSendForIncidentClose;

                if (!isEmailSend)
                    return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);

                var incident = await context.IncidentRepository.GetIncidentDataByKey(key);

                if (incident == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                List<UserAccount> users = await context.UserAccountRepository.GetAdminUser();
                List<string> adminUserMail = new List<string>();

                users.ForEach(x => adminUserMail.Add(x.Email));

                if (!string.IsNullOrEmpty(incident.CallerEmail))
                {
                    var callerTicketCloseEmailTemplateData = await context.EmailTemplateRepository.GetEmailTemplateByBodyType(2);

                    if (callerTicketCloseEmailTemplateData == null)
                        return new ResponseDto(HttpStatusCode.NotFound,false, MessageConstants.NoMatchFoundError, null);

                    EmailModelDto emailModel = new();

                    callerTicketCloseEmailTemplateData.Subject = callerTicketCloseEmailTemplateData.Subject.Replace("[TicketNo]", incident.Oid.ToString());

                    emailModel.Subject = callerTicketCloseEmailTemplateData.Subject;
                    string FilePath = Directory.GetCurrentDirectory() + "\\EmailTemplate\\Closeticketmailtemplate.htm";
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    MailText = MailText.Replace("[MailBody]", callerTicketCloseEmailTemplateData.MailBody);
                    MailText = MailText.Replace("[Username]", incident.CallerName).Replace("[TicketNo]", incident.Oid.ToString()).Replace("[TicketTitle]", incident.TicketTitle);

                    var builder = new BodyBuilder();

                    builder.HtmlBody = MailText;

                    emailModel.Body = builder.ToMessageBody();
                    emailModel.ToMail = incident.CallerEmail;
                    emailModel.Bcc = adminUserMail;

                    await SendEmail(emailModel);
                }

                var user = await context.UserAccountRepository.GetUserAccountByKey(incident.ReportedBy);

                if (user == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                if (!string.IsNullOrEmpty(user.Email))
                {
                    var userTicketCloseEmailTemplateData = await context.EmailTemplateRepository.GetEmailTemplateByBodyType(2);

                    if (userTicketCloseEmailTemplateData == null)
                        return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                    EmailModelDto emailModel = new();

                    userTicketCloseEmailTemplateData.Subject = userTicketCloseEmailTemplateData.Subject.Replace("[TicketNo]", incident.Oid.ToString());

                    emailModel.Subject = userTicketCloseEmailTemplateData.Subject;
                    string FilePath = Directory.GetCurrentDirectory() + "\\EmailTemplate\\Closeticketmailtemplate.htm";
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    MailText = MailText.Replace("[MailBody]", userTicketCloseEmailTemplateData.MailBody);
                    MailText = MailText.Replace("[Username]", user.Username).Replace("[TicketNo]", incident.Oid.ToString()).Replace("[TicketTitle]", incident.TicketTitle);

                    var builder = new BodyBuilder();

                    builder.HtmlBody = MailText;

                    emailModel.Body = builder.ToMessageBody();
                    emailModel.ToMail = user.Email;
                    emailModel.Bcc = adminUserMail;

                    await SendEmail(emailModel);
                }

                return new ResponseDto(HttpStatusCode.OK, true, "Ticket Clossing Mail Send Successfully", null);
            }
            catch (Exception ex)
            {
                logger.LogError("Smtp gmail error occurred:" + ex.ToString());
                logger.LogError("Smtp gmail error occurred:" + ex.Message.ToString());
                return new ResponseDto(HttpStatusCode.InternalServerError,false, MessageConstants.GenericError,null);
            }
        }

        private async Task<ResponseDto> SendEmail(EmailModelDto emailModel)
        {
            try
            {
                var emailConfigurations = await context.EmailConfigurationRepository.GetEmailConfigurations();
                var emailConfiguration = emailConfigurations.FirstOrDefault();

                if (emailConfiguration == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                string smail = emailConfiguration.EmailAddress;
                string authToken = emailConfiguration.Password;

                var email = new MimeMessage();

                email.From.Add(new MailboxAddress("TUSO", smail));
                email.To.Add(MailboxAddress.Parse(emailModel.ToMail));
                email.Subject = emailModel.Subject;
                email.Body = emailModel.Body;

                if (emailConfiguration.Auditmails is not null)
                {
                    emailConfiguration.Auditmails.Split(",").ToList().ForEach(x => email.Bcc.Add(MailboxAddress.Parse(x)));
                }

                var fromEmailPassword = authToken;
                var smtp = new SmtpClient();

                smtp.Connect(emailConfiguration.SMTPServer, Convert.ToInt32(emailConfiguration.Port),SecureSocketOptions.StartTls);
                smtp.Authenticate(smail, fromEmailPassword);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

                return new ResponseDto(HttpStatusCode.OK,true,"Data Create Successfully", null);
            }
            catch (Exception ex)
            {
                logger.LogError("Smtp gmail error occurred:" + ex.ToString());
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}