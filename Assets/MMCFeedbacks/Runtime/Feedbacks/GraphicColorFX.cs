using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

namespace MMCFeedbacks.Core
{
    [Serializable] public class GraphicColorFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "Graphic/Graphic Color";
        public override Color TagColor => FeedbackStyling.GraphicFXColor; 
        [Space(10)]
        [SerializeField] private Graphic target;
        [SerializeField] private bool isReturn;
        [SerializeField] private ColorTweenParameter Color = new();

        private TweenCallback _onKillCache;
        private TweenCallback _onCompleteCache;
        private DOGetter<Color> _getterCache;
        private DOSetter<Color> _setterCache;
        private Color _initialColor;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCache = () => { if (isReturn) target.color = _initialColor; };
            _onCompleteCache = () => { if (isReturn) target.color = _initialColor; Complete(); };
            _getterCache = () => target.color;
            _setterCache = x => target.color=x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _initialColor = target.color;
            _tween = Color.ExecuteTween(ignoreTimeScale,_getterCache,_setterCache)
                .OnKill(_onKillCache)
                .OnComplete(_onCompleteCache);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}