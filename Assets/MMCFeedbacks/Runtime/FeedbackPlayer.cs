using System.Collections.Generic;
using System.Linq;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    public class FeedbackPlayer : MonoBehaviour
    {
        [SerializeField] private ExecuteMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)ExecuteMode.Loop)] private int loopCount;
        [SerializeField] private bool playAwake;
        [SerializeField] private FeedbackList list=new (new List<IFeedback>());
        public FeedbackState State { get; private set; }
        
        public void PlayFeedbacks()
        {
            State = FeedbackState.Running;
            FeedbackPlayerUtility.ExecuteFeedbacks(list,loopCount,destroyCancellationToken,(() => State=FeedbackState.Completed),mode);
        }

        public void StopFeedbacks()
        {
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.Stop();
            }
        }

        private void OnEnable()
        {
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.OnEnable();
            }
        }

        private void OnDestroy()
        {
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.OnDestroy();
            }
        }
    }
    public enum ExecuteMode
    {
        Concurrent,
        Sequence,
        Loop
    }
}