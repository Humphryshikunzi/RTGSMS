using RtgsmsApi.Models.Device;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RtgsmsApi.IRepositories
{
    public interface ISigFoxDeviceRepository
    {
        Task AddRtgsmsDevice(SigFoxDeviceModel deviceMessage);
        IEnumerable<SigFoxDevice> GetRtgsmsDevice();
        IEnumerable<SigFoxDevice> GetRecentMessages();
        SigFoxDevice GetRtgsmsDeviceById(int id);
        Task DeleteRtgsmsDeviceById(int id);
        Task UpdateRtgsmsDeviceById(SigFoxDevice deviceMessage);
    }
}
