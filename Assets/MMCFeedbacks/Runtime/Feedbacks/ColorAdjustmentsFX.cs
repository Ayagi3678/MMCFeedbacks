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
    [Serializable] public class ColorAdjustmentsFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Color Adjustments";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;
        [SerializeField] private FloatTweenParameter Contrast=new(true);
        [SerializeField] private FloatTweenParameter HueShift=new(true);
        [SerializeField] private FloatTweenParameter Satuation = new(true);
        [Header("Color Adjustments")] 
        [SerializeField][DisplayIf(nameof(Contrast),typeof(TweenParameter))][Range(-100,100)]
        private float contrast;
        [SerializeField] private Color colorFilter=Color.white;
        [SerializeField][DisplayIf(nameof(HueShift),typeof(TweenParameter))][Range(-180,180)] 
        private float hueShift;
        [SerializeField][DisplayIf(nameof(Satuation),typeof(TweenParameter))][Range(-100,100)] 
        private float saturation;
        
        private DOGetter<float> _getterContrastCache;
        private DOSetter<float> _setterContrastCache;
        private DOGetter<float> _getterHueShiftCache;
        private DOSetter<float> _setterHueShiftCache;
        private DOGetter<float> _getterSaturationCache;
        private DOSetter<float> _setterSaturationCache;
        private TweenCallback _onCompleteCache;
        private ColorAdjustments _colorAdjustments;
        private Sequence _tweenSequence;

        protected override void OnEnable(GameObject gameObject)
        {
            _getterContrastCache = () => _colorAdjustments.contrast.value;
            _setterContrastCache = x => _colorAdjustments.contrast.value = x;
            _getterHueShiftCache = () => _colorAdjustments.hueShift.value;
            _setterHueShiftCache = x => _colorAdjustments.hueShift.value = x;
            _getterSaturationCache = () => _colorAdjustments.saturation.value;
            _setterSaturationCache = x => _colorAdjustments.saturation.value = x;
            _onCompleteCache = () => { VolumeSingleton.Instance.DisableVolumeComponent(_colorAdjustments); Complete(); };
        }
        protected override void OnReset()
        {
            _tweenSequence?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _colorAdjustments ??= VolumeSingleton.Instance.TryGetVolumeComponent<ColorAdjustments>();
            VolumeSingleton.Instance.EnableVolumeComponent(_colorAdjustments);
            if (!Contrast.IsActive) _colorAdjustments.contrast.value = contrast;
            if (!HueShift.IsActive) _colorAdjustments.hueShift.value = hueShift;
            if (!Satuation.IsActive) _colorAdjustments.saturation.value = saturation;
            _colorAdjustments.colorFilter.value = colorFilter;
            
            _tweenSequence = DOTween.Sequence().OnComplete(_onCompleteCache);
            if (Contrast.IsActive) _tweenSequence.Join(Contrast.ExecuteTween(ignoreTimeScale, _getterContrastCache,_setterContrastCache));
            if (HueShift.IsActive) _tweenSequence.Join(HueShift.ExecuteTween(ignoreTimeScale, _getterHueShiftCache,_setterHueShiftCache));
            if (Satuation.IsActive) _tweenSequence.Join(Satuation.ExecuteTween(ignoreTimeScale, _getterSaturationCache,_setterSaturationCache));
        }
        protected override void OnStop()
        {
            _tweenSequence?.Pause();
        }
    }
}