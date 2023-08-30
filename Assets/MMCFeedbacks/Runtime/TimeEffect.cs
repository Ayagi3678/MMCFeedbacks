using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace MMCFeedbacks.Core
{
    public sealed class TimeEffect : ITimeRequest
    {
        public int Priority { get; }
        public float TimeScale { get; }
        public event Action OnDiscard;

        public TimeEffect(int priority, float timeScale, float discordTime)
        {
            Priority = priority;
            TimeScale = timeScale;
            DiscardAsync(discordTime).Forget();
        }

        private async UniTaskVoid DiscardAsync(float discordTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(discordTime),true);
            OnDiscard?.Invoke();
        }

        public void Discard()
        {
            OnDiscard?.Invoke();
        }
    }
}