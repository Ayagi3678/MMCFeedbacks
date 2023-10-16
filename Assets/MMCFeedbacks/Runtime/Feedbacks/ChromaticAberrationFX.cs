using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ChromaticAberrationFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Chromatic Aberration";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;
        [SerializeField] private FloatTweenParameter Intensity=new();

        [Header("Chromatic Aberration")] 
        [SerializeField][DisplayIf(nameof(Intensity),typeof(TweenParameter))][Range(0,1)]
        private float intensity;

        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private ChromaticAberration _chromaticAberration;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = () => {  VolumeSingleton.Instance.DisableVolumeComponent(_chromaticAberration); Complete();};
            _getterCache = () => _chromaticAberration.intensity.value;
            _setterCache = x => _chromaticAberration.intensity.value = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _chromaticAberration ??= VolumeSingleton.Instance.TryGetVolumeComponent<ChromaticAberration>();
            VolumeSingleton.Instance.EnableVolumeComponent(_chromaticAberration);
            if (!Intensity.IsActive) _chromaticAberration.intensity.value = intensity;
            
            _tween = Intensity.ExecuteTween(ignoreTimeScale, _getterCache,_setterCache)
                .OnComplete(_onCompleteCache);
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}