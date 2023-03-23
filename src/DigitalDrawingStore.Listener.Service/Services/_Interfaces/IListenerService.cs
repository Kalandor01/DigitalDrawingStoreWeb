using System.Threading.Tasks;

namespace DigitalDrawingStore.Listener.Service.Services
{
    internal interface IListenerService
    {
        void StartListening();
        Task StartListeningAsync(int listeningCycles, int millisecondsBetweenCycles);
        void StopListening();
    }
}
