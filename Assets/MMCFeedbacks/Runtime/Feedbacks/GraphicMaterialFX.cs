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
    [Serializable] public class GraphicMaterialFX : Feedback
    {
        public override int Order => 5;
        public override string MenuString => "Graphic/Graphic Material";
        public override Color TagColor => FeedbackStyling.GraphicFXColor;
        [Space(10)]
        [SerializeField] private Graphic target;
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
        private DOGetter<int> _getterIntCache;
        private DOSetter<int> _setterIntCache;
        private DOGetter<Color> _getterColorCache;
        private DOSetter<Color> _setterColorCache;
        private DOGetter<Vector3> _getterVector3Cache;
        private DOSetter<Vector3> _setterVector3Cache;
        private Material _targetMaterial;
        private Tween _tween;
        protected override void OnEnable(GameObject gameObject)
        {
            _onCompleteCache = () => { Object.Destroy(_targetMaterial);Complete(); };
            _getterFloatCache = () => _targetMaterial.GetFloat(propertyName);
            _setterFloatCache = x => _targetMaterial.SetFloat(propertyName, x);
            _getterIntCache = () => _targetMaterial.GetInt(propertyName);
            _setterIntCache = x => _targetMaterial.SetInt(propertyName, x);
            _getterColorCache = () => _targetMaterial.GetColor(propertyName);
            _setterColorCache = x => _targetMaterial.SetColor(propertyName, x);
            _getterVector3Cache = () => _targetMaterial.GetVector(propertyName);
            _setterVector3Cache = x => _targetMaterial.SetVector(propertyName, x);
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
                ParameterType.Int => Int.DoTween(_ignoreTimeScale, _getterIntCache,_setterIntCache),
                ParameterType.Color => Color.ExecuteTween(_ignoreTimeScale, _getterColorCache,_setterColorCache),
                ParameterType.Vector3 => Vector3.DoTween(_ignoreTimeScale,_getterVector3Cache,_setterVector3Cache),
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