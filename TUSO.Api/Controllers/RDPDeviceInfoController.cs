using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;

/*
 * Created by: Selim
 * Date created: 14.02.2023
 * Last modified: 
 * Modified by: 
 */
namespace TUSO.Api.Controllers
{
    /// <summary>
    ///RDPDeviceInfo Controller
    /// </summary>
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class RDPDeviceInfoController : ControllerBase
    {
        private readonly IUnitOfWork context;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context"></param>
        public RDPDeviceInfoController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// <summary>
        /// URL: tuso-api/rdp-device
        /// </summary>
        /// <param name="rdp-device">Object to be saved in the table as a row.</param>
        /// <returns>Saved object.</returns>
        [HttpPost]
        [Route(RouteConstants.CreateRDPDeviceInfo)]
        public async Task<ResponseDto> CreateRDPDeviceInfo(RDPDeviceInfo rDPDeviceInfo)
        {
            try
            {
                if (await IsUsernameDuplicate(rDPDeviceInfo) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                rDPDeviceInfo.DateCreated = DateTime.Now;
                rDPDeviceInfo.IsDeleted = false;

                context.RDPDeviceInfoRepository.Add(rDPDeviceInfo);
                await context.SaveChangesAsync();
               var rdp = await context.RDPDeviceInfoRepository.GetRDPDeviceByKey(rDPDeviceInfo.Oid);
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.SaveMessage, rdp);

            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device-infoes
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoes)]
        public async Task<ResponseDto> ReadRDPDeviceInfoes()
        {
            try
            {
                var rdpdevice = await context.RDPDeviceInfoRepository.GetRDPDevices();
                return new ResponseDto(HttpStatusCode.OK, true, rdpdevice == null ? "Data Not Found" : "Data Loaded", rdpdevice);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table RdpDeviceinfo</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoByKey)]
        public async Task<ResponseDto> ReadRDPDeviceInfoByKey(int key)
        {
            try
            {
                if (key <= 0)
                return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var rdp = await context.RDPDeviceInfoRepository.GetRDPDeviceByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, rdp == null ? "Data Not Found" : "Data Loaded", rdp);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device-info/key/{username}
        /// </summary>
        /// <param name="username">username of the table RdpDeviceinfo</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoByName)]
        public async Task<ResponseDto> ReadRDPDeviceInfoByUserName(string username)
        {
            try
            {
                var rdp = await context.RDPDeviceInfoRepository.GetByUsername(username);

                return new ResponseDto(HttpStatusCode.OK, true, rdp == null? "Data Not Found":"Data Loaded", rdp);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-deviceinfobydeviceId/{deviceId}
        /// </summary>
        /// <param name="deviceId">username of the table RdpDeviceinfo</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.GetFacilitiesByDeviceId)]
        public async Task<ResponseDto> GetFacilitiesByDeviceId(string deviceId)
        {
            try
            {
                var rdp = await context.RDPDeviceInfoRepository.GetByDeviceId(deviceId);

                if (rdp == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                var username = await context.UserAccountRepository.GetUserAccountByName(rdp.UserName);
                var facilityId = await context.FacilityPermissionRepository.GetFacilityPermissionByUserId(username.Oid, false);
                if (username == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                if (facilityId == null)
                {
                    RDPDeviceInfoDto rDPDeviceInfo = new RDPDeviceInfoDto();

                    rDPDeviceInfo.UserName = username.Username;
                    rDPDeviceInfo.FacilityName = null;
                    rDPDeviceInfo.DistrictName = null;
                    rDPDeviceInfo.ProviceName = null;
                    rDPDeviceInfo.DeviceId = deviceId;
                    rDPDeviceInfo.PrivateIp = rdp.PrivateIp;
                    rDPDeviceInfo.PublicIp = rdp.PublicIp;
                    rDPDeviceInfo.MACAddress = rdp.MACAddress;
                    rDPDeviceInfo.MotherBoardSerial = rdp.MotherBoardSerial;


                    return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, rDPDeviceInfo);
                }
                else
                {
                    var facility = await context.FacilityRepository.GetFacilityByKey(facilityId.FacilityId);

                    var district = await context.DistrictRepository.GetDistrictByKey(facility.DistrictId);

                    var province = await context.ProvinceRepository.GetProvinceByKey(district.ProvinceId);

                    RDPDeviceInfoDto rDPDeviceInfo = new RDPDeviceInfoDto();

                    rDPDeviceInfo.UserName = username.Username;
                    rDPDeviceInfo.FacilityName = facility.FacilityName;
                    rDPDeviceInfo.DistrictName = district.DistrictName;
                    rDPDeviceInfo.ProviceName = province.ProvinceName;
                    rDPDeviceInfo.DeviceId = deviceId;
                    rDPDeviceInfo.PrivateIp = rdp.PrivateIp;
                    rDPDeviceInfo.PublicIp = rdp.PublicIp;
                    rDPDeviceInfo.MACAddress = rdp.MACAddress;
                    rDPDeviceInfo.MotherBoardSerial = rdp.MotherBoardSerial;

                    return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, rDPDeviceInfo);
                }

            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        // <summary>
        /// URL: tuso-api/rdp-device-info-list
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoList)]
        public async Task<ResponseDto> ReadRDPDeviceInfoList()
        {
            try
            {
                var rdpdevice = await context.RDPDeviceInfoRepository.GetRDPDevices();

                var deviceInfos = new List<RDPDeviceInfoDto>();

                foreach (var device in rdpdevice)
                {

                    var rdp = await context.RDPDeviceInfoRepository.GetRDPDeviceInfoesByDevice(device.DeviceId);

                    if (rdp == null)
                    {
                        return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);
                    }


                    var deviceInfo = new RDPDeviceInfoDto
                    {
                        DeviceId = device.DeviceId,
                        UserName = device.UserName,
                        MACAddress = device.MACAddress,
                        MotherBoardSerial = device.MotherBoardSerial,
                        PrivateIp = device.PrivateIp,
                        PublicIp = device.PublicIp,
                        FacilityName = null,
                        ProviceName = null,
                        DistrictName = null,
                        UserTypes = null
                    };

                    var facilities = new List<Facility>();
                    var userTypes = new List<DeviceType>();

                    foreach (var user in rdp)
                    {
                        var username = await context.UserAccountRepository.GetUserAccountBydevice(user.UserName);

                        if (username == null)
                        {
                            return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);
                        }

                        foreach (var facility in username)
                        {
                            var facilityPermission = await context.FacilityPermissionRepository.GetFacilityPermissionByUserId(facility.Oid, false);
                            if (facilityPermission != null)
                            {
                                facilities.Add(await context.FacilityRepository.GetFacilityByKey(facilityPermission.Oid));
                            }
                        }

                        foreach (var users in username)
                        {
                            if (users.DeviceTypeId != null)
                            {
                                userTypes.Add(await context.DeviceTypeRepository.GetDeviceTypeByKey(users.DeviceTypeId.GetValueOrDefault()));
                            }

                        }
                    }

                    if (facilities.Count > 0)
                    {
                        var facility = facilities.First();
                        var district = await context.DistrictRepository.GetDistrictByKey(facility.DistrictId);
                        var province = await context.ProvinceRepository.GetProvinceByKey(district.ProvinceId);

                        deviceInfo.FacilityName = facility.FacilityName;
                        deviceInfo.ProviceName = province.ProvinceName;
                        deviceInfo.DistrictName = district.DistrictName;
                        var itexpert = await context.FacilityPermissionRepository.GetFacilityUserByKey(facility.Oid);

                        if (itexpert is not null && itexpert.Count > 0)
                            deviceInfo.ItExpertName = itexpert.FirstOrDefault().UserAccount.Name;
                    }
                    if (userTypes.Count > 0)
                    {
                        var userTypeName = userTypes.First();

                        deviceInfo.UserTypes = userTypeName.DeviceTypeName;
                    }

                    deviceInfos.Add(deviceInfo);
                }

                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, deviceInfos);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        /// <summary>
        /// URL: tuso-api/rdp-device-info/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="rdp-device">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateRDPDeviceInfo)]
        public async Task<ResponseDto> UpdateRDPDeviceInfo(int key, RDPDeviceInfo rdpDevice)
        {
            try
            {
                if (key != rdpDevice.Oid)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.UnauthorizedAttemptOfRecordUpdateError, null);

                if (await IsUsernameDuplicate(rdpDevice) == true)
                    return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);

                rdpDevice.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(rdpDevice);
                await context.SaveChangesAsync();

                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <param name="rdp-device">Object to be updated</param>
        /// <returns>Update row in the table.</returns>
        [HttpPut]
        [Route(RouteConstants.UpdateRDPDeviceInfoByUsername)]
        public async Task<ResponseDto> UpdateRDPDeviceInfobyUsername(string username, RDPDeviceInfo rdpDevice)
        {
            try
            {
                var deviceInDb = await context.RDPDeviceInfoRepository.GetByUsername(username);
                if (deviceInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                if (deviceInDb.UserName != username)
                {
                    if (await IsUsernameDuplicate(rdpDevice) == true)
                        return new ResponseDto(HttpStatusCode.Conflict, false, MessageConstants.DuplicateError, null);
                }

                rdpDevice.Oid = deviceInDb.Oid;
                rdpDevice.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(rdpDevice);
                await context.SaveChangesAsync();
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.UpdateMessage, null);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRDPDeviceInfo)]
        public async Task<ResponseDto> DeleteRDPDeviceInfo(int key)
        {
            try
            {
                if (key <= 0)
                    return new ResponseDto(HttpStatusCode.BadRequest, false, MessageConstants.InvalidParameterError, null);

                var deviceInDb = await context.RDPDeviceInfoRepository.GetRDPDeviceByKey(key);

                if (deviceInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                deviceInDb.IsDeleted = true;
                deviceInDb.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(deviceInDb);
                await context.SaveChangesAsync();
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.DeleteMessage, deviceInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRDPDeviceInfoByUsername)]
        public async Task<ResponseDto> DeleteRDPDeviceInfoByUsername(string username)
        {
            try
            {
                var deviceInDb = await context.RDPDeviceInfoRepository.GetByUsername(username);

                if (deviceInDb == null)
                    return new ResponseDto(HttpStatusCode.NotFound, false, MessageConstants.NoMatchFoundError, null);

                deviceInDb.IsDeleted = true;
                deviceInDb.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(deviceInDb);
                await context.SaveChangesAsync();
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.DeleteMessage, deviceInDb);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// Checks whether the rdp-device name is duplicate? 
        /// </summary>
        /// <param name="rdp-device">rdp-device object.</param>
        /// <returns>Boolean</returns>
        private async Task<bool> IsUsernameDuplicate(RDPDeviceInfo rdpDevice)
        {
            try
            {
                var rdpDeviceInDb = await context.RDPDeviceInfoRepository.GetByUsername(rdpDevice.UserName);

                if (rdpDeviceInDb != null)
                    if (rdpDeviceInDb.Oid != rdpDevice.Oid)
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