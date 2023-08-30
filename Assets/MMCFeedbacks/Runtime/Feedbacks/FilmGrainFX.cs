using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace MMCFeedbacks.Core
{
    [Serializable] public class FilmGrainFX : Feedback
    {
        public override int Order => 7;
        public override string MenuString => "Volume/Film Grain";
        public override Color TagColor => FeedbackStyling.VolumeFXColor;  
        [SerializeField] private FloatTweenParameter Intensity=new();

        [Header("Film Grain")] 
        [DisplayIf(nameof(Intensity),typeof(TweenParameter))]
        [SerializeField,Range(0,1)] private float intensity;
        [SerializeField] private FilmGrainLookup type;
        [SerializeField,Range(0,1)] private float response = .8f;

        private Tween _tween;
        private FilmGrain _filmGrain;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            _filmGrain ??= VolumeSingleton.Instance.TryGetVolumeComponent<FilmGrain>();
            VolumeSingleton.Instance.EnableVolumeComponent(_filmGrain);
            if (!Intensity.IsActive) _filmGrain.intensity.value = intensity;
            _filmGrain.type.value = type;
            _filmGrain.intensity.value = intensity;
            _filmGrain.response.value = response;
            
            _tween = Intensity.DoTween(_ignoreTimeScale, value => _filmGrain.intensity.value=value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(_filmGrain);
                    Complete();
                });
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
    /*[Serializable]
    public class FilmGrainFX : IFeedback
    {
        public int Order => 7;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set;}
        public string MenuString => "Volume/Film Grain";
        public Color TagColor => FeedbackStyling.VolumeFXColor;

        [SerializeField] private Timing timing;
        [SerializeField] private bool ignoreTimeScale;
        [SerializeField] private FloatTweenParameter Intensity=new();

        [Header("Film Grain")] 
        [DisplayIf(nameof(Intensity),typeof(TweenParameter))]
        [SerializeField,Range(0,1)] private float intensity;
        [SerializeField] private FilmGrainLookup type;
        [SerializeField,Range(0,1)] private float response = .8f;

        private Tween _tween;
        private FilmGrain _filmGrain;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            _filmGrain ??= VolumeSingleton.Instance.TryGetVolumeComponent<FilmGrain>();
            VolumeSingleton.Instance.EnableVolumeComponent(_filmGrain);
            if (!Intensity.IsActive) _filmGrain.intensity.value = intensity;
            _filmGrain.type.value = type;
            _filmGrain.intensity.value = intensity;
            _filmGrain.response.value = response;
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            _tween?.Kill();
            State = FeedbackState.Pending;
            PlayAsync().Forget();
        }

        public void Stop()
        {
            _tween?.Pause();
        }

        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
            State = FeedbackState.Running;
            
            _tween = Intensity.DoTween(ignoreTimeScale, value => _filmGrain.intensity.value=value)
                .OnComplete(() =>
                {
                    VolumeSingleton.Instance.DisableVolumeComponent(_filmGrain);
                    State = FeedbackState.Completed;
                });
        }
    }*/
}