using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MMCFeedbacks.Core;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    [Serializable] public class FeedbackPlayerFX : Feedback
    {
        public override int Order => -5;
        public override string MenuString => "etc/Feedback Player";
        public override Color TagColor => FeedbackStyling.EtcFXColor;
        [Space(10)]
        [SerializeField] private FeedbackPlayer feedbackPlayer;

        protected override void OnPlay(CancellationToken token)
        {
            feedbackPlayer.PlayFeedbacks();
            Complete();
        }

        protected override void OnStop()
        {
            feedbackPlayer.StopFeedbacks();
        }
    }
}