using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace MMCFeedbacks.Core
{
    public sealed class TimeEffect : ITimeRequest
    {
        public int Priority { get; }
        public float TimeScale { get; }
        private readonly Subject<Unit> _onDiscard = new();
        public IObservable<Unit> OnDiscard => _onDiscard;

        public TimeEffect(int priority, float timeScale, float discordTime)
        {
            Priority = priority;
            TimeScale = timeScale;
            DiscardAsync(discordTime).Forget();
        }

        private async UniTaskVoid DiscardAsync(float discordTime)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(discordTime),true);
            _onDiscard.OnNext(Unit.Default);
        }

        public void Discard()
        {
            _onDiscard.OnNext(Unit.Default);
        }
    }
}