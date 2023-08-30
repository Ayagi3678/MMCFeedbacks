using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ParticleStopFX : Feedback
    {
        public override int Order => 4;
        public override string MenuString => "Particles/Particle Stop";
        public override Color TagColor => FeedbackStyling.ParticlesFXColor;
        [Space(10)] [SerializeField] private ParticleSystem particle;

        protected override void OnPlay()
        {
            particle.Stop(true);
            Complete();
        }
        protected override void OnStop()
        {
            particle.Stop();
        }
    }
}