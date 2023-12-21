using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;

namespace TUSO.Infrastructure.Contracts
{
    public interface IUserRepository : IRepository<UserAccount>
    {
        /// <summary>
        /// Returns a user account if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts</param>
        /// <returns>Instance of a UserAccount object.</returns>
        public Task<UserAccount> GetUserAccountByKey(long key);

        /// <summary>
        /// Returns a user account if role matched.
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts</param>
        /// <param name="start">Starting position of UserAccount</param>
        /// <param name="take">Get UserAccount from database</param>
        /// <returns>Instance of a UserAccount object.</returns>
        public Task<UserListDto> GetUserAccountByRole(int key, int start, int take);

        /// <summary>
        /// Returns a user account if expert matched.
        /// </summary>
        /// <returns>Instance of a UserAccount object.</returns>
        public Task<IEnumerable<UserAccount>> GetUserAccountByExpert();

        /// <summary>
        /// Returns a user account if the username matched.
        /// </summary>
        /// <param name="name">Name of user</param>
        /// <returns>Instance of a UserAccount object.</returns>
        public Task<UserAccount> GetUserAccountByName(string name);

        /// <summary>
        /// Returns all active user accounts.
        /// </summary>
        /// <returns>List of UserAccount object.</returns>
        public Task<UserListDto> GetUsers(int start, int take);

        /// Returns all active user accounts.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start">Starting position of UserAccount</param>
        /// <param name="take">Get UserAccount from database</param>
        /// <returns>List of UserAccount object.</returns>
        public Task<UserListDto> GetUsersByName(string name, int start, int take);

        /// <summary>
        /// The method is used to get a user account by cellphone number.
        /// </summary>
        /// <param name="cellphone">Cellphone number of a user.</param>
        /// <returns>Returns a user account if the cellphone number is matched.</returns>
        public Task<UserAccount> GetUserAccountByCellphone(string cellphone);

        /// <summary>
        /// Returns a user account if the username,password matched.
        /// </summary>
        /// <param name="UserName">Username of the UserAccount.</param>
        /// <param name="Password">Password of the UserAccount</param>
        /// <returns>Instance of a UserAccount object.</returns>
        public Task<UserAccount> GetUserByUserNamePassword(string UserName, string Password);

        /// <summary>
        /// Returns a user List if the name matched.
        /// </summary>
        /// <param name="name">Device Info of the user.</param>
        /// <returns>Instance of a Device Info object.</returns>
        public Task<IEnumerable<UserAccount>> GetUserAccountBydevice(string name);

        /// <summary>
        ///Returns a user account if the username, Cellphone matched.
        /// </summary>
        /// <param name="Cellphone"></param>
        /// <param name="Username"></param>
        /// <param name="CountryCode"></param>
        /// <returns>Instance of a UserAccount object.</returns>
        public Task<UserAccount> GetUserByUsernameCellPhone(string Cellphone, string Username, string CountryCode);

        /// <summary>
        /// Check is there any open ticket under a client.
        /// </summary>
        /// <param name="OID">Primary key of the table UserAccount</param>
        /// <returns>Number of open ticket under a client.</returns>
        public Task<int> TotalOpenTicketUnderClient(long OID);

        /// <summary>
        /// Check if expert is a team leader.
        /// </summary>
        /// <param name="OID">Primary key of the table UserAccount</param>
        /// <returns>return true if user is a team leader.</returns>
        public Task<bool> IsTeamLeader(long OID);

        /// <summary>
        /// Returns a user account if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table UserAccounts</param>
        /// <returns>Instance of a UserDto object.</returns>
        public Task<UserDto> GetClientAccountByKey(long key);

        /// <summary>
        /// Returns a System permission if SystemID matched
        /// </summary>
        /// <param name="SystemID">Primary key of the System table></param>
        /// <returns>Instance of a SystemPermission object.</returns>
        public Task<IEnumerable<UserAccount>> GetUserByUsertype(int UsertypeID);

        /// <summary>
        /// Returns a User  of  name matched.
        /// </summary>
        /// <param name="name">name of the UserAccount table></param>
        /// <returns>Instance of a UserAccount object.</returns>
        public Task<List<UserAccount>> GetUserAccountByFullName(string name);


        /// <summary>
        /// Returns admin user accounts.
        /// </summary>
        /// <returns>get of UserAccount object.</returns>
        public Task<List<UserAccount>> GetAdminUser();

        /// <summary>
        /// Returns admin user accounts.
        /// </summary>
        /// <returns>List of UserAccount object.</returns>
        public Task<UserAccountCountDto> UserAccouontCount();
    }
}
