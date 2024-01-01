using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateRDPDeviceInfo(RDPDeviceInfo rDPDeviceInfo)
        {
            try
            {
                if (await IsUsernameDuplicate(rDPDeviceInfo) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                rDPDeviceInfo.DateCreated = DateTime.Now;
                rDPDeviceInfo.IsDeleted = false;

                context.RDPDeviceInfoRepository.Add(rDPDeviceInfo);
                await context.SaveChangesAsync();

                return CreatedAtAction("ReadRDPDeviceInfoByKey", new { key = rDPDeviceInfo.Oid }, rDPDeviceInfo);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device-infoes
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoes)]
        public async Task<IActionResult> ReadRDPDeviceInfoes()
        {
            try
            {
                var rdpdevice = await context.RDPDeviceInfoRepository.GetRDPDevices();

                return Ok(rdpdevice);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table RdpDeviceinfo</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoByKey)]
        public async Task<IActionResult> ReadRDPDeviceInfoByKey(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var rdp = await context.RDPDeviceInfoRepository.GetRDPDeviceByKey(key);

                if (rdp == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(rdp);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device-info/key/{username}
        /// </summary>
        /// <param name="username">username of the table RdpDeviceinfo</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoByName)]
        public async Task<IActionResult> ReadRDPDeviceInfoByUserName(string username)
        {
            try
            {
                var rdp = await context.RDPDeviceInfoRepository.GetByUsername(username);

                if (rdp == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                return Ok(rdp);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-deviceinfobydeviceId/{deviceId}
        /// </summary>
        /// <param name="deviceId">username of the table RdpDeviceinfo</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.GetFacilitiesByDeviceId)]
        public async Task<IActionResult> GetFacilitiesByDeviceId(string deviceId)
        {
            try
            {
                var rdp = await context.RDPDeviceInfoRepository.GetByDeviceId(deviceId);

                if (rdp == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                var username = await context.UserAccountRepository.GetUserAccountByName(rdp.UserName);

                if (username == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (username.FacilityID == null)
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

                    return Ok(rDPDeviceInfo);
                }
                else
                {
                    var facility = await context.FacilityRepository.GetFacilityByKey(username.FacilityID.GetValueOrDefault());

                    var district = await context.DistrictRepository.GetDistrictByKey(facility.DistrictID);

                    var province = await context.ProvinceRepository.GetProvinceByKey(district.ProvinceID);

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

                    return Ok(rDPDeviceInfo);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        // <summary>
        /// URL: tuso-api/rdp-device-info-list
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadRDPDeviceInfoList)]
        public async Task<IActionResult> ReadRDPDeviceInfoList()
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
                        return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);
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
                            return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);
                        }

                        foreach (var facility in username)
                        {
                            if (facility.FacilityId != null)
                            {
                                facilities.Add(await context.FacilityRepository.GetFacilityByKey(facility.FacilityId.GetValueOrDefault()));
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
                            deviceInfo.ItExpertName = itexpert.FirstOrDefault().User.Name;
                    }
                    if (userTypes.Count > 0)
                    {
                        var userTypeName = userTypes.First();

                        deviceInfo.UserTypes = userTypeName.DeviceTypeName;
                    }

                    deviceInfos.Add(deviceInfo);
                }

                return Ok(deviceInfos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateRDPDeviceInfo(int key, RDPDeviceInfo rdpDevice)
        {
            try
            {
                if (key != rdpDevice.Oid)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.UnauthorizedAttemptOfRecordUpdateError);

                if (await IsUsernameDuplicate(rdpDevice) == true)
                    return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);

                rdpDevice.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(rdpDevice);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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
        public async Task<IActionResult> UpdateRDPDeviceInfobyUsername(string username, RDPDeviceInfo rdpDevice)
        {
            try
            {
                var deviceInDb = await context.RDPDeviceInfoRepository.GetByUsername(username);
                if (deviceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                if (deviceInDb.UserName != username)
                {
                    if (await IsUsernameDuplicate(rdpDevice) == true)
                        return StatusCode(StatusCodes.Status409Conflict, MessageConstants.DuplicateError);
                }

                rdpDevice.Oid = deviceInDb.Oid;
                rdpDevice.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(rdpDevice);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRDPDeviceInfo)]
        public async Task<IActionResult> DeleteRDPDeviceInfo(int key)
        {
            try
            {
                if (key <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest, MessageConstants.InvalidParameterError);

                var deviceInDb = await context.RDPDeviceInfoRepository.GetRDPDeviceByKey(key);

                if (deviceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                deviceInDb.IsDeleted = true;
                deviceInDb.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(deviceInDb);
                await context.SaveChangesAsync();

                return Ok(deviceInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/{key}
        /// </summary>
        /// <param name="key">Primary key of the table</param>
        /// <returns>Deletes a row from the table.</returns>
        [HttpDelete]
        [Route(RouteConstants.DeleteRDPDeviceInfoByUsername)]
        public async Task<IActionResult> DeleteRDPDeviceInfoByUsername(string username)
        {
            try
            {
                var deviceInDb = await context.RDPDeviceInfoRepository.GetByUsername(username);

                if (deviceInDb == null)
                    return StatusCode(StatusCodes.Status404NotFound, MessageConstants.NoMatchFoundError);

                deviceInDb.IsDeleted = true;
                deviceInDb.DateModified = DateTime.Now;

                context.RDPDeviceInfoRepository.Update(deviceInDb);
                await context.SaveChangesAsync();

                return Ok(deviceInDb);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, MessageConstants.GenericError);
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