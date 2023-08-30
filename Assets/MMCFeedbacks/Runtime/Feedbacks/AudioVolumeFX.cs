using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class AudioVolumeFX : Feedback
    {
        public override int Order => 11;
        public override string MenuString => "Audio/Audio Volume";
        public override Color TagColor => FeedbackStyling.AudioFXColor;
        [Space(10)] 
        [SerializeField] private AudioSource target;
        [SerializeField] private bool isReturn = true;

        [SerializeField] private FloatTweenParameter Volume = new();
        
        private float _initialVolume;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay()
        {
            _initialVolume = target.volume;
            _tween=Volume.DoTween(_ignoreTimeScale, value => target.volume = value)
                .OnKill(() =>
                {
                    if (isReturn) target.volume = _initialVolume;
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.volume = _initialVolume;
                    Complete();
                });
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}