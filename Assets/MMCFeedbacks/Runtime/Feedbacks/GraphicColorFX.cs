using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        [SerializeField] private TweenMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private ColorTweenParameter Color = new();
        [SerializeField,DisplayIf(nameof(mode),1)] private FloatTweenParameter Alpha = new();

        private Color _initialColor;
        private float _initialAlpha;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            switch (mode)
            {
                case TweenMode.Color:
                    _initialColor = target.color;
                    _tween = Color.DoTween(_ignoreTimeScale,value=>target.color=value)
                        .OnKill(() =>
                        {
                            if (isReturn) target.color = _initialColor;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.color = _initialColor;
                            Complete();
                        });
                    break;
                case TweenMode.Alpha:
                    _initialAlpha = target.color.a;
                    _tween = Alpha.DoTween(_ignoreTimeScale,value=>target.color=new Color(target.color.r,target.color.g,target.color.b,value))
                        .OnKill(() =>
                        {
                            if (isReturn) target.color = new Color(target.color.r,target.color.g,target.color.b,_initialAlpha);
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.color = new Color(target.color.r,target.color.g,target.color.b,_initialAlpha);
                            Complete();
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }

        private enum TweenMode
        {
            Color,
            Alpha
        }
    }
}