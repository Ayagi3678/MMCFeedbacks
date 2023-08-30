using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace MMCFeedbacks.Core
{
    public static class FeedbackPlayerUtility
    {
        public static void ExecuteFeedbacks(FeedbackList list,int loopCount,ExecuteMode mode = ExecuteMode.Concurrent)
        {
            switch (mode)
            {
                case ExecuteMode.Concurrent:
                    ConcurrentExecute(list);
                    break;
                case ExecuteMode.Sequence:
                    SequenceExecute(list).Forget();
                    break;
                case ExecuteMode.Loop:
                    LoopExecute(list, loopCount).Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        private static void ConcurrentExecute(FeedbackList list)
        {
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.Play();
            }
        }
        private static async UniTaskVoid SequenceExecute(FeedbackList list)
        {
            foreach (var t in list.List)
            {
                if(!t.IsActive) continue;
                t.Play();
                await t.WaitCompleted();
            }
        }
        private static async UniTaskVoid LoopExecute(FeedbackList list,int loopCount)
        {
            for (int i = 0; i < loopCount; i++)
            {
                foreach (var t in list.List)
                {
                    if(!t.IsActive) continue;
                    t.Play();
                    await t.WaitCompleted();
                }
            }
        }
    }
}