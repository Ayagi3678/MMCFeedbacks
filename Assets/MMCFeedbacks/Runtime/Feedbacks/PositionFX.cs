using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
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
        
        private TweenCallback _onKillCacheWorld;
        private TweenCallback _onCompleteCacheWorld;
        private TweenCallback _onKillCacheLocal;
        private TweenCallback _onCompleteCacheLocal;
        private DOGetter<Vector3> _getterCacheWorld;
        private DOSetter<Vector3> _setterCacheWorld;
        private DOGetter<Vector3> _getterCacheLocal;
        private DOSetter<Vector3> _setterCacheLocal;
        private Vector3 _initialPosition;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCacheWorld = () => { if (isReturn) target.transform.position = _initialPosition; };
            _onCompleteCacheWorld = () => { if (isReturn) target.transform.position = _initialPosition; Complete(); };
            _onKillCacheLocal = () => { if (isReturn) target.transform.localPosition = _initialPosition; };
            _onCompleteCacheLocal = () => { if (isReturn) target.transform.localPosition = _initialPosition; Complete(); };
            
            _getterCacheWorld = () => target.transform.position;
            _setterCacheWorld = x => target.transform.position = x;
            _getterCacheLocal = () => target.transform.localPosition;
            _setterCacheLocal = x => target.transform.localPosition = x;
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
                    _initialPosition = target.transform.position;
                    _tween = DOTween.To(_getterCacheWorld,_setterCacheWorld,one,duration)
                        .From(zero, true, isRelative)
                        .SetRelative(isRelative)
                        .SetUpdate(_ignoreTimeScale)
                        .OnKill(_onKillCacheWorld)
                        .OnComplete(_onCompleteCacheWorld);
                    break;
                case SimulationSpace.Local:
                    _initialPosition = target.transform.localPosition;
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