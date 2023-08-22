using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.etc;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class VignetteFX : IFeedback
    {
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Volume/Vignette";
        public Color TagColor => FeedbackStyling.volumeFeedbackColor;
        
        [SerializeField] private Timing timing = new();
        [SerializeField] private bool ignoreTimeScale;
        [SerializeField] private FloatTweenParameter Intensity=new();
        [SerializeField] private ColorTweenParameter Color = new();

        [Header("Vignette")] 
        [SerializeField][DisplayIf(nameof(Intensity),typeof(TweenParameter))][Range(0,1)]
        private float intensity;
        [SerializeField][DisplayIf(nameof(Color),typeof(TweenParameter))]
        private Color color;
        [SerializeField] private Vector2 center=new(.5f,.5f);
        [SerializeField,Range(.01f,1f)] private float smoothness=.2f;
        [SerializeField] private bool rounded;
        
        private Tween _tween;
        private GameObject _gameObject;
        public void OnEnable(GameObject gameObject)
        {
            _gameObject = gameObject;
        }
        public void Play()
        {
            var hashCode = GetHashCode();
            var vignette = VolumeSingleton.Instance.TryGetVolumeComponent<Vignette>();
            VolumeSingleton.Instance.EnableVolumeComponent(vignette);
            vignette.center.value = center;
            vignette.smoothness.value = smoothness;
            vignette.rounded.value = rounded;

            _tween?.Kill();
            State = FeedbackState.Pending;
            PlayAsync(vignette).Forget();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
        private async UniTaskVoid PlayAsync(Vignette vignette)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_gameObject.GetCancellationTokenOnDestroy());
            State = FeedbackState.Running;
            
            _tween = Intensity.DoTween(ignoreTimeScale, value=>vignette.intensity.value=value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(vignette);
                    State = FeedbackState.Completed;
                });
        }

        public IFeedback Clone()
        {
            var copy = new VignetteFX()
            {
                IsActive = IsActive,
                timing = timing,
                ignoreTimeScale = ignoreTimeScale,
                Intensity = new FloatTweenParameter(Intensity),
                Color=Color,
                center=center,
                smoothness = smoothness,
                rounded = rounded
            };
            return copy;
        }
    }
}