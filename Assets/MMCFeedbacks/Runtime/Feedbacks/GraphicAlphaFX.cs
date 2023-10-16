using System;
using System.Threading;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class GraphicAlphaFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "Graphic/Graphic Alpha";
        public override Color TagColor => FeedbackStyling.GraphicFXColor; 
        [Space(10)]
        [SerializeField] private Graphic target;
        [SerializeField] private bool isReturn;
        [SerializeField] private FloatTweenParameter Alpha = new();
        
        private TweenCallback _onKillCache;
        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private float _initialAlpha;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCache = () => { if (isReturn) target.color = new Color(target.color.r,target.color.g,target.color.b,_initialAlpha); };
            _onCompleteCache = () => { if (isReturn) target.color = new Color(target.color.r,target.color.g,target.color.b,_initialAlpha); Complete(); };
            _getterCache = () => target.color.a;
            _setterCache = x => target.color=new Color(target.color.r,target.color.g,target.color.b,x);
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _initialAlpha = target.color.a;
            _tween = Alpha.ExecuteTween(ignoreTimeScale,_getterCache, _setterCache)
                .OnKill(_onKillCache)
                .OnComplete(_onCompleteCache);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}