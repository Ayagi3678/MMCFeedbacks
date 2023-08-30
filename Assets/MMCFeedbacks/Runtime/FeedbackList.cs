using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public struct FeedbackList : IEquatable<FeedbackList>
    {
        [SerializeReference] public List<Feedback> List;

        public FeedbackList(List<Feedback> list)
        {
            List = list;
        }

        public void AddFeedback(string selectString)
        {
            var types = ReflectionUtility.FindClassesImplementing<Feedback>();
            foreach (var type in types)
            {
                // ReSharper disable once HeapView.ObjectAllocation
                var instance = Activator.CreateInstance(type);
                if(instance is not Feedback feedback) continue;
                if (feedback.Label != selectString)continue;
                List.Add(feedback);
            }
        }

        public bool Equals(FeedbackList other)
        {
            return List == other.List;
        }

        public static implicit operator List<Feedback>(FeedbackList feedbackList)
        {
            return feedbackList.List;
        }
    }
}