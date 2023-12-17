using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface IMemberRepository : IRepository<Member>
    {
        /// <summary>
        /// Returns a member if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<Member> GetMemberByKey(long key);

        /// <summary>
        /// Returns a member if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<Member> GetLeaderByTeam(long key);

        /// <summary>
        /// Returns a member if key matched.
        /// </summary>
        /// <param name="key">UserID of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<Member> GetMemberByUser(long key);

        /// <summary>
        /// Returns all member by Team.
        /// </summary>
        /// <returns>List of Member object.</returns>
        public Task<IEnumerable<Member>> GetMemberByTeam(long key);

        /// <summary>
        /// Returns all member by Team.
        /// </summary>
        /// <returns>List of Member object.</returns>
        public Task<IEnumerable<Member>> GetMembersByTeam(long key, int start, int take);

        /// <summary>
        /// Returns a Member permission if UserAccountID, TeamID matched
        /// </summary>
        /// <param name="UserAccountID">Primary key of the useraccount table</param>
        /// <param name="TeamID">Primary key of the System table</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<Member> GetMemberPermission(long UserAccountID, long TeamID);

        /// <summary>
        /// Returns all member.
        /// </summary>
        /// <returns>List of Member object.</returns>
        public Task<IEnumerable<Member>> GetMembers();

        /// <summary>
        /// Count  all member.
        /// </summary>
        /// <returns>Count number Member object.</returns>
        public Task<int> GetMemberCount(int key);

        /// <summary>
        /// Returns all the members if key matched.
        /// </summary>
        /// <param name="key">UserID of the table Members</param>
        /// <returns>Instance of a List of Member object.</returns>
        public Task<IEnumerable<Member>> GetMembersByUser(long key);

        /// <summary>
        /// Returns a member if member is team lead of any team.
        /// </summary>
        /// <param name="key">UserID of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<Member> GetMemberByTeamLead(long key, long teamId);
    }
}