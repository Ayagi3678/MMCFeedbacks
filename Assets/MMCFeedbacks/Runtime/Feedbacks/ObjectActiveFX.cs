using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ObjectActiveFX : Feedback
    {
        public override string MenuString => "Object/Active";
        public override Color TagColor => FeedbackStyling.ObjectFXColor;
        [Space(10)] [SerializeField] private GameObject target;
        [SerializeField] private bool active = true;
        protected override void OnPlay()
        {
            target.SetActive(active);
            Complete();
        }
    }
}