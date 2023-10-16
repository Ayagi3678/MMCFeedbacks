using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MMCFeedbacks.Core
{

    [Serializable]
    public abstract class Feedback
    {
        public virtual int Order => 0;
        public abstract string MenuString { get; }
        public string Label => StringConversionUtility.SplitLast(MenuString);
        public abstract Color TagColor { get; }
        public bool IsActive { get; set; } = true;
        
        [SerializeField] protected Timing timing;
        [SerializeField] protected bool ignoreTimeScale;

        private CancellationToken _token;
        private Func<bool> _cache;
        private bool _isCompleted;
        // ReSharper disable Unity.PerformanceAnalysis
        public void Play(CancellationToken token)
        {
            _isCompleted = true;
            _token = token;
            if (timing.delayTime != 0)
            {
                PlayAsync(token).Forget();
            }
            else
            {
                OnPlay(token);
            }
        }
        public void Stop()=>OnStop();
        public void Enable(GameObject gameObject)
        {
            _cache = () => _isCompleted;
            OnEnable(gameObject);
        }
        public void Destroy() => OnDestroy();
        public void Reset() => OnReset();

        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTaskVoid PlayAsync(CancellationToken token)
        {
            await UniTask.WaitForSeconds(timing.delayTime, ignoreTimeScale,cancellationToken : token);
            OnPlay(token);
        }
        protected virtual void OnReset(){}
        protected virtual void OnPlay(CancellationToken token){}
        protected virtual void OnStop(){}
        protected virtual void OnEnable(GameObject gameObject){}
        protected virtual void OnDestroy(){}

        protected void Complete()
        {
            _isCompleted = true;
        }
        public async UniTask<bool> WaitCompleted()
        {
            await UniTask.WaitUntil(_cache,cancellationToken:_token);
            return true;
        }
    }
}