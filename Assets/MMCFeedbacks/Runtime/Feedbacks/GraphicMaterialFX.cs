using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class GraphicMaterialFX : IFeedback
    {
        public int Order => 5;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Graphic/Graphic Material";
        public Color TagColor => FeedbackStyling.GraphicFXColor;

        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private Graphic target;
        [Header("Material")]
        [SerializeField] private string propertyName;
        [SerializeField] private ParameterType type;
        [SerializeField,DisplayIf(nameof(type),0)] private FloatTweenParameter Float = new();
        [SerializeField,DisplayIf(nameof(type),1)] private IntTweenParameter Int = new();
        [SerializeField,DisplayIf(nameof(type),2)] private ColorTweenParameter Color = new();
        private Tween _tween;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tween?.Kill();
            State = FeedbackState.Pending;
            PlayAsync().Forget();
        }

        public void Stop()
        {
            _tween.Pause();
        }
        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            var targetMaterial = target.material;
            _tween = type switch
            {
                ParameterType.Float => Float.DoTween(ignoreTimeScale, value => targetMaterial.SetFloat(propertyName, value)),
                ParameterType.Int => Int.DoTween(ignoreTimeScale, value => targetMaterial.SetInt(propertyName, value)),
                ParameterType.Color => Color.DoTween(ignoreTimeScale,
                    value => targetMaterial.SetColor(propertyName, value)),
                _ => throw new ArgumentOutOfRangeException()
            };
            _tween.OnComplete(() =>
            {
                State = FeedbackState.Completed;
                Object.Destroy(targetMaterial);
            });
        }
    }
}