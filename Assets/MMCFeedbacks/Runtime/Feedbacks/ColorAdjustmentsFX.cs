using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ColorAdjustmentsFX : IFeedback
    {
        public int Order => 7;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Volume/Color Adjustments";
        public Color TagColor => FeedbackStyling.VolumeFXColor;
        
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
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
        
        private Sequence _tweenSequence;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            var colorAdjustments = VolumeSingleton.Instance.TryGetVolumeComponent<ColorAdjustments>();
            VolumeSingleton.Instance.EnableVolumeComponent(colorAdjustments);
            if (!Contrast.IsActive) colorAdjustments.contrast.value = contrast;
            if (!HueShift.IsActive) colorAdjustments.hueShift.value = hueShift;
            if (!Satuation.IsActive) colorAdjustments.saturation.value = saturation;
            colorAdjustments.colorFilter.value = colorFilter;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tweenSequence?.Kill();
            State = FeedbackState.Pending;
            PlayAsync(colorAdjustments).Forget();
        }

        public void Stop()
        {
            _tweenSequence.Pause();
        }
        private async UniTaskVoid PlayAsync(ColorAdjustments colorAdjustments)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            _tweenSequence = DOTween.Sequence();
            if (Contrast.IsActive) _tweenSequence.Join(Contrast.DoTween(ignoreTimeScale, value => colorAdjustments.contrast.value = value));
            if (HueShift.IsActive) _tweenSequence.Join(HueShift.DoTween(ignoreTimeScale, value => colorAdjustments.hueShift.value = value));
            if (Satuation.IsActive) _tweenSequence.Join(Satuation.DoTween(ignoreTimeScale, value => colorAdjustments.saturation.value = value));
            _tweenSequence?.OnComplete(() =>
            {
                VolumeSingleton.Instance.DisableVolumeComponent(colorAdjustments);
                State = FeedbackState.Completed;
            });

        }
    }
}