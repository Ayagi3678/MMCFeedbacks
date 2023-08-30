using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private Material _targetMaterial;
        private Tween _tween;
        protected override void OnReset()
        {
            _tween?.Kill();
        }
        protected override void OnPlay()
        {
            _targetMaterial = target.material;
            _tween = type switch
            {
                ParameterType.Float => Float.DoTween(_ignoreTimeScale, value => _targetMaterial.SetFloat(propertyName, value)),
                ParameterType.Int => Int.DoTween(_ignoreTimeScale, value => _targetMaterial.SetInt(propertyName, value)),
                ParameterType.Color => Color.DoTween(_ignoreTimeScale, value => _targetMaterial.SetColor(propertyName, value)),
                ParameterType.Vector3 => Vector3.DoTween(_ignoreTimeScale,value=>_targetMaterial.SetVector(propertyName,value)),
                _ => throw new ArgumentOutOfRangeException()
            };
            _tween.OnComplete(() =>
            {
                Complete();
                Object.Destroy(_targetMaterial);
            });
        }
        protected override void OnStop()
        {
            _tween?.Pause();
        }
    }
}