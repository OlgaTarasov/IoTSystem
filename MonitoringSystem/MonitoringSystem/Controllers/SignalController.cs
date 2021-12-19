using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonitoringSystem.BL;

namespace MonitoringSystem.Controllers
{
    [Route("signal")]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private readonly ISignalService _signalService = new SignalService();

        [HttpGet("source")]
        public IEnumerable<SignalSource> GetSignalSourceList()
        {
            return _signalService.GetSignalSource();
        }

        [HttpGet("sine/page{page}/size{size}")]
        public IEnumerable<SignalDTO> GetSineSignalList(int page, int size)
        {
            return _signalService.GetSineSignals(page, size);
        }

        [HttpGet("state/page{page}/size{size}")]
        public IEnumerable<SignalDTO> GetStateSignalList(int page, int size)
        {
            return _signalService.GetStateSignals(page, size);
        }

        [HttpGet("anomaly/page{page}/size{size}")]
        public IEnumerable<SignalDTO> GetAnimalySignalList(int page, int size)
        {
            return _signalService.GetAnimalySignals(page, size);
        }
    }
}
