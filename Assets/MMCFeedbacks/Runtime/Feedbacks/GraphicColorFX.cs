using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class GraphicColorFX : IFeedback
    {
        public int Order => 5;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Graphic/Graphic Color";
        public Color TagColor => FeedbackStyling.GraphicFXColor;

        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private Graphic target;
        [SerializeField] private bool isReturn;
        [SerializeField] private TweenMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private ColorTweenParameter Color = new();
        [SerializeField,DisplayIf(nameof(mode),1)] private FloatTweenParameter Alpha = new();
        private Tween _tween;
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


            switch (mode)
            {
                case TweenMode.Color:
                    var initialColor = target.color;
                    _tween = Color.DoTween(ignoreTimeScale,value=>target.color=value)
                        .OnKill(() =>
                        {
                            if (isReturn) target.color = initialColor;
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.color = initialColor;
                            State = FeedbackState.Completed;
                        });
                    break;
                case TweenMode.Alpha:
                    var initialAlpha = target.color.a;
                    _tween = Alpha.DoTween(ignoreTimeScale,value=>target.color=new Color(target.color.r,target.color.g,target.color.b,value))
                        .OnKill(() =>
                        {
                            if (isReturn) target.color = new Color(target.color.r,target.color.g,target.color.b,initialAlpha);
                        })
                        .OnComplete(() =>
                        {
                            if (isReturn) target.color = new Color(target.color.r,target.color.g,target.color.b,initialAlpha);
                            State = FeedbackState.Completed;
                        });
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        private enum TweenMode
        {
            Color,
            Alpha
        }
    }
}