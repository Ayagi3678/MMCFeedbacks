using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    public class TimeSingleton : SingletonBehaviour<TimeSingleton>
    {
        private readonly ReactiveCollection<ITimeRequest> _timeRequests = new(new List<ITimeRequest>());
        private ITimeRequest _currentTimeRequest;

        protected override void OnInitialize()
        {
            _timeRequests.ObserveCountChanged()
                .Subscribe(count =>
                {
                    if (count == 0) Time.timeScale = 1;
                    var activeRequest = _timeRequests
                        .OrderByDescending(item => item.Priority)
                        .FirstOrDefault();
                    if (activeRequest != null) _currentTimeRequest = activeRequest;
                }).AddTo(this);

            Observable.EveryUpdate()
                .Where(_ => _timeRequests.Count != 0 && _currentTimeRequest != null)
                .Subscribe(_ => Time.timeScale = _currentTimeRequest.TimeScale)
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _timeRequests.Clear();
            Time.timeScale = 1;
        }

        public void SetTimeRequest(ITimeRequest timeEffect)
        {
            _timeRequests.Add(timeEffect);
            timeEffect.OnDiscard.Subscribe(_ => _timeRequests.Remove(timeEffect)).AddTo(this);
        }
    }
}