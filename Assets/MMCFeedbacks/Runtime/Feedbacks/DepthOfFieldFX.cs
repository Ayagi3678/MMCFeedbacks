using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable] public class DepthOfFieldFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Depth Of Field";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;
        [SerializeField] private FloatTweenParameter FocusDistance = new(true);
        [SerializeField] private FloatTweenParameter FocalLength = new(true);

        [Header("Depth Of Field")]
        [SerializeField][DisplayIf(nameof(FocusDistance),typeof(TweenParameter))]
        private float focusDistance=10;
        [SerializeField][DisplayIf(nameof(FocalLength),typeof(TweenParameter))][Range(1,300)]
        private float focalLength=50;
        [SerializeField,Range(1,32)] private float aperture = 5.6f;
        [SerializeField,Range(3,9)] private int bladeCount = 5;
        [SerializeField,Range(0,1)] private float bladeCurvature=1;
        [SerializeField,Range(-180,180)] private float bladeRotation;

        private DepthOfField _depthOfField;
        private Sequence _tweenSequence;
        protected override void OnReset()
        {
            _tweenSequence?.Kill();
        }
        protected override void OnPlay()
        {
            _depthOfField ??= VolumeSingleton.Instance.TryGetVolumeComponent<DepthOfField>();
            VolumeSingleton.Instance.EnableVolumeComponent(_depthOfField);
            if (!FocusDistance.IsActive) _depthOfField.focusDistance.value = focusDistance;
            if (!FocalLength.IsActive) _depthOfField.focalLength.value = focalLength;
            _depthOfField.aperture.value = aperture;
            _depthOfField.bladeCount.value = bladeCount;
            _depthOfField.bladeCurvature.value = bladeCurvature;
            _depthOfField.bladeRotation.value = bladeRotation;
            
            _tweenSequence = DOTween.Sequence();
            if(FocusDistance.IsActive)_tweenSequence.Join(FocusDistance.DoTween(_ignoreTimeScale,value=>_depthOfField.focusDistance.value=value));
            if(FocalLength.IsActive)_tweenSequence.Join(FocalLength.DoTween(_ignoreTimeScale, value => _depthOfField.focalLength.value = value));

            _tweenSequence.OnComplete(() =>
            {
                VolumeSingleton.Instance.DisableVolumeComponent(_depthOfField);
                Complete();
            });
        }
        protected override void OnStop()
        {
            _tweenSequence?.Pause();
        }
    }
}