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
        
        [SerializeField] protected Timing _timing;
        [SerializeField] protected bool _ignoreTimeScale;
        
        private UniTaskCompletionSource _completionSource = new();
        protected CancellationTokenSource CancellationTokenSource;
        public void Play()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = new();
            _completionSource = new();
            OnReset();
            PlayAsync().Forget();
        }
        public void Stop()=>OnStop();
        public void Enable(GameObject gameObject) => OnEnable(gameObject);
        public void Destroy()
        {
            CancellationTokenSource?.Cancel();
            OnDestroy();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTaskVoid PlayAsync()
        {
            await UniTask.WaitForSeconds(_timing.delayTime, _ignoreTimeScale,
                cancellationToken:CancellationTokenSource.Token);
            OnPlay();
        }
        protected virtual void OnReset(){}
        protected virtual void OnPlay(){}
        protected virtual void OnStop(){}
        public virtual void OnEnable(GameObject gameObject){}
        public virtual void OnDestroy(){}

        protected void Complete()
        {
            _completionSource.TrySetResult();
        }
        public async UniTask<bool> WaitCompleted()
        {
            await _completionSource.Task;
            return true;
        }
    }
}