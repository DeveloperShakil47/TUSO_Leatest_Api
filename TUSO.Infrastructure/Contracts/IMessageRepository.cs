using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IMessageRepository : IRepository<Message>
    {
        /// <summary>
        /// Returns a message if key matched.
        /// </summary>
        /// <param name="Oid">Primary key of the table Messages</param>
        /// <returns>Instance of a Message object.</returns>
        public Task<Message> GetMessageByKey(long Oid);

        /// <summary>
        /// Returns a message if key matched.
        /// </summary>
        /// <param name="Oid">Primary key of the table Messages</param>
        /// <returns>Instance of a Message object.</returns>
        public Task<IEnumerable<Message>> GetMessageByIncedent(long Oid);

        /// <summary>
        /// Returns all message.
        /// </summary>
        /// <returns>List of Message object.</returns>
        public Task<IEnumerable<Message>> GetMessages();
    }
}