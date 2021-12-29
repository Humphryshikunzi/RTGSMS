using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RtgsmsApi.Data;
using RtgsmsApi.IRepositories;
using RtgsmsApi.Models.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RtgsmsApi.Repositories
{
    public class SigFoxDeviceRepository : ISigFoxDeviceRepository
    {
        private readonly RtgsmsDbContext _rtgsmsDbContext;
        private readonly ILogger<SigFoxDeviceRepository> _logger;
        public SigFoxDeviceRepository(RtgsmsDbContext rtgsmsDbContext, ILogger<SigFoxDeviceRepository> logger)
        {
            _rtgsmsDbContext = rtgsmsDbContext;
            _logger = logger;
        }

        public async Task AddRtgsmsDevice(SigFoxDeviceModel rtgsmsObject)
        {
            var device = new SigFoxDevice()
            {
                Id = 0,
                Geophone_analog_value = rtgsmsObject.Geophone_analog_value,
                x_acc = rtgsmsObject.x_acc,
                Y_acc = rtgsmsObject.Y_acc,
                Z_acc = rtgsmsObject.Z_acc,
                Data = rtgsmsObject.Data,
                deviceTypeId = rtgsmsObject.DeviceTypeId,
                Hum = rtgsmsObject.Hum,
                Temp = rtgsmsObject.Temp,
                Lat = rtgsmsObject.Lat,
                Device = rtgsmsObject.Device,
                Flags = rtgsmsObject.Flags,
                Ldr = rtgsmsObject.Ldr,
                Long = rtgsmsObject.Long,
                Time = rtgsmsObject.Time
            };
            /*var httpClient = new HttpClient();
            var jsonDataUser = JsonConvert.SerializeObject(device);
            var httpContent = new StringContent(jsonDataUser);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync("http://127.0.0.1:5050/update_realtime", httpContent).ConfigureAwait(false);
            */
            _logger.LogInformation($"RTGSMS Notification:\r\n Received Payload from Device ID :{rtgsmsObject.DeviceTypeId}\n\t DeviceTypeId : {rtgsmsObject.Geophone_analog_value}, Time :{DateTime.Now.TimeOfDay}, Data :{rtgsmsObject.Data}");
            _rtgsmsDbContext.Add(device);
            await _rtgsmsDbContext.SaveChangesAsync();
        }      

        public Task DeleteRtgsmsDeviceById(int id)
        {
            _rtgsmsDbContext.Remove(_rtgsmsDbContext.SigFoxDevices.Find(id));
            return _rtgsmsDbContext.SaveChangesAsync();
        }

        public IEnumerable<SigFoxDevice> GetRecentMessages()
        {
            return _rtgsmsDbContext.SigFoxDevices.OrderByDescending(message => message.Id).Take(20);
        }

        public IEnumerable<SigFoxDevice> GetRtgsmsDevice()
        {
            return _rtgsmsDbContext.SigFoxDevices;
        }

        public SigFoxDevice GetRtgsmsDeviceById(int id)
        {
            return _rtgsmsDbContext.SigFoxDevices.Find(id);
        }

        public Task UpdateRtgsmsDeviceById(SigFoxDevice deviceMessage)
        {
            _rtgsmsDbContext.Update(deviceMessage);
            return _rtgsmsDbContext.SaveChangesAsync();
        }
    }
}
