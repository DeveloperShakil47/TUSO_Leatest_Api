﻿using TUSO.Domain.Entities;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Infrastructure.Contracts
{
    public interface ITeamRepository : IRepository<Team>
    {
        /// <summary>
        /// Returns a team if key matched.
        /// </summary>
        /// <param name="OID">Primary key of the table Teams</param>
        /// <returns>Instance of a Team object.</returns>
        public Task<Team> GetTeamByKey(long oid);

        /// <summary>
        /// Returns a team if the title matched.
        /// </summary>
        /// <param name="title">Team title of the user.</param>
        /// <returns>Instance of a Team object.</returns>
        public Task<Team> GetTeamByTitle(string title);

        /// <summary>
        /// Returns all team.
        /// </summary>
        /// <returns>List of team object.</returns>
        public Task<IEnumerable<Team>> GetTeams();

        /// <summary>
        /// Returns all team.
        /// </summary>
        /// <returns>List of team object.</returns>
        public Task<IEnumerable<Team>> GetTeamsbyPage(int start, int take);
        public  Task<int> GetTeamsCount();

        /// <summary>
        /// Check is there any open ticket under the Team.
        /// </summary>
        /// <param name="OID">Primary key of the table Teams</param>
        /// <returns>Number of open ticket under the Team.</returns>
        public Task<int> TotalOpenTicketUnderTeam(long oid);
    }
}