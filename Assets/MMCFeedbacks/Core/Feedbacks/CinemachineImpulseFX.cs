using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.etc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class CinemachineImpulseFX : IFeedback
    {
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Camera/Cinemachine Impulse";
        public Color TagColor => FeedbackStyling.cameraFeedbackColor;

        [SerializeField] private Timing timing = new();
        [SerializeField] private bool ignoreTimeScale;
        [Header("Cinemachine")] [SerializeField]
        private ImpulseMode mode;
        [Space(5)]
        [SerializeField] [DisplayIf(nameof(mode),(int)ImpulseMode.ImpulseSource)]
        private CinemachineImpulseSource impulseSource;
        [SerializeField][DisplayIf(nameof(mode),(int)ImpulseMode.CollisionImpulseSource)]
        private CinemachineCollisionImpulseSource impulseCollisionSource;
        [SerializeField][DisplayIf(nameof(mode),(int)ImpulseMode.ImpulseDefinition)]
        private CinemachineImpulseDefinition impulseDefinition = new ();

        private GameObject _gameObject;
        public void OnEnable(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void Play()
        {
            State = FeedbackState.Pending;
            CinemachineImpulseManager.Instance.IgnoreTimeScale=ignoreTimeScale;
            PlayAsync().Forget();
        }
        public void Stop()
        {
            CinemachineImpulseManager.Instance.Clear();
        }
        
        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_gameObject.GetCancellationTokenOnDestroy());
            State = FeedbackState.Completed;
            switch(mode)
            {
                case ImpulseMode.ImpulseSource:
                    impulseSource.GenerateImpulse();
                    break;
                case ImpulseMode.CollisionImpulseSource:
                    impulseCollisionSource.GenerateImpulse();
                    break;
                case ImpulseMode.ImpulseDefinition:
                    if (Camera.main != null)
                        impulseDefinition.CreateEvent(Camera.main.transform.position, Random.onUnitSphere);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            };
        }
        public IFeedback Clone()
        {
            var copy = new CinemachineImpulseFX();
            copy.IsActive = IsActive;
            copy.timing = timing;
            copy.mode = mode;
            copy.impulseSource = impulseSource;
            copy.impulseCollisionSource = impulseCollisionSource;
            copy.impulseDefinition = impulseDefinition;
            return copy;
        }
        private enum ImpulseMode
        {
            ImpulseSource,
            CollisionImpulseSource,
            ImpulseDefinition
        }
    }
}