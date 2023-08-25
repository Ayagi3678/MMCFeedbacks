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
    public class DepthOfFieldFX : IFeedback
    {
        public int Order => 7;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Volume/Depth Of Field";
        public Color TagColor => FeedbackStyling.VolumeFXColor;
        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
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

        private Sequence _tweenSequence;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        
        public void Play()
        {
            var depthOfField = VolumeSingleton.Instance.TryGetVolumeComponent<DepthOfField>();
            VolumeSingleton.Instance.EnableVolumeComponent(depthOfField);
            if (!FocusDistance.IsActive) depthOfField.focusDistance.value = focusDistance;
            if (!FocalLength.IsActive) depthOfField.focalLength.value = focalLength;
            depthOfField.aperture.value = aperture;
            depthOfField.bladeCount.value = bladeCount;
            depthOfField.bladeCurvature.value = bladeCurvature;
            depthOfField.bladeRotation.value = bladeRotation;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tweenSequence?.Kill();
            State = FeedbackState.Pending;
            PlayAsync(depthOfField).Forget();
        }

        public void Stop()
        {
            _tweenSequence.Pause();
        }
        private async UniTaskVoid PlayAsync(DepthOfField depthOfField)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;

            _tweenSequence = DOTween.Sequence();
            if(FocusDistance.IsActive)_tweenSequence.Join(FocusDistance.DoTween(ignoreTimeScale,value=>depthOfField.focusDistance.value=value));
            if(FocalLength.IsActive)_tweenSequence.Join(FocalLength.DoTween(ignoreTimeScale, value => depthOfField.focalLength.value = value));

            _tweenSequence.OnComplete(() =>
            {
                VolumeSingleton.Instance.DisableVolumeComponent(depthOfField);
                State = FeedbackState.Completed;
            });

        }
    }
}