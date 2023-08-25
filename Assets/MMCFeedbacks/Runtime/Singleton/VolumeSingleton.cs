using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MMCFeedbacks.Core
{
    public class VolumeSingleton : SingletonBehaviour<VolumeSingleton>
    {
        public Volume volume;
        protected override void OnInitialize()
        {
            if (volume==null)
            {
                volume = gameObject.AddComponent<Volume>();
            }
        }
        public T TryGetVolumeComponent<T>() where T : VolumeComponent
        {
            if (volume.profile.TryGet(out T t)) return t;
            var component = volume.profile.Add(typeof(T));
            return component as T;
        }
        public void EnableVolumeComponent(VolumeComponent volumeComponent)
        {
            volumeComponent.active = true;
            foreach (var t in volumeComponent.parameters) t.overrideState = true;
        }
        public void DisableVolumeComponent(VolumeComponent volumeComponent)
        {
            volumeComponent.active = false;
        }


    }
}