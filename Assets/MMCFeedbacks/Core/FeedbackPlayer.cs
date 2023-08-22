using System.Collections.Generic;
using System.Linq;
using MMCFeedbacks.etc;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    public class FeedbackPlayer : MonoBehaviour
    {
        [SerializeField] private ExecuteMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)ExecuteMode.Loop)] private int loopCount;
        [SerializeField] private bool playAwake;
        [SerializeField] private FeedbackList list=new (new List<IFeedback>());
        public void PlayFeedbacks()
        {
            FeedbackPlayerUtility.ExecuteFeedbacks(list,destroyCancellationToken,mode);
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
                t.OnEnable(gameObject);
            }
        }

        private void OnDisable()
        {
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.OnDisable();
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