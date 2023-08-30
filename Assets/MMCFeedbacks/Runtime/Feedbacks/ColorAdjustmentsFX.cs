using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private ColorAdjustments _colorAdjustments;
        private Sequence _tweenSequence;

        protected override void OnReset()
        {
            _tweenSequence?.Kill();
        }
        protected override void OnPlay()
        {
            _colorAdjustments ??= VolumeSingleton.Instance.TryGetVolumeComponent<ColorAdjustments>();
            VolumeSingleton.Instance.EnableVolumeComponent(_colorAdjustments);
            if (!Contrast.IsActive) _colorAdjustments.contrast.value = contrast;
            if (!HueShift.IsActive) _colorAdjustments.hueShift.value = hueShift;
            if (!Satuation.IsActive) _colorAdjustments.saturation.value = saturation;
            _colorAdjustments.colorFilter.value = colorFilter;
            
            _tweenSequence = DOTween.Sequence();
            if (Contrast.IsActive) _tweenSequence.Join(Contrast.DoTween(_ignoreTimeScale, value => _colorAdjustments.contrast.value = value));
            if (HueShift.IsActive) _tweenSequence.Join(HueShift.DoTween(_ignoreTimeScale, value => _colorAdjustments.hueShift.value = value));
            if (Satuation.IsActive) _tweenSequence.Join(Satuation.DoTween(_ignoreTimeScale, value => _colorAdjustments.saturation.value = value));
            _tweenSequence?.OnComplete(() =>
            {
                VolumeSingleton.Instance.DisableVolumeComponent(_colorAdjustments);
                Complete();
            });
        }
        protected override void OnStop()
        {
            _tweenSequence?.Pause();
        }
    }
}