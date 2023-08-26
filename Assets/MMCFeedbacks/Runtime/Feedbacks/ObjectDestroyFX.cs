using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ObjectDestroyFX : IFeedback
    {
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Object/Destroy";
        public Color TagColor => FeedbackStyling.ObjectFXColor;
        
        [SerializeField] private Timing timing;
        [Space(10)] [SerializeField] private GameObject target;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void Play()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            State = FeedbackState.Pending;
            PlayAsync().Forget();
        }

        public void Stop(){}

        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),
                cancellationToken: _cancellationTokenSource.Token);
            State = FeedbackState.Completed;
            Object.Destroy(target);
        }
    }
}