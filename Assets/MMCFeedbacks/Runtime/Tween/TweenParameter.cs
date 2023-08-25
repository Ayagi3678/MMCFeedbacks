using UnityEngine;

namespace MMCFeedbacks.Core
{
    public abstract class TweenParameter
    {
        [HideInInspector] public bool IsActive = true;
        [HideInInspector] public bool ShowActiveBox;
    }
}