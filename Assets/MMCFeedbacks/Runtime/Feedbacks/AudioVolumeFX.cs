﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
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
        
        private TweenCallback _onKillCache;
        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private float _initialVolume;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onKillCache = () => { if (isReturn) target.volume = _initialVolume; };
            _onCompleteCache = () => { if (isReturn) target.volume = _initialVolume; Complete();};
            _getterCache = () => target.volume;
            _setterCache = x => target.volume = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _initialVolume = target.volume;
            _tween=Volume.ExecuteTween(ignoreTimeScale, _getterCache,_setterCache)
                .OnKill(_onKillCache)
                .OnComplete(_onCompleteCache);
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}