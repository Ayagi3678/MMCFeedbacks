using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class RotationFX : Feedback
    {
        public override int Order => 9;
        public override string MenuString => "Transform/Rotation";
        public override Color TagColor => FeedbackStyling.TransformFXColor;
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

        
        private Vector3 _initialRotation;
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
                    _initialRotation = target.transform.eulerAngles;
                    _tween = target.transform.DORotate(one, duration, RotateMode.FastBeyond360)
                        .From(zero, true, isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(_ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.eulerAngles = _initialRotation;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.eulerAngles = _initialRotation;
                            Complete();
                        });
                    break;
                case SimulationSpace.Local:
                    _initialRotation = target.transform.localEulerAngles;
                    _tween = target.transform.DOLocalRotate(one, duration, RotateMode.FastBeyond360)
                        .From(zero,true,isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(_ignoreTimeScale)
                        .OnKill(() =>
                        {
                            if (isReturn) target.transform.localEulerAngles = _initialRotation;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.transform.localEulerAngles = _initialRotation;
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