using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable] public class LensDistortionFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Lens Distortion";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;
        [SerializeField] private bool ignoreTimeScale;
        [SerializeField] private FloatTweenParameter Intensity=new();

        [Header("Lens Distortion")] 
        [DisplayIf(nameof(Intensity),typeof(TweenParameter))]
        [SerializeField] private float intensity;
        [SerializeField,Range(0,1)] private float xMultiplier=1;
        [SerializeField,Range(0,1)] private float yMultiplier=1;
        [SerializeField] private Vector2 center=new (.5f,.5f);
        [SerializeField, Range(0.01f, 5)] private float scale=1;

        private LensDistortion _lensDistortion;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            _lensDistortion = VolumeSingleton.Instance.TryGetVolumeComponent<LensDistortion>();
            VolumeSingleton.Instance.EnableVolumeComponent(_lensDistortion);
            if (!Intensity.IsActive) _lensDistortion.intensity.value = intensity;
            _lensDistortion.xMultiplier.value = xMultiplier;
            _lensDistortion.yMultiplier.value = yMultiplier;
            _lensDistortion.center.value = center;
            _lensDistortion.scale.value = scale;
            
            _tween = Intensity.DoTween(ignoreTimeScale, value=>_lensDistortion.intensity.value=value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(_lensDistortion);
                    Complete();
                });
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}