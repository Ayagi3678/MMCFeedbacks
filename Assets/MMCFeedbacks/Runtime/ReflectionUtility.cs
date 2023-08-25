using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MMCFeedbacks.Core
{
    public static class ReflectionUtility
    {
        public static List<Type> FindClassesImplementing<T>()
        {
            var implementingTypes = new List<Type>();

            // アセンブリ内のすべての型を取得
            var types = Assembly.GetExecutingAssembly().GetTypes();

            // 各型に対してインターフェースの継承をチェック
            foreach (var type in types)
                if (typeof(T).IsAssignableFrom(type) && type.IsClass)
                    implementingTypes.Add(type);

            return implementingTypes;
        }

        public static IFeedback CopyFeedback(this IFeedback feedback)
        {
            Type type = feedback.GetType();
            var constructorInfo = type.GetConstructor(Type.EmptyTypes);
            IFeedback clone = (IFeedback)constructorInfo.Invoke(Type.EmptyTypes);
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .SelectMany(x => x.CustomAttributes
                    .Where(t => t.AttributeType == typeof(SerializeField))
                    .Select(_ => x));;
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsClass)
                {
                    field.SetValue(clone, field.GetValue(feedback));    
                }
                else
                {
                    var constructor = field.FieldType.GetConstructor(new [] { field.FieldType });
                    if (constructor != null)
                    {
                        var a = constructor.Invoke(new []{field.GetValue(feedback)});
                        field.SetValue(clone,a);
                        continue;
                    }
                    field.SetValue(clone, field.GetValue(feedback));
                }
            }
            return clone;
        }
    }
}