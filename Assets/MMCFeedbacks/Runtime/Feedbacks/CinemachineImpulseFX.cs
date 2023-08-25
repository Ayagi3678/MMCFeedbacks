using System;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class CinemachineImpulseFX : IFeedback
    {
        public int Order => 10;
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Camera/Cinemachine Impulse";
        public Color TagColor => FeedbackStyling.CameraFXColor;

        [SerializeField] private Timing timing;
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
        
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            _cancellationTokenSource = new();
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
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),cancellationToken:_cancellationTokenSource.Token);
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
        private enum ImpulseMode
        {
            ImpulseSource,
            CollisionImpulseSource,
            ImpulseDefinition
        }
    }
}