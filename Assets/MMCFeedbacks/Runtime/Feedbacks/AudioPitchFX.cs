using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class AudioPitchFX : Feedback
    {
        public override int Order => 11;
        public override string MenuString => "Audio/Audio Pitch";
        public override Color TagColor => FeedbackStyling.AudioFXColor;
        [Space(10)] 
        [SerializeField] private AudioSource target;
        [SerializeField] private bool isReturn = true;

        [SerializeField] private FloatTweenParameter Pitch = new();

        private float _initialPitch;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay()
        {
            _initialPitch = target.pitch;
            _tween=Pitch.DoTween(_ignoreTimeScale, value => target.pitch = value)
                .OnKill(()=>
                {
                    if (isReturn) target.pitch = _initialPitch;
                    target.Stop();
                })
                .OnComplete(() =>
                {
                    if (isReturn) target.pitch = _initialPitch;
                    target.Stop();
                    Complete();
                });
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}