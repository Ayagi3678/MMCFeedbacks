using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace MMCFeedbacks.Core
{
    [Serializable]
    public class ObjectInstantiate : IFeedback
    {
        public bool IsActive { get; set; } = true;
        public FeedbackState State { get; private set; }
        public string MenuString => "Object/Instantiate";
        public Color TagColor => FeedbackStyling.ObjectFXColor;
        
        [SerializeField] private Timing timing;
        [Space(10)] 
        [SerializeField] private GameObject prefab;
        [SerializeField] private SimulationSpace space;
        [SerializeField, DisplayIf(nameof(space), 2)] private Transform target;
        [Header("Position")]
        [SerializeField] private Vector3 targetPosition;
        [Header("Rotation")]
        [SerializeField]private Vector3 targetRotation;

        private GameObject _gameObject;
        private CancellationTokenSource _cancellationTokenSource;
        public void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void OnEnable(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void Play()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new();
            State = FeedbackState.Pending;
            PlayAsync().Forget();
        }

        public void Stop(){}

        private async UniTaskVoid PlayAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timing.delayTime),
                cancellationToken: _cancellationTokenSource.Token);
            State = FeedbackState.Completed;
            switch (space)
            {
                case SimulationSpace.World:
                    UnityEngine.Object.Instantiate(prefab,targetPosition,Quaternion.Euler(targetRotation));
                    break;
                case SimulationSpace.Local:
                    UnityEngine.Object.Instantiate(prefab,targetPosition,Quaternion.Euler(targetRotation),_gameObject.transform);
                    break;
                case SimulationSpace.CustomTarget:
                    UnityEngine.Object.Instantiate(prefab,targetPosition,Quaternion.Euler(targetRotation),target);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}