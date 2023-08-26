using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class SoundFX : IFeedback
    {
        public int Order => 11;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Audio/Sound";
        public Color TagColor => FeedbackStyling.AudioFXColor;
        [SerializeField] private Timing timing;
        [Space(10)] 
        [SerializeField] private AudioSource target;

        [Header("Sound")] 
        [SerializeField] private PlayMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private AudioClip clip;
        [SerializeField,DisplayIf(nameof(mode),0)] private float volumeScale = 1;
        [SerializeField, DisplayIf(nameof(mode), 0)] private float pitchScale = 1;
        private float _initialPitch;
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
            target.Stop();
        }
        private async UniTaskVoid PlayAsync()
        {
            OnComplete();
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            _initialPitch = target.pitch;
            switch (mode)
            {
                case PlayMode.PlayOneShot:
                    target.pitch *= pitchScale;
                    target.PlayOneShot(clip,volumeScale);
                    await UniTask.Delay(TimeSpan.FromSeconds(clip.length/target.pitch),cancellationToken:_cancellationTokenSource.Token);
                    OnComplete();
                    State = FeedbackState.Completed;
                    break;
                case PlayMode.PlayAudioSource:
                    target.volume = volumeScale;
                    target.pitch *= pitchScale;
                    target.Play();
                    await UniTask.Delay(TimeSpan.FromSeconds(clip.length/target.pitch),cancellationToken:_cancellationTokenSource.Token);
                    OnComplete();
                    State = FeedbackState.Completed;
                    break;
                case PlayMode.StopAudioSource:
                    target.Stop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void OnComplete()
        {
            if(target.pitch>0) target.pitch = _initialPitch;
        }
        private enum PlayMode
        {
            PlayOneShot,
            PlayAudioSource,
            StopAudioSource
        }
    }
}