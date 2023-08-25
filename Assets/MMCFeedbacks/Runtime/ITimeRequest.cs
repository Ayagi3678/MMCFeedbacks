using System;
using UniRx;

namespace MMCFeedbacks.Core
{
    public interface ITimeRequest
    {
        public int Priority { get; }
        public float TimeScale { get; }
        IObservable<Unit> OnDiscard { get; }
    }
}