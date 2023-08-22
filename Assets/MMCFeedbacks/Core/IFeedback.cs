using UnityEngine;

namespace MMCFeedbacks.Core
{
    public interface IFeedback
    {
        public bool IsActive { get; set; }
        public FeedbackState State { get; }
        public string MenuString { get; }
        public string Label => FeedbackPlayerUtility.SplitLast(MenuString);
        public Color TagColor { get; }
        void Play();
        void Stop();
        
        IFeedback Clone();

        void OnEnable(GameObject gameObject){}
        void OnDisable(){}
    }

    public enum FeedbackState
    {
        Pending,
        Running,
        Completed
    }
}