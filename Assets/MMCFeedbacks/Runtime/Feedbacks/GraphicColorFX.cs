using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class GraphicColorFX : IFeedback
    {
        public int Order => 8;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Graphic/Graphic Color";
        public Color TagColor => FeedbackStyling.GraphicFXColor;

        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private Graphic target;
        [SerializeField] private bool isReturn;
        [Header("Color")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Color zero=Color.black;
        [SerializeField] private Color one=Color.white;
        [SerializeField] private float duration=1;

        private Color _initialColor;
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
            
            _initialColor = target.color;
            _tween = target.DOColor(one,duration)
                .From(zero)
                .SetUpdate(ignoreTimeScale)
                .OnKill(() =>
                {
                    if (isReturn) target.color = _initialColor;
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.color = _initialColor;
                    State = FeedbackState.Completed;
                });
            
            if (mode == EaseMode.Ease) 
                _tween.SetEase(ease);
            else 
                _tween.SetEase(curve);
        }
    }
}