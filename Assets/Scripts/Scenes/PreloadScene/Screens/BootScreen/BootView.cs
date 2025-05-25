using Base.Systems;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;

namespace Base.Scenes.Preload
{
    [AddressablePath("Screens/BootView")]
    public class BootView : View
    {
        [SerializeField] private TextMeshProUGUI _loadingText;
        private string _loadingString = "{0}%";
        private string _loadingFinishedString = "Complete!";

        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTask PlayLoadingAnimation(float duration)
        {
            float loadingPercentage = 0;
            var tween = DOTween.To(() => loadingPercentage, x => loadingPercentage = x, 100, duration)
                .OnUpdate(() =>
                {
                    _loadingText.text = string.Format(_loadingString, Mathf.RoundToInt(loadingPercentage));
                });

            await tween.ToUniTask(cancellationToken: _cancellationTokenSource.Token);
            _loadingText.text = _loadingFinishedString;

            //Adding another second of delay to see the loading finished text 
            await UniTask.Delay(1000, cancellationToken: _cancellationTokenSource.Token);
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Dispose();
        }
    }
}