using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ObjectActiveFX : IFeedback
    {
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Object/Active";
        public Color TagColor => FeedbackStyling.ObjectFXColor;
        
        [SerializeField] private Timing timing;
        [Space(10)] [SerializeField] private GameObject target;
        [SerializeField] private bool active = true;
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
            target.SetActive(active);
        }
    }
}