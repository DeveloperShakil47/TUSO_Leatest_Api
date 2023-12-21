﻿using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Stephan
 * Date created: 17.12.2023
 * Last modified:
 * Modified by: 
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///Team Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly IUnitOfWork context;

        /// <summary>
        ///Default constructor
        /// </summary>
        /// <param name="context"></param>
        public TeamController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/team
        /// </summary>
        /// <param name="team">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateTeam)]
        public async Task<IActionResult> CreateTeam(Team team)
        {
            try
            {
                if (await IsTeamDuplicate(team) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                team.DateCreated = DateTime.Now;
                team.IsDeleted = false;

                context.TeamRepository.Add(team);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadTeamByKey", new { key = team.Oid }, team);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/team
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTeams)]
        public async Task<IActionResult> ReadTeams()
        {
            try
            {
                var team = await context.TeamRepository.GetTeams();
                return Ok(team);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/team
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTeamsbyPage)]
        public async Task<IActionResult> ReadTeamsbyPage(int start, int take)
        {
            try
            {
                var team = await context.TeamRepository.GetTeamsbyPage(start, take);
                var response = new
                {
                    team = team,
                    currentPage = start+1,
                    totalRows = context.TeamRepository.GetTeamsCount()
                };
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/team/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Teams</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTeamByKey)]
        public async Task<IActionResult> ReadTeamByKey(long key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var team = await context.TeamRepository.GetTeamByKey(key);

                if (team == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(team);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/team/{key}
        /// </summary>
        /// <param name="key">Primary key of the talbe</param>
        /// <param name="team">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateTeam)]
        public async Task<IActionResult> UpdateTeam(long key, Team team)
        {
            try
            {
                if (key != team.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsTeamDuplicate(team) == true)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.DuplicateError);

                team.DateModified = DateTime.Now;
                team.IsDeleted = false;

                context.TeamRepository.Update(team);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/team/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteTeam)]
        public async Task<IActionResult> DeleteTeam(long key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var teamInDb = await context.TeamRepository.GetTeamByKey(key);

                if (teamInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var totalOpenTicketUnderTeam = await context.TeamRepository.TotalOpenTicketUnderTeam(teamInDb.Oid);

                if (totalOpenTicketUnderTeam > 0)
                    return StatusCode(StatusCodes.Status405MethodNotAllowed, MessageConstants.DependencyError);

                teamInDb.IsDeleted = true;
                teamInDb.DateModified = DateTime.Now;

                context.TeamRepository.Update(teamInDb);
                await context.SaveChangesAsync();

                return Ok(teamInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the team name is duplicate? 
        /// </summary>
        /// <param name="team">Team object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsTeamDuplicate(Team team)
        {
            try
            {
                var teamInDb = await context.TeamRepository.GetTeamByTitle(team.Title);

                if (teamInDb != null)
                    if (teamInDb.Oid != team.Oid)
                        return true;

                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}