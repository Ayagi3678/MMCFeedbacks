using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public CancellationTokenSource CancellationTokenSource { get; private set; }

        private void Start()
        {
            if(playAwake) PlayFeedbacks();
        }

        public void PlayFeedbacks()
        {
            ResetCancellationSource();
            IsComplete = false;
            FeedbackPlayerUtility.ExecuteFeedbacks(list,loopCount,CancellationTokenSource.Token,mode);
        }

        public void ResetCancellationSource()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = new();
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.Reset();
            }
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
                t.Enable(gameObject);
            }
        }

        private void OnDestroy()
        {
            CancellationTokenSource?.Cancel();
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.Destroy();
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