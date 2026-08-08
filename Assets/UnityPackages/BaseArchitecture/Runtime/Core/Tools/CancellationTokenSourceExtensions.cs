using System.Threading;

namespace BaseArchitecture.Core
{
    public static class CancellationTokenSourceExtensions
    {
        /// <summary>Cancels and disposes the source. Null and already cancelled sources are safe.</summary>
        public static void CancelAndDispose(this CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
                return;

            if (!cancellationTokenSource.IsCancellationRequested)
                cancellationTokenSource.Cancel();
                
            cancellationTokenSource.Dispose();
        }
    }
}
