using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class PositionFX : IFeedback
    {
        public int Order => 9;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Transform/Position";
        public Color TagColor => FeedbackStyling.TransformFXColor;
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)] 

        [SerializeField] private SimulationSpace simulationSpace;
        [SerializeField] private GameObject target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool isReturn;
        [Header("Position")]
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

            
            switch (simulationSpace)
            {
                case SimulationSpace.World:
                    _initialPosition = target.transform.position;
                    _tween = target.transform.DOMove(one, duration)
                        .From(zero, true, isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.position = _initialPosition;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.position = _initialPosition;
                            State = FeedbackState.Completed;
                        });
                    break;
                case SimulationSpace.Local:
                    _initialPosition = target.transform.localPosition;
                    _tween = target.transform.DOLocalMove(one, duration)
                        .From(zero,true,isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.localPosition = _initialPosition;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.localPosition = _initialPosition;
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
    public enum SimulationSpace
    {
        World,
        Local,
        CustomTarget
    }
}