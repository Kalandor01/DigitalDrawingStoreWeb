using DigitalDrawingStore.Listener.Service.Document.Resources;
using DigitalDrawingStore.Listener.Service.Document.Scout;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitalDrawingStore.Listener.Service.Services
{
    internal class DocumentListenerService : IListenerService
    {
        #region Constants
        private const int DELAY_BETWEEN_DOCUMENT_SCOUTING = 5000;
        #endregion

        #region Fields
        private CancellationTokenSource _listenerTaskCancellationTokenSource;
        private readonly IDocumentScout _documentScout;
        private readonly IDocumentResource _documentResource;
        #endregion

        public DocumentListenerService(IDocumentScout documentScout, IDocumentResource documentResource)
        {
            _documentScout = documentScout ?? throw new ArgumentNullException(nameof(documentScout));
            _documentResource = documentResource ?? throw new ArgumentNullException(nameof(documentResource));
        }

        public void StartListening()
        {
            var listenerTask = CreateListenerTask(0, 0);
            listenerTask.Start();
        }

        public async Task StartListeningAsync(int listeningCycles, int millisecondsBetweenCycles)
        {
            var listenerTask = CreateListenerTask(listeningCycles, millisecondsBetweenCycles);

            await Task.Run(() =>
            {
                listenerTask.Start();
                return listenerTask;
            });
        }

        private Task CreateListenerTask(int listeningCycles, int millisecondsBetweenCycles)
        {
            _listenerTaskCancellationTokenSource = new CancellationTokenSource();

            return new Task(() =>
            {
                var listeningCyclesDone = 0;

                while (true && !_listenerTaskCancellationTokenSource.IsCancellationRequested)
                {
                    var documents = _documentScout.FindDocuments();

                    if (documents != null)
                    {
                        _documentResource.SaveDocuments(documents);
                    }

                    listeningCyclesDone++;

                    if (listeningCycles > 0 && listeningCyclesDone == listeningCycles)
                    {
                        _listenerTaskCancellationTokenSource.Cancel();
                    }

                    if (millisecondsBetweenCycles > 0)
                    {
                        Thread.Sleep(millisecondsBetweenCycles);
                    }
                    else
                    {
                        Thread.Sleep(DELAY_BETWEEN_DOCUMENT_SCOUTING);
                    }
                }
            }, _listenerTaskCancellationTokenSource.Token);
        }

        public void StopListening()
        {
            _listenerTaskCancellationTokenSource.Cancel();
        }
    }
}
