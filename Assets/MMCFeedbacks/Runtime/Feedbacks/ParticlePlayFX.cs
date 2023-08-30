using System;
using UnityEngine;

namespace MMCFeedbacks.Core
{

    [Serializable] public class ParticlePlayFX : Feedback
    {
        public override int Order => 4;
        public override string MenuString => "Particles/Particle Play";
        public override Color TagColor => FeedbackStyling.ParticlesFXColor;
        [Space(10)] [SerializeField] private ParticleSystem particle;

        protected override void OnPlay()
        {
            particle.Play(true);
            Complete();
        }

        protected override void OnStop()
        {
            particle.Stop();
        }
    }
}
