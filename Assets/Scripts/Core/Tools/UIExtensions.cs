
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace BaseArchitecture.Core
{
    /// <summary>
    /// Extension methods for common UI animation patterns.
    /// </summary>
    public static class UIExtensions
    {
        /// <summary>
        /// Fades a CanvasGroup to a target alpha value.
        /// </summary>
        public static async UniTask FadeToAsync(this CanvasGroup canvasGroup, float targetAlpha, float duration, CancellationTokenSource cancellationTokenSource)
        {
            var tween = DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, targetAlpha, duration);
            await tween.ToUniTask(cancellationToken: cancellationTokenSource.Token);
        }

        /// <summary>
        /// Animates a countdown from start value to end value, updating the text field.
        /// </summary>
        public static async UniTask CountdownAsync(this TextMeshProUGUI text, float startValue, float endValue, float duration,
            Action<int> onUpdate, CancellationTokenSource cancellationTokenSource)
        {
            float countdownValue = startValue;
            var tween = DOTween.To(() => countdownValue, x => countdownValue = x, endValue, duration)
                .OnUpdate(() =>
                {
                    int number = Mathf.RoundToInt(countdownValue);
                    text.text = number.ToString();
                    onUpdate?.Invoke(number);
                });
                
            await tween.ToUniTask(cancellationToken: cancellationTokenSource.Token);
        }
    }
}
