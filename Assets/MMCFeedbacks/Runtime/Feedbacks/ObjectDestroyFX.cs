using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ObjectDestroyFX : Feedback
    {
        public override string MenuString => "Object/Destroy";
        public override Color TagColor => FeedbackStyling.ObjectFXColor;
        [Space(10)] [SerializeField] private GameObject target;
        protected override void OnPlay(CancellationToken token)
        {
            Object.Destroy(target);
            Complete();
        }
    }
}