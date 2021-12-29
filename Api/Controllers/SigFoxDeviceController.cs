using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RtgsmsApi.IRepositories;
using RtgsmsApi.Models.Device;
using System.Threading.Tasks;

namespace RtgsmsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RtgsmsController : ControllerBase
    {
        private readonly ISigFoxDeviceRepository _rtgsmsRepository;
        private readonly ILogger<ISigFoxDeviceRepository> _logger;
        public RtgsmsController(ISigFoxDeviceRepository rtgsmsRepository, ILogger<ISigFoxDeviceRepository> logger)
        {
            _rtgsmsRepository = rtgsmsRepository;
            _logger = logger;
        }

        [HttpPost("addRtgsmsObject")]
        public async Task<ActionResult> AddRtgsmsMessage(SigFoxDeviceModel rtgsmsObject)
        {
            await _rtgsmsRepository.AddRtgsmsDevice(rtgsmsObject);
            return Ok();
        }

        [HttpGet("all")]
        public ActionResult GetAll()
        {
            return Ok(_rtgsmsRepository.GetRtgsmsDevice());
        }
        [HttpGet("recentMessages")]
        public ActionResult GetRecent()
        {
            return Ok(_rtgsmsRepository.GetRecentMessages());
        }

        [HttpGet("getbyid/{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_rtgsmsRepository.GetRtgsmsDeviceById(id));
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(SigFoxDevice deviceMessage)
        {
            if (ModelState.IsValid)
            {
                await _rtgsmsRepository.UpdateRtgsmsDeviceById(deviceMessage);
                return Ok(deviceMessage);
            }
            return BadRequest();
        }

        [HttpDelete("delete/{id}")]
        public ActionResult Delete(int id)
        {
            var help = _rtgsmsRepository.GetRtgsmsDeviceById(id);
            if (help != null)
            {
                _rtgsmsRepository.DeleteRtgsmsDeviceById(id);
                return Ok(help);
            }
            return BadRequest();
        }
    }
}
