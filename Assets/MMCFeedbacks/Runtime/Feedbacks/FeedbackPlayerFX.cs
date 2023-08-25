using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class FeedbackPlayerFX : IFeedback
    {
        public int Order => -5;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "etc/Feedback Player";
        public Color TagColor => FeedbackStyling.EtcFXColor;
        
        [SerializeField] private Timing timing;
        [Space(10)]
        [SerializeField] private FeedbackPlayer feedbackPlayer;
        
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            State = FeedbackState.Pending;
            PlayAsync().Forget();
        }

        public void Stop()
        {
            feedbackPlayer.StopFeedbacks();
        }

        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),
                cancellationToken: _cancellationTokenSource.Token);
            feedbackPlayer.PlayFeedbacks();
            await UniTask.WaitUntil(() => feedbackPlayer.State == FeedbackState.Completed,
                cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Completed;
        }
    }
}