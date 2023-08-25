using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ShakeRotationFX : IFeedback
    {
        public int Order => 9;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Transform/Shake Rotation";
        public Color TagColor => FeedbackStyling.TransformFXColor;
        [SerializeField] private Timing timing = new();
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private GameObject target;
        [SerializeField] private bool isRelative = true;
        [Header("Shake Rotation")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private float duration=1;
        [Space(10)]
        [SerializeField] private float strength=1;
        [SerializeField] private int vibrato = 10;
        [SerializeField] private float randomness = 90f;
        [SerializeField] private bool isFadeOut = true;
        
        private Tween _tween;
        private Vector3 _initialRotation;
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
            _tween?.Pause();
        }
        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            _initialRotation = target.transform.eulerAngles;
            _tween = target.transform.DOShakeRotation(duration,strength,vibrato,randomness,isFadeOut)
                .SetUpdate(ignoreTimeScale)
                .SetRelative(isRelative)
                .OnKill(()=> target.transform.eulerAngles = _initialRotation)
                .OnComplete(() =>
                {
                    target.transform.eulerAngles = _initialRotation;
                    State = FeedbackState.Completed;
                });
            if (mode == EaseMode.Ease) 
                _tween.SetEase(ease);
            else 
                _tween.SetEase(curve);

        }
    }
}