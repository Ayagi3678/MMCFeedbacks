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
        public static void ExecuteFeedbacks(FeedbackList list,CancellationToken token,ExecuteMode mode = ExecuteMode.Concurrent)
        {
            switch (mode)
            {
                case ExecuteMode.Concurrent:
                    ConcurrentExecute(list);
                    break;
                case ExecuteMode.Sequence:
                    SequenceExecute(list,token).Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
        private static void ConcurrentExecute(FeedbackList list)
        {
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.Play();
            }
        }
        private static async UniTaskVoid SequenceExecute(FeedbackList list,CancellationToken token)
        {
            foreach (var t in list.List.Where(t => t.IsActive))
            {
                t.Play();
                await UniTask.WaitUntil(() => t.State == FeedbackState.Completed,cancellationToken:token);
            }
        }


        public static string SplitLast(string str,char splitChar='/')
        {
            var splitString = str.Split(splitChar);
            return splitString[^1] ?? str;
        }
    }
}