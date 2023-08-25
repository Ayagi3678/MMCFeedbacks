using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    public interface IFeedback
    {
        public int Order => 0;
        public bool IsActive { get; set; }
        public FeedbackState State { get; }
        public string MenuString { get; }
        public string Label => StringConversionUtility.SplitLast(MenuString);
        public Color TagColor { get; }
        void Play();
        void Stop();

        void OnEnable(){}
        void OnDestroy(){}
    }

    public enum FeedbackState
    {
        Pending,
        Running,
        Completed
    }
}