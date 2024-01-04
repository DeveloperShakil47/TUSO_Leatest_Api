using Microsoft.AspNetCore.Mvc;
using System.Net;
using TUSO.Authorization;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Utilities.Constants;
using Utilities.Encryptions;

/*
* Created by: Stephan
* Date created: 01.01.2024
* Last modified:
* Modified by: 
*/
namespace TUSO.Api.Controllers
{
    [Route(RouteConstants.BaseRoute)]
    [ApiController]
    public class RDPController : Controller
    {
        private readonly IUnitOfWork context;

        public RDPController(IUnitOfWork context)
        {
            this.context = context;
        }

        /// URL: tuso-api/rdpserver/login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(RouteConstants.RDPLogin)]
        [CustomAuthorization]
        public async Task<ResponseDto> UserLogin(LoginDto login)
        {
            try
            {
                EncryptionHelpers encryptionHelpers = new EncryptionHelpers();
                string encryptedPassword = encryptionHelpers.Encrypt(login.Password);
                var rdp = await context.RDPRepository.GetSingleRDPServerInfo();
                var user = await context.UserAccountRepository.GetUserByUserNamePassword(login.UserName, encryptedPassword);

             
                return new ResponseDto(HttpStatusCode.OK, true, user != null && rdp != null ? "Data Not Found" : "Data Loaded", rdp);
            }
            catch (Exception ex)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-devices
        /// </summary>
        /// <returns>List of table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDevicesType)]
        [CustomAuthorization]
        public async Task<ResponseDto> ReadDevices()
        {
            try
            {
                var device = await context.RDPRepository.GetRootobject();
                List<Device> devices = new List<Device>();
                foreach (var d in device)
                {
                    devices.Add(new Device()
                    {
                        agentVersion  = d.agentVersion,
                        alerts  = d.alerts,
                        alias  = d.alias,
                        cpuUtilization  = d.cpuUtilization,
                        currentUser  = d.currentUser,
                        deviceGroup  = d.deviceGroup,
                        deviceGroupID  = d.deviceGroupID,
                        deviceName  = d.deviceName,
                        drives  = d.drives,
                        id  = d.id,
                        is64Bit  = d.is64Bit,
                        isOnline  = d.isOnline,
                        OnlineStatus  = d.OnlineStatus,
                        lastOnline  = d.lastOnline,
                        notes  = d.notes,
                        organizationID  = d.organizationID,
                        osArchitecture  = d.osArchitecture,
                        osDescription  = d.osDescription,
                        platform  = d.platform,
                        processorCount  = d.processorCount,
                        publicIP  = d.publicIP,
                        serverVerificationToken  = d.serverVerificationToken,
                        tags  = d.tags,
                        totalMemory  = d.totalMemory,
                        totalStorage  = d.totalStorage,
                        usedMemory  = d.usedMemory,
                        usedMemoryPercent  = d.usedMemoryPercent,
                        usedStorage  = d.usedStorage,
                        usedStoragePercent  = d.usedStoragePercent,
                        webRtcSetting  = d.webRtcSetting,
                        offlineHours  = TimeSpan.FromDays(1) - d.onlineHours,
                        onlineHours  = d.onlineHours,
                        onlineHoursInDigit  =(int)d.onlineHours.TotalHours,
                        offlineHoursInDigit =(int)TimeSpan.FromDays(1).Hours - d.onlineHours.Hours,
                        //offlineHoursInDigit  = (int) ( 24 - d.onlineHours.TotalHours)

                    }); ;
                }


                return new ResponseDto(HttpStatusCode.OK, true, devices?.Count()==0?"Data Not Found":"Data Loaded", devices);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }


        [HttpGet]
        [Route(RouteConstants.GetDeviceActivity)]
        [CustomAuthorization]
        public ResponseDto GetDeviceActivity(string fromDate, string? toDate, string? userName)
        {
            try
            {
                var devicesByDate =  context.RDPRepository.GetDeviceActivity(fromDate, toDate);
                List<DeviceActivityReportDto> devices = new List<DeviceActivityReportDto>();
                IEnumerable<RDPDeviceInfo> deviceWithUser = context.RDPRepository.GetDeviceActivity(userName);
                int sl = 1;
                if (devicesByDate != null && deviceWithUser != null)
                {
                    foreach (var device in devicesByDate)
                    {

                        foreach (var deviceUser in deviceWithUser)
                        {
                            if (device.DeviceID == deviceUser.DeviceId)
                            {
                                devices.Add(new DeviceActivityReportDto()
                                {
                                    DeviceID = device.DeviceID,
                                    UserName= deviceUser.UserName,
                                    OnlineHours = device.OnlineHours,
                                    OfflineHours = device.OfflineHours,
                                    DeviceName = device.DeviceName,
                                    MacAddress = deviceUser.MACAddress,
                                    PublicIP = deviceUser.PublicIp,
                                    PrivateIP = deviceUser.PrivateIp,
                                    MotherboardSerial = deviceUser.MotherBoardSerial,
                                    DistrictName = deviceUser.DistrictName,
                                    FacilityName = deviceUser.FacilityName,
                                    ProvinceName = deviceUser.ProvinceName,
                                    ItExpertName = "",
                                    //OfflineHoursDigit = device.OfflineHours.Split(":").ToArray()[0],
                                    //OnlineHoursDigit = device.OnlineHours.Hours,
                                    Processors =device.Processor,
                                    Platform = device.Platform,
                                    UsedStorage = device.UsedStorage,
                                    UserType = deviceUser.UserTypeName,
                                    Sl  = sl,

                                });
                            }
                        }
                        sl++;
                    }
                }
                return new ResponseDto(HttpStatusCode.OK, true, devices?.Count()==0 ? "Data Not Found" : "Data Loaded", devices);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Devices</param>
        /// <returns>Instance of a table object.</returns>
        [HttpGet]
        [Route(RouteConstants.ReadDeviceByKey)]
        [CustomAuthorization]
        public ResponseDto ReadDeviceByKey(string key)
        {
            try
            {
                var device = context.RDPRepository.GetDeviceByKey(key);

                return new ResponseDto(HttpStatusCode.OK, true, device == null ? MessageConstants.NoMatchFoundError : "Data Loaded", device);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        [HttpGet]
        [Route(RouteConstants.ReadDeviceUserByKey)]
        [CustomAuthorization]
        public ResponseDto ReadDeviceUserByKey(string key)
        {
            try
            {
                var response = context.RDPRepository.GetDeviceUserByKey(key);
                if (response == null)
                {

                    return new ResponseDto(HttpStatusCode.NotFound, true, MessageConstants.NoMatchFoundError, null);
                }

                UserAccountDto userAccount = new()
                {
                    Email = response.Email,
                    Name = response.Name,
                    SureName = response.Surname,
                    Cellphone = response.Cellphone
                };
                return new ResponseDto(HttpStatusCode.OK, true, MessageConstants.NoMatchFoundError, userAccount);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }

        /// <summary>
        /// URL: tuso-api/rdp-device/key/{key}
        /// </summary>
        /// <param name="key">Primary key of the table Devices</param>
        /// <returns>Instance of a table object.</returns>
        [HttpPost]
        [Route(RouteConstants.UninstallDeviceByKey)]
        [CustomAuthorization]
        public ResponseDto UninstallDeviceByKey(string deviceID)
        {
            try
            {
                var device = context.RDPRepository.UninstallDeviceByKey(deviceID);

                return new ResponseDto(HttpStatusCode.OK, true, device == null? MessageConstants.NoMatchFoundError:"Data Loaded" , device);
            }
            catch (Exception)
            {
                return new ResponseDto(HttpStatusCode.InternalServerError, false, MessageConstants.GenericError, null);
            }
        }
    }
}