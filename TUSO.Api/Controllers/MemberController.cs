using Microsoft.AspNetCore.Mvc;
using TUSO.Domain.Dto;
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
    ///Member Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IUnitOfWork context;

        /// <summary>
        ///Default constructor
        /// </summary>
        /// <param name="context"></param>
        public MemberController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/member
        /// </summary>
        /// <param name="member">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMember)]
        public async Task<IActionResult> CreateMember(MemberDto model)
        {
            try
            {
                if (!model.IsTeamLead)
                {
                    Member member = new Member()
                    {
                        UserAccountId = model.UserAccountId,
                        TeamId = model.TeamId,
                    };

                    if (await IsMemberDuplicate(member) == true)
                        return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                    member.DateCreated = DateTime.Now;
                    member.IsDeleted = false;

                    context.MemberRepository.Add(member);
                }
                else
                {
                    TeamLead leadMember = new TeamLead()
                    {
                        UserAccountId = model.UserAccountId,
                        TeamId = model.TeamId,
                    };

                    if (await IsLeadMemberDuplicateAndAlreadyHaveTeamLead(leadMember) == true)
                        return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                    leadMember.DateCreated = DateTime.Now;
                    leadMember.IsDeleted = false;

                    context.LeadMemberRepository.Add(leadMember);
                }

                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/members
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMembers)]
        public async Task<IActionResult> ReadMembers()
        {
            try
            {
                var member = await context.MemberRepository.GetMembers();

                return Ok(member);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">Primary key of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMemberByKey)]
        public async Task<IActionResult> ReadMemberByKey(long key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var member = await context.MemberRepository.GetMemberByKey(key);

                if (member == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(member);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">UserAccountID of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMemberByUser)]
        public async Task<IActionResult> GetMemberByUser(long key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var member = await context.MemberRepository.GetMemberByUser(key);

                if (member == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(member);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">UserAccountID of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTeamMemberByKey)]
        public async Task<IActionResult> GetMemberByTeam(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var member = await context.MemberRepository.GetMemberByTeam(key);

                if (member == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(member);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }


        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">UserAccountID of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMemberByTeamPage)]
        public async Task<IActionResult> ReadMembersByTeam(int key, int start, int take)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var member = await context.MemberRepository.GetMembersByTeam(key, start, take);
                var response = new
                {
                    members = member,
                    currentPage = start+1,
                    totalRows = await context.MemberRepository.GetMemberCount(key)
                };
                if (member == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/member/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="member">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateMember)]
        public async Task<IActionResult> UpdateMember(long key, Member member)
        {
            try
            {
                if (key != member.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsMemberDuplicate(member) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                member.DateModified = DateTime.Now;
                member.IsDeleted = false;

                context.MemberRepository.Update(member);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// Checks whether the permission is duplicate? 
        /// </summary>
        /// <param name ="member"> Member object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsMemberDuplicate(Member member)
        {
            try
            {
                var permissionInDb = await context.MemberRepository.GetMemberPermission(member.UserAccountId, member.TeamId);

                if (permissionInDb != null)
                    return true;

                return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks whether the permission is duplicate? 
        /// </summary>
        /// <param name ="member"> Member object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsLeadMemberDuplicateAndAlreadyHaveTeamLead(TeamLead leadMember)
        {
            try
            {
                var permissionInDb = await context.LeadMemberRepository.GetMemberPermission(leadMember.UserAccountId, leadMember.TeamId);

                if (permissionInDb != null)
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