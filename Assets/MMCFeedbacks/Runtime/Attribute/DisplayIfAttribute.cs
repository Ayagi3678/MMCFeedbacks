using System;
using MMCFeedbacks.Core;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = true)]
    public class DisplayIfAttribute : PropertyAttribute
    {
        public readonly float ComparedFloat;
        public readonly int ComparedInt;
        public readonly string ComparedStr;
        public readonly TweenParameter ComparedTweenParameter;
        public readonly bool TrueThenShow;
        public readonly string VariableName;
        public readonly Type VariableType;

        public DisplayIfAttribute(string variableName, Type variableType, bool trueThenShow = true)
        {
            VariableName = variableName;
            VariableType = variableType;
            TrueThenShow = trueThenShow;
        }
        public DisplayIfAttribute(string boolVariableName, bool trueThenShow = true)
            : this(boolVariableName, typeof(bool), trueThenShow)
        {
        }

        public DisplayIfAttribute(string strVariableName, string comparedStr, bool trueThenShow = true)
            : this(strVariableName, comparedStr.GetType(),trueThenShow)
        {
            ComparedStr = comparedStr;
        }

        public DisplayIfAttribute(string intVariableName, int comparedInt, bool trueThenShow = true)
            : this(intVariableName, comparedInt.GetType(), trueThenShow)
        {
            ComparedInt = comparedInt;
        }

        public DisplayIfAttribute(string floatVariableName, float comparedFloat, bool trueThenShow = true)
            : this(floatVariableName, comparedFloat.GetType(), trueThenShow)
        {
            ComparedFloat = comparedFloat;
        }
    }
}