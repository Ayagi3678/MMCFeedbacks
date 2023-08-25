using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class RotationFX : IFeedback
    {
        public int Order => 9;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Transform/Rotation";
        public Color TagColor => FeedbackStyling.TransformFXColor;
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)] 

        [SerializeField] private SimulationSpace simulationSpace;
        [SerializeField] private GameObject target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool isReturn;
        [Header("Rotation")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Ease)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),(int)EaseMode.Curve)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;

        
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
            _tween.Pause();
        }
        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            
            switch (simulationSpace)
            {
                case SimulationSpace.World:
                    _initialRotation = target.transform.eulerAngles;
                    _tween = target.transform.DORotate(one, duration, RotateMode.FastBeyond360)
                        .From(zero, true, isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.eulerAngles = _initialRotation;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.eulerAngles = _initialRotation;
                            State = FeedbackState.Completed;
                        });
                    break;
                case SimulationSpace.Local:
                    _initialRotation = target.transform.localEulerAngles;
                    _tween = target.transform.DOLocalRotate(one, duration, RotateMode.FastBeyond360)
                        .From(zero,true,isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.localEulerAngles = _initialRotation;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.localEulerAngles = _initialRotation;
                            State = FeedbackState.Completed;
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
            if (mode == EaseMode.Ease) 
                _tween.SetEase(ease);
            else 
                _tween.SetEase(curve);

        }
    }
}