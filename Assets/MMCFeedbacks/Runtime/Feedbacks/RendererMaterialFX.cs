using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace MMCFeedbacks.Core
{
    [Serializable] public class RendererMaterialFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "Graphic/Renderer Material";
        public override Color TagColor => FeedbackStyling.GraphicFXColor;
        [Space(10)]
        [SerializeField] private Renderer target;
        [Header("Material")]
        [SerializeField] private string propertyName;
        [SerializeField] private ParameterType type;
        [SerializeField,DisplayIf(nameof(type),0)] private FloatTweenParameter Float = new();
        [SerializeField,DisplayIf(nameof(type),1)] private IntTweenParameter Int = new();
        [SerializeField,DisplayIf(nameof(type),2)] private ColorTweenParameter Color = new();
        [SerializeField,DisplayIf(nameof(type),3)] private Vector3TweenParameter Vector3 =new ();

        private TweenCallback _onCompleteCache;
        private DOGetter<float> _getterFloatCache;
        private DOSetter<float> _setterFloatCache;
        private DOGetter<Color> _getterColorCache;
        private DOSetter<Color> _setterColorCache;
        private Material _targetMaterial;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = () => { Object.Destroy(_targetMaterial);Complete(); };
            _getterFloatCache = () => _targetMaterial.GetFloat(propertyName);
            _setterFloatCache = x => _targetMaterial.SetFloat(propertyName, x);
            _getterColorCache = () => _targetMaterial.GetColor(propertyName);
            _setterColorCache = x => _targetMaterial.SetColor(propertyName, x);
        }
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay(CancellationToken token)
        {
            _targetMaterial = target.material;
            _tween = type switch
            {
                ParameterType.Float => Float.ExecuteTween(_ignoreTimeScale, _getterFloatCache,_setterFloatCache),
                ParameterType.Int => Int.DoTween(_ignoreTimeScale, value => _targetMaterial.SetInt(propertyName, value)),
                ParameterType.Color => Color.ExecuteTween(_ignoreTimeScale, _getterColorCache,_setterColorCache),
                ParameterType.Vector3 => Vector3.DoTween(_ignoreTimeScale,value=>_targetMaterial.SetVector(propertyName,value)),
                _ => throw new ArgumentOutOfRangeException()
            };
            _tween.OnComplete(_onCompleteCache);
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}