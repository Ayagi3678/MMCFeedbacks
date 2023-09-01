using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class TimeEffectFX : Feedback
    {
        public override string MenuString => "etc/Time Effect";
        public override Color TagColor => FeedbackStyling.EtcFXColor;
        [Space(10)] 
        [SerializeField] private int priority;
        [SerializeField,Min(0)] private float timeScale;
        [SerializeField,Min(0)] private float durationTime = 1; 
        private TimeEffect _timeEffect;
        protected override void OnReset()
        {
            _timeEffect?.Discard();
        }

        protected override void OnPlay(CancellationToken token)
        {
            _timeEffect = new(priority, timeScale, durationTime);
            TimeSingleton.Instance.SetTimeRequest(_timeEffect);
        }
    }
}