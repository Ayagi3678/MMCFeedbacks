using System;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class NormalizedAnimationCurveAttribute : PropertyAttribute
    {
        public NormalizedAnimationCurveAttribute(bool normalizeValue = true, bool normalizeTime = true)
        {
            NormalizeValue = normalizeValue;
            NormalizeTime = normalizeTime;
        }

        public bool NormalizeValue { get; }
        public bool NormalizeTime { get; }
    }
}