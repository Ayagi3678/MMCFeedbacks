using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
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

        private TweenCallback _onKillCacheWorld;
        private TweenCallback _onCompleteCacheWorld;
        private TweenCallback _onKillCacheLocal;
        private TweenCallback _onCompleteCacheLocal;
        private DOGetter<Vector3> _getterCacheWorld;
        private DOSetter<Vector3> _setterCacheWorld;
        private DOGetter<Vector3> _getterCacheLocal;
        private DOSetter<Vector3> _setterCacheLocal;
        private Vector3 _initialRotation;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCacheWorld = () => { if (isReturn) target.transform.eulerAngles = _initialRotation; };
            _onCompleteCacheWorld = () => { if (isReturn) target.transform.eulerAngles = _initialRotation; Complete(); };
            _onKillCacheLocal = () => { if (isReturn) target.transform.localEulerAngles = _initialRotation; };
            _onCompleteCacheLocal = () => { if (isReturn) target.transform.localEulerAngles = _initialRotation; Complete(); };
            
            _getterCacheWorld = () => target.transform.eulerAngles;
            _setterCacheWorld = x => target.transform.eulerAngles = x;
            _getterCacheLocal = () => target.transform.localEulerAngles;
            _setterCacheLocal = x => target.transform.localEulerAngles = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            switch (simulationSpace)
            {
                case SimulationSpace.World:
                    _initialRotation = target.transform.eulerAngles;
                    _tween = DOTween.To(_getterCacheWorld,_setterCacheWorld,one,duration)
                        .From(zero, true, isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(_ignoreTimeScale)
                        .OnKill(_onKillCacheWorld)
                        .OnComplete(_onCompleteCacheWorld);
                    break;
                case SimulationSpace.Local:
                    _initialRotation = target.transform.localEulerAngles;
                    _tween = DOTween.To(_getterCacheLocal,_setterCacheLocal,one,duration)
                        .From(zero,true,isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(_ignoreTimeScale)
                        .OnKill(_onKillCacheLocal)
                        .OnComplete(_onCompleteCacheLocal);
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