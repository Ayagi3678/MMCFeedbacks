using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    public class FeedbackPlayer : MonoBehaviour
    {
        [SerializeField] private ExecuteMode mode;
        [SerializeField,DisplayIf(nameof(mode),(int)ExecuteMode.Loop)] private int loopCount;
        [SerializeField] private bool playAwake;
        [SerializeField] private FeedbackList list=new (new List<Feedback>());
        public bool IsComplete { get; private set; }

        private void Start()
        {
            if(playAwake) PlayFeedbacks();
        }

        public void PlayFeedbacks()
        {
            IsComplete = false;
            FeedbackPlayerUtility.ExecuteFeedbacks(list,loopCount,mode);
        }

        public void StopFeedbacks()
        {
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.Stop();
            }
        }

        private void OnEnable()
        {
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.OnEnable(gameObject);
            }
        }

        private void OnDestroy()
        {
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
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