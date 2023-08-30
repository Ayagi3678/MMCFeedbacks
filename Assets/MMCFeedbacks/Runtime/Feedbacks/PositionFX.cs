using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable] public class PositionFX : Feedback
    {
        public override int Order => 9;
        public override string MenuString => "Transform/Position";
        public override Color TagColor => FeedbackStyling.TransformFXColor;
        [Space(10)] 

        [SerializeField] private SimulationSpace simulationSpace;
        [SerializeField, DisplayIf(nameof(simulationSpace), 2)] private Transform customTransformTarget;
        [Space(5)]
        [SerializeField] private GameObject target;
        [SerializeField] private bool isRelative = true;
        [SerializeField] private bool isReturn;
        [Header("Position")]
        [SerializeField] private EaseMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private Ease ease=Ease.Linear;
        [SerializeField,DisplayIf(nameof(mode),1)]
        [NormalizedAnimationCurve(false)] private AnimationCurve curve=AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private Vector3 zero;
        [SerializeField] private Vector3 one;
        [SerializeField] private float duration=1;
        
        private Vector3 _initialPosition;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            switch (simulationSpace)
            {
                case SimulationSpace.World:
                    _initialPosition = target.transform.position;
                    _tween = target.transform.DOMove(one, duration)
                        .From(zero, true, isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(_ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.position = _initialPosition;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.position = _initialPosition;
                            Complete();
                        });
                    break;
                case SimulationSpace.Local:
                    _initialPosition = target.transform.localPosition;
                    _tween = target.transform.DOLocalMove(one, duration)
                        .From(zero,true,isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(_ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.localPosition = _initialPosition;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.localPosition = _initialPosition;
                            Complete();
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
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}