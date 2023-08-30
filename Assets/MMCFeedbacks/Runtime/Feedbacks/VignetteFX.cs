using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable] public class VignetteFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Vignette";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;
        [SerializeField] private FloatTweenParameter Intensity=new(true);
        [SerializeField] private ColorTweenParameter Color = new(true);

        [Header("Vignette")] 
        [SerializeField][DisplayIf(nameof(Intensity),typeof(TweenParameter))][Range(0,1)]
        private float intensity;
        [SerializeField][DisplayIf(nameof(Color),typeof(TweenParameter))]
        private Color color;
        [SerializeField] private Vector2 center=new(.5f,.5f);
        [SerializeField,Range(.01f,1f)] private float smoothness=.2f;
        [SerializeField] private bool rounded;

        private Vignette _vignette;
        private Sequence _tweenSequence;
        protected override void OnReset()
        {
            _tweenSequence?.Kill();
        }

        protected override void OnPlay()
        {
            _vignette = VolumeSingleton.Instance.TryGetVolumeComponent<Vignette>();
            VolumeSingleton.Instance.EnableVolumeComponent(_vignette);
            if (!Intensity.IsActive) _vignette.intensity.value = intensity;
            if (!Color.IsActive) _vignette.color.value = color;
            _vignette.center.value = center;
            _vignette.smoothness.value = smoothness;
            _vignette.rounded.value = rounded;
            
            _tweenSequence = DOTween.Sequence();
            _tweenSequence
                .Join(Intensity.DoTween(_ignoreTimeScale, value => _vignette.intensity.value = value))
                .Join(Color.DoTween(_ignoreTimeScale, value => _vignette.color.value = value));
            _tweenSequence.OnComplete(() =>
            {
                VolumeSingleton.Instance.DisableVolumeComponent(_vignette);
                Complete();
            });
        }

        protected override void OnStop()
        {
            _tweenSequence?.Pause();
        }
    }
}