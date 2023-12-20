using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IUnitOfWork
    {
        ICountryRepository CountryRepository { get; }

        Task<int> SaveChangesAsync();

    }
}