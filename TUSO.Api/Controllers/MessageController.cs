using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Bithy
 * Date created: 14.09.2022
 * Last modified: 14.09.2022, 17.09.2022
 * Modified by: Bithy, Rakib
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///Message Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUnitOfWork context;

        /// <summary>
        ///Default constructor
        /// </summary>
        /// <param name="context"></param>
        public MessageController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/message
        /// </summary>
        /// <param name="message">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMessage)]
        public async Task<ResponseDto> CreateMessage(Message message)
        {
            try
            {
                message.MessageDate = DateTime.Now;
                message.DateCreated = DateTime.Now;
                message.IsDeleted = false;

                context.MessageRepository.Add(message);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.SaveMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/message
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMessages)]
        public async Task<ResponseDto> ReadMessages()
        {
            try
            {
                var message = await context.MessageRepository.GetMessages();

                return new ResponseDto(HttpStatusCode.OK, true, message == null ? "Data Not Found" : string.Empty, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/message/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Messages</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMessageByKey)]
        public async Task<ResponseDto> ReadMessageByKey(long key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);


                var message = await context.MessageRepository.GetMessageByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, message == null ? "Data Not Found" : string.Empty, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/message/{key}
        /// </summary>
        /// <param name="key">Primary key of the talbe</param>
        /// <param name="message">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMessage)]
        public async Task<ResponseDto> UpdateMessage(long key, Message message)
        {
            try
            {
                if (key != message.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                message.DateModified = DateTime.Now;

                context.MessageRepository.Update(message);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/message/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteMessage)]
        public async Task<ResponseDto> DeleteMessage(long key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var messageInDb = await context.MessageRepository.GetMessageByKey(key);

                if (messageInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                messageInDb.IsDeleted = true;
                messageInDb.DateModified = DateTime.Now;

                context.MessageRepository.Update(messageInDb);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.DeleteMessage, messageInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}