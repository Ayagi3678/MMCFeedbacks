using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    /*[Serializable]
    public class TimeScaleFX : IFeedback
    {
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "etc/Time Scale";
        public Color TagColor => FeedbackStyling.EtcFXColor;
        [SerializeField] private Timing timing;
        [Space(10)] 
        [SerializeField] private int priority;
        [SerializeField,Min(0)] private float timeScale;
        [SerializeField,Min(0)] private float durationTime=1; 
        private TimeEffect _timeEffect;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        public void Play()
        {
            _timeEffect?.Discard();
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
            _timeEffect = new TimeEffect(priority, timeScale, durationTime);
            TimeSingleton.Instance.SetTimeRequest(_timeEffect);
        }
    }*/
}