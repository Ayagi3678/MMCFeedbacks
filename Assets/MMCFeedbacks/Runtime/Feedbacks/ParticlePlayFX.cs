using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ParticlePlayFX : IFeedback
    {
        public int Order => 4;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Particles/Particle Play";
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

        public void Stop()
        {
            particle.Stop();
        }

        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),
                cancellationToken: _cancellationTokenSource.Token);
            State = FeedbackState.Completed;
            particle.Play(true);
        }
    }
}