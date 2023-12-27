using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
 * Created by: Bithy
 * Date created: 14.09.2022
 * Last modified: 14.09.2022
 * Modified by: Bithy
 */
namespace TUSO.Infrastructure.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DataContext context) : base(context)
        {

        }

        public async Task<Message> GetMessageByKey(long OID)
        {
            try
            {
                return await FirstOrDefaultAsync(c => c.Oid == OID && c.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Message>> GetMessages()
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Message>> GetMessageByIncedent(long key)
        {
            try
            {
                return await QueryAsync(c => c.IsDeleted == false && c.IncidentId == key, o => o.Oid);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}