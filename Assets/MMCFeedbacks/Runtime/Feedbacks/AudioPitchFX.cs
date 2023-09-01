using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
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
        
        private TweenCallback _onKillCache;
        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private float _initialPitch;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCache = () => { if (isReturn) target.pitch = _initialPitch; target.Stop(); };
            _onCompleteCache = () => { if (isReturn) target.pitch = _initialPitch; target.Stop(); Complete(); };
            _getterCache = () => target.pitch;
            _setterCache = x => target.pitch = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialPitch = target.pitch;
            _tween=Pitch.ExecuteTween(_ignoreTimeScale, _getterCache,_setterCache)
                .OnKill(_onKillCache)
                .OnComplete(_onCompleteCache);
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}