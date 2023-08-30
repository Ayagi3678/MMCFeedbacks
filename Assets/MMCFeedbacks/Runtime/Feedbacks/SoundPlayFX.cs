using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class SoundPlayFX : Feedback
    {
        public override int Order => 11;
        public override string MenuString => "Audio/Sound Play";
        public override Color TagColor => FeedbackStyling.AudioFXColor;
        [Space(10)] 
        [SerializeField] private AudioSource target;

        [Header("Sound")] 
        [SerializeField] private PlayMode mode;
        [SerializeField,DisplayIf(nameof(mode),0)] private AudioClip clip;
        [SerializeField,DisplayIf(nameof(mode),0)] private float volumeScale = 1;

        protected override void OnPlay()
        {
            PlayAsync().Forget();
        }

        private async UniTaskVoid PlayAsync()
        {
            switch (mode)
            {
                case PlayMode.PlayOneShot:
                    target.PlayOneShot(clip,volumeScale);
                    await UniTask.WaitUntil(()=>!target.isPlaying,cancellationToken:CancellationTokenSource.Token);
                    target.Stop();
                    Complete();
                    break;
                case PlayMode.PlayAudioSource:
                    target.volume = volumeScale;
                    target.Play();
                    await UniTask.WaitUntil(()=>!target.isPlaying,cancellationToken:CancellationTokenSource.Token);
                    Complete();
                    break;
                case PlayMode.StopAudioSource:
                    target.Stop();
                    Complete();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnStop()
        {
            target.Stop();
        }
        private enum PlayMode
        {
            PlayOneShot,
            PlayAudioSource,
            StopAudioSource
        }
        
    }
}