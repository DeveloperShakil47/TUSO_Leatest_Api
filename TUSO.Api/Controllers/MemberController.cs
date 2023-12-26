using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        private readonly ILogger<MemberController> logger;

        /// <summary>
        ///Default constructor
        /// </summary>
        /// <param name="context"></param>
        public MemberController(IUnitOfWork context, ILogger<MemberController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <summary>
        /// URL: tuso-api/member
        /// </summary>
        /// <param name="member">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateMember)]
        public async Task<ResponseDto> CreateMember(MemberDto memberDto  )
        {
            try
            {
                if (!memberDto.IsTeamLead)
                {
                    Member member = new Member()
                    {
                        UserAccountId = memberDto.UserAccountId,
                        TeamId = memberDto.TeamId,
                    };

                    if (await IsMemberDuplicate(member) == true)
                        return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                    member.DateCreated = DateTime.Now;
                    member.IsDeleted = false;

                    context.MemberRepository.Add(member);
                }
                else
                {
                    TeamLead leadMember = new TeamLead()
                    {
                        UserAccountId = memberDto.UserAccountId,
                        TeamId = memberDto.TeamId,
                    };

                    if (await IsLeadMemberDuplicateAndAlreadyHaveTeamLead(leadMember) == true)
                        return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                    leadMember.DateCreated = DateTime.Now;
                    leadMember.IsDeleted = false;

                    context.LeadMemberRepository.Add(leadMember);
                }

                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Create Successfully", memberDto);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "CreateMember", "MemberController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/members
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMembers)]
        public async Task<ResponseDto> ReadMembers()
        {
            try
            {
                var member = await context.MemberRepository.GetMembers();

                return new ResponseDto(HttpStatusCode.OK, true, member == null ? "Data Not Found" : "Successfully Get All Data", member);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMembers", "MemberController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">Primary key of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMemberByKey)]
        public async Task<ResponseDto> ReadMemberByKey(long key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var member = await context.MemberRepository.GetMemberByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, member == null ? "Data Not Found" : "Successfully Get Data by Key", member);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMemberByKey", "MemberController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">UserAccountID of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMemberByUser)]
        public async Task<ResponseDto> GetMemberByUser(long key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var member = await context.MemberRepository.GetMemberByUser(key);

                return new ResponseDto(HttpStatusCode.OK, true, member == null ? "Data Not Found" : "Successfully Get Data by Key", member);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetMemberByUser", "MemberController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">UserAccountID of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadTeamMemberByKey)]
        public async Task<ResponseDto> GetMemberByTeam(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var member = await context.MemberRepository.GetMemberByTeam(key);

                return new ResponseDto(HttpStatusCode.OK, true, member == null ? "Data Not Found" : "Successfully Get Data by Key", member);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "GetMemberByTeam", "MemberController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL: tuso-api/member/key/{OID}
        /// </summary>
        /// <param name="key">UserAccountID of the table Members</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadMemberByTeamPage)]
        public async Task<ResponseDto> ReadMembersByTeam(int key, int start, int take)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var member = await context.MemberRepository.GetMembersByTeam(key, start, take);
                var response = new
                {
                    members = member,
                    currentPage = start+1,
                    totalRows = await context.MemberRepository.GetMemberCount(key)
                };
                return new ResponseDto(HttpStatusCode.OK, true, response == null ? "Data Not Found" : "Successfully Get All Data", response);

            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "ReadMembersByTeam", "MemberController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
        public async Task<ResponseDto> UpdateMember(long key, Member member)
        {
            try
            {
                if (key != member.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsMemberDuplicate(member) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                member.DateModified = DateTime.Now;
                member.IsDeleted = false;

                context.MemberRepository.Update(member);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, "Data Updated Successfully", member);
            }
            catch (Exception ex)
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "UpdateMember", "MemberController.cs", ex.Message);

                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
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
            catch(Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsMemberDuplicate", "MemberController.cs", ex.Message);

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
            catch( Exception ex) 
            {
                logger.LogError("{LogDate}{Location}{MethodName}{ClassName}{ErrorMessage}", DateTime.Now, "BusinessLayer", "IsLeadMemberDuplicateAndAlreadyHaveTeamLead", "MemberController.cs", ex.Message);

                throw;
            }
        }
    }
}