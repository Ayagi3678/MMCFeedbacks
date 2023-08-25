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
        [SerializeReference] public List<IFeedback> List;

        public FeedbackList(List<IFeedback> list)
        {
            List = list;
        }

        public void AddFeedback(string selectString)
        {
            var types = ReflectionUtility.FindClassesImplementing<IFeedback>();
            foreach (var instance in types.Select(Activator.CreateInstance))
            {
                if(instance is not IFeedback feedback) continue;
                if (feedback.Label != selectString)continue;
                List.Add(feedback);
            }
        }

        public bool Equals(FeedbackList other)
        {
            return List == other.List;
        }

        public static implicit operator List<IFeedback>(FeedbackList feedbackList)
        {
            return feedbackList.List;
        }
    }
}