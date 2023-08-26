using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    public class FeedbackPlayer : MonoBehaviour
    {
        [SerializeField] private ExecuteMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)ExecuteMode.Loop)] private int loopCount;
        [SerializeField] private bool playAwake;
        [SerializeField] private FeedbackList list=new (new List<IFeedback>());
        public bool IsComplete { get; private set; }

        private void Start()
        {
            if(playAwake) PlayFeedbacks();
        }

        public void PlayFeedbacks()
        {
            IsComplete = false;
            FeedbackPlayerUtility.ExecuteFeedbacks(list,loopCount,destroyCancellationToken,(() => IsComplete=true),mode);
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