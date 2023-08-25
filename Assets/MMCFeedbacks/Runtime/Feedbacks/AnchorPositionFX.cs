using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class AnchorPositionFX : IFeedback
    {
        public int Order => 8;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "RectTransform/Anchor Position";
        public Color TagColor => FeedbackStyling.RectTransformFXColor;
        
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [Space(10)]
        [SerializeField] private RectTransform target;
        [SerializeField] private bool isReturn;
        [SerializeField] private Vector3TweenParameter AnchorPosition=new();

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

            _initialPosition = target.anchoredPosition3D;
            _tween = AnchorPosition.DoTween(ignoreTimeScale,value=>target.anchoredPosition3D=value)
                .OnKill(() =>
                {
                    if (isReturn) target.anchoredPosition3D = _initialPosition;
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.anchoredPosition3D = _initialPosition;
                    State = FeedbackState.Completed;
                });

        }
    }
}