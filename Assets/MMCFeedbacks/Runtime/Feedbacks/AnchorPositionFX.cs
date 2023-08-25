using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class AnchorPositionFX : IFeedback
    {
        public int Order => 8;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "RectTransform/Anchor Position";
        public Color TagColor => FeedbackStyling.RectTransformFXColor;
        
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private RectTransform target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool isReturn;
        [Header("Anchor Position")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        private Tween _tween;
        private Vector3 _initialPosition;
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

            _initialPosition = target.anchoredPosition3D;
            _tween = target.DOAnchorPos3D(one, duration)
                .From(zero, true, isRelative)
                .SetRelative(isRelative)
                .SetUpdate(ignoreTimeScale)
                .OnKill(() =>
                {
                    if (isReturn) target.anchoredPosition3D = _initialPosition;
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.anchoredPosition3D = _initialPosition;
                    State = FeedbackState.Completed;
                });
            
            if (mode == EaseMode.Ease) 
                _tween.SetEase(ease);
            else 
                _tween.SetEase(curve);

        }
    }
}