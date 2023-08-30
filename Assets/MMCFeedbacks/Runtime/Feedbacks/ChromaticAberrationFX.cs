using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private ChromaticAberration _chromaticAberration;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }

        protected override void OnPlay()
        {
            _chromaticAberration ??= VolumeSingleton.Instance.TryGetVolumeComponent<ChromaticAberration>();
            VolumeSingleton.Instance.EnableVolumeComponent(_chromaticAberration);
            if (!Intensity.IsActive) _chromaticAberration.intensity.value = intensity;
            
            _tween = Intensity.DoTween(_ignoreTimeScale, value => _chromaticAberration.intensity.value = value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(_chromaticAberration);
                    Complete();
                });
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}