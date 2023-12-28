using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IImplementingItemRepository : IRepository<ImplemenentingItem>
    {
        // <summary>
        /// The method is used to get a TestItem by key.
        /// </summary>
        /// <param name="key">Primary key of the table ImplemenentingItem.</param>
        /// <returns>Returns a ImplemenentingItem if the key is matched.</returns>
        public Task<ImplemenentingItem> GetImplemenentingItemByKey(int key);

        /// <summary>
        /// The method is used to get the list of ImplemenentingItem.
        /// </summary>
        /// <returns>Returns a list of all ImplemenentingItem.</returns>
        public Task<IEnumerable<ImplemenentingItem>> GetImplemenentingItems();
    }
}