using System.Threading;

namespace Base.Systems
{
    public static class CancellationTokenSourceExtensions
    {
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
