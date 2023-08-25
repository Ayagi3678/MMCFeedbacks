using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

namespace MMCFeedbacks.Core
{
    public static class FeedbackPlayerUtility
    {
        public static void ExecuteFeedbacks(FeedbackList list,int loopCount,CancellationToken token,Action action,ExecuteMode mode = ExecuteMode.Concurrent)
        {
            switch (mode)
            {
                case ExecuteMode.Concurrent:
                    ConcurrentExecute(list,action).Forget();
                    break;
                case ExecuteMode.Sequence:
                    SequenceExecute(list,token,action).Forget();
                    break;
                case ExecuteMode.Loop:
                    LoopExecute(list, loopCount, token,action).Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        private static async UniTaskVoid ConcurrentExecute(FeedbackList list,Action action)
        {
            var tasks = new List<UniTask>();
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.Play();
                tasks.Add(UniTask.WaitUntil(() => t.State == FeedbackState.Completed));
            }

            await UniTask.WhenAll(tasks);
            action.Invoke();
        }
        private static async UniTaskVoid SequenceExecute(FeedbackList list,CancellationToken token,Action action)
        {
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.Play();
                await UniTask.WaitUntil(() => t.State == FeedbackState.Completed,cancellationToken:token);
            }
            action.Invoke();
        }
        private static async UniTaskVoid LoopExecute(FeedbackList list,int loopCount,CancellationToken token,Action action)
        {
            for (int i = 0; i < loopCount; i++)
            {
                foreach (var t in list.List.Where(t => t.IsActive))
                {
                    t.Play();
                    await UniTask.WaitUntil(() => t.State == FeedbackState.Completed,cancellationToken:token);
                }
            }
            action.Invoke();
        }
    }
}