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

        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterCache;
        private DOSetter<float> _setterCache;
        private FilmGrain _filmGrain;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = () => {  VolumeSingleton.Instance.DisableVolumeComponent(_filmGrain); Complete();};
            _getterCache = () => _filmGrain.intensity.value;
            _setterCache = x => _filmGrain.intensity.value = x;
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _filmGrain ??= VolumeSingleton.Instance.TryGetVolumeComponent<FilmGrain>();
            VolumeSingleton.Instance.EnableVolumeComponent(_filmGrain);
            if (!Intensity.IsActive) _filmGrain.intensity.value = intensity;
            _filmGrain.type.value = type;
            _filmGrain.intensity.value = intensity;
            _filmGrain.response.value = response;
            
            _tween = Intensity.ExecuteTween(ignoreTimeScale, _getterCache,_setterCache)
                .OnComplete(_onCompleteCache);
        }

        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}