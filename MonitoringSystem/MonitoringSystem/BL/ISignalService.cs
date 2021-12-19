
namespace MonitoringSystem.BL
{
    public interface ISignalService
    {
        List<SignalSource> GetSignalSource();
        List<SignalDTO> GetSineSignals(int page, int size);
        List<SignalDTO> GetStateSignals(int page, int size);
        List<SignalDTO> GetAnimalySignals(int page, int size);
    }
}