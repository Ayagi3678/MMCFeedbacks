using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            var types = CoreUtility.FindClassesImplementing<IFeedback>();
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