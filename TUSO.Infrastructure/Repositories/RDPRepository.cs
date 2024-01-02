using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using TUSO.Domain.Dto;
using TUSO.Domain.Entities;
using TUSO.Infrastructure.Contracts;
using TUSO.Infrastructure.SqlServer;

/*
* Created by: Stephan
* Date created: 01.01.2024
* Last modified:
* Modified by: 
*/
namespace TUSO.Infrastructure.Repositories
{

    public class RDPRepository : Repository<RdpServerInfo>, IRDPRepository
    {
        private readonly RemoteDeviceSettings _remoteDeviceSettings;

        public RDPRepository(DataContext context, IOptions<RemoteDeviceSettings> options) : base(context)
        {

            _remoteDeviceSettings = options.Value;
        }

        public async Task<RdpServerInfo> GetSingleRDPServerInfo()
        {
            return await FirstOrDefaultAsync(r => r.IsDeleted == false);
        }

        public async Task<RdpServerInfo> GetUserByUserNamePassword(string UserName, string Password)
        {
            try
            {
                return await FirstOrDefaultAsync(u => u.ServerURL == UserName && u.OrganizationId == Password && u.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Device>> GetRootobject()
        {
            try
            {
                var client = new RestClient($"{_remoteDeviceSettings.BaseUrl}Devices");

                var request = new RestRequest();

                request.Method = Method.Get;

                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", $"Basic {_remoteDeviceSettings.AuthToken}");

                var response = client.Execute(request).Content;

                List<Device> root = JsonConvert.DeserializeObject<List<Device>>(response);

                return root;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<RdpDeviceActivityDto> GetDeviceActivity(string fromDate, string toDate)
        {
            try
            {
                var client = new RestClient($"{_remoteDeviceSettings.BaseUrl}Devices/DevicesByDate?fromDate={fromDate}&toDate={toDate}");
              
                toDate = string.IsNullOrEmpty(toDate) == true? string.Empty : toDate;

                var request = new RestRequest();

                request.Method = Method.Get;

                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", $"Basic {_remoteDeviceSettings.AuthToken}");

                var response = client.Execute(request).Content;

                List<RdpDeviceActivityDto> root = JsonConvert.DeserializeObject<List<RdpDeviceActivityDto>>(response);

                return root;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<RDPDeviceInfo> GetDeviceActivity(string userName)
        {
            var procedureName = "sp_get_devices_with_user";
            var parameters = new[] { new SqlParameter("@user_name", string.IsNullOrEmpty(userName)?"": userName) };
            var result = context.RDPDeviceInfoes.FromSqlRaw($"EXEC {procedureName} @user_name", parameters).ToList();

            return result;
        }

        public string GetDeviceByKey(string key)
        {
            try
            {
                var client = new RestClient($"{_remoteDeviceSettings.BaseUrl}RemoteControl/" + key);

                var request = new RestRequest();

                request.Method = Method.Get;
                request.AddHeader("Authorization", $"Basic {_remoteDeviceSettings.AuthToken}");

                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);

                var response = client.Execute(request);

                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public UserAccount GetDeviceUserByKey(string key)
        {
            try
            {
                UserAccount user = new();

                RDPDeviceInfo rDPDevice = context.RDPDeviceInfoes.Where(x => x.DeviceId == key).FirstOrDefault();

                if (rDPDevice != null)
                {
                    user  = context.UserAccounts.FirstOrDefault(x => x.Username == rDPDevice.UserName);
                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UninstallDeviceByKey(string DeviceId)
        {
            try
            {
                var client = new RestClient($"{_remoteDeviceSettings.BaseUrl}UninstallDevice/" + DeviceId);

                var request = new RestRequest();

                request.Method = Method.Post;
                request.AddHeader("Authorization", $"Basic {_remoteDeviceSettings.AuthToken}");

                var body = @"";
                request.AddParameter("text/plain", body, ParameterType.RequestBody);

                var response = client.Execute(request);

                return response.Content;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}