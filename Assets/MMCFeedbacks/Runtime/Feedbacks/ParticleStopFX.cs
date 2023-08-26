using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ParticleStopFX : IFeedback
    {
        public int Order => 4;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Particles/Particle Stop";
        public Color TagColor => FeedbackStyling.ParticlesFXColor;
        
        [SerializeField] private Timing timing;
        [Space(10)] 
        [SerializeField] private ParticleSystem particle;
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

        public void Stop(){}

        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),
                cancellationToken: _cancellationTokenSource.Token);
            State = FeedbackState.Completed;
            particle.Stop(true);
        }
    }
}