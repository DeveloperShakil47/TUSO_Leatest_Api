using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface ILeadMemberRepository : IRepository<TeamLead>
    {
        /// <summary>
        /// Returns a member if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<TeamLead> GetMemberByKey(long key);

        /// <summary>
        /// Returns a member if key matched.
        /// </summary>
        /// <param name="key">Primary key of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<TeamLead> GetLeaderByTeam(long key);

        /// <summary>
        /// Returns a member if key matched.
        /// </summary>
        /// <param name="key">UserID of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<TeamLead> GetMemberByUser(long key);

        /// <summary>
        /// Returns all member by Team.
        /// </summary>
        /// <returns>List of Member object.</returns>
        public Task<IEnumerable<TeamLead>> GetMemberByTeam(long key);

        /// <summary>
        /// Returns all member by Team.
        /// </summary>
        /// <returns>List of Member object.</returns>
        public Task<IEnumerable<TeamLead>> GetMembersByTeam(long key, int start, int take);

        /// <summary>
        /// Returns a Member permission if UserAccountID, TeamID matched
        /// </summary>
        /// <param name="userAccountId">Primary key of the useraccount table</param>
        /// <param name="teamId">Primary key of the System table</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<TeamLead> GetMemberPermission(long userAccountId, long teamId);

        public Task<TeamLead> GetLeadMemberByTeamId(long teamId);

        /// <summary>
        /// Returns all member.
        /// </summary>
        /// <returns>List of Member object.</returns>
        public Task<IEnumerable<TeamLead>> GetMembers();

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
        public Task<IEnumerable<TeamLead>> GetMembersByUser(long key);

        /// <summary>
        /// Returns a member if member is team lead of any team.
        /// </summary>
        /// <param name="key">UserID of the table Members</param>
        /// /// <param name="teamId">team of the table Members</param>
        /// <returns>Instance of a Member object.</returns>
        public Task<TeamLead> GetMemberByTeamLead(long key, long teamId);
    }
}