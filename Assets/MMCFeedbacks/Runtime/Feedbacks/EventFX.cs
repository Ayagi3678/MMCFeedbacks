using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Events;

namespace MMCFeedbacks.Core
{
    [Serializable] public class EventFX : Feedback
    {
        public override int Order => -5;
        public override string MenuString => "etc/Event";
        public override Color TagColor => FeedbackStyling.EtcFXColor;
        [Space(10)]
        [SerializeField] private UnityEvent @event;

        protected override void OnPlay(CancellationToken token)
        {
            @event.Invoke();
            Complete();
        }
    }
}