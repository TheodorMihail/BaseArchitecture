using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Zenject;

namespace Base.Project
{
    public interface IAuthenticationManager : IInitializable, IDisposable
    {
        UniTask<bool> LoginUser((string username, string password) credentials);
        UniTask<bool> RegisterUser((string username, string password) credentials);
        void LogOut();
    }

    public class AuthenticationManager : IAuthenticationManager
    {
        private CancellationTokenSource _cancellationTokenSource;
        private const int _serverResponseSimulation = 3000;

        public void Initialize()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        public async UniTask<bool> LoginUser((string username, string password) credentials)
        {
            //simulate login process with success
            await UniTask.Delay(_serverResponseSimulation, cancellationToken: _cancellationTokenSource.Token);
            return true;
        }

        public async UniTask<bool> RegisterUser((string username, string password) credentials)
        {
            //simulate register process with success
            await UniTask.Delay(_serverResponseSimulation, cancellationToken: _cancellationTokenSource.Token);
            return true;
        }

        public void LogOut()
        {
            //simulate logout process
        }
    }
}