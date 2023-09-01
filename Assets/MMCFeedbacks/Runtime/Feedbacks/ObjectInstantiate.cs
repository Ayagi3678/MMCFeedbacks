using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = System.Object;

namespace MMCFeedbacks.Core
{
    [Serializable] public class ObjectInstantiate : Feedback
    {
        public override string MenuString => "Object/Instantiate";
        public override Color TagColor => FeedbackStyling.ObjectFXColor;
        [Space(10)] 
        [SerializeField] private GameObject prefab;
        [SerializeField] private SimulationSpace space;
        [SerializeField, DisplayIf(nameof(space), 2)] private Transform target;
        [Header("Position")]
        [SerializeField] private Vector3 targetPosition;
        [Header("Rotation")]
        [SerializeField]private Vector3 targetRotation;

        private GameObject _gameObject;
        protected override void OnPlay(CancellationToken token)
        {
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
            Complete();
        }
        public enum SimulationSpace
        {
            World,
            Local,
            CustomTarget
        }
    }
}