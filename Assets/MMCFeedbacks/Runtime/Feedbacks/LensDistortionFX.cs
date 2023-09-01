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
        
        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private LensDistortion _lensDistortion;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = () => {  VolumeSingleton.Instance.DisableVolumeComponent(_lensDistortion); Complete();};
            _getterCache = () => _lensDistortion.intensity.value;
            _setterCache = x => _lensDistortion.intensity.value = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _lensDistortion = VolumeSingleton.Instance.TryGetVolumeComponent<LensDistortion>();
            VolumeSingleton.Instance.EnableVolumeComponent(_lensDistortion);
            if (!Intensity.IsActive) _lensDistortion.intensity.value = intensity;
            _lensDistortion.xMultiplier.value = xMultiplier;
            _lensDistortion.yMultiplier.value = yMultiplier;
            _lensDistortion.center.value = center;
            _lensDistortion.scale.value = scale;
            
            _tween = Intensity.ExecuteTween(ignoreTimeScale,_getterCache,_setterCache)
                .OnComplete(_onCompleteCache);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}