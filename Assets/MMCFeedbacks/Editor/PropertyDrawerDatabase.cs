using MMCFeedbacks.Core;

namespace MMCFeedbacks.Editor
{
    using UnityEditor;
    using System.Collections.Generic;

    public static class PropertyDrawerDatabase
    {
        private  static readonly Dictionary<System.Type, PropertyDrawer> Drawers;

        static PropertyDrawerDatabase()
        {
            Drawers = new Dictionary<System.Type, PropertyDrawer> {
                // クラスと対応するPropertyDrawerを登録しておく
                { typeof(FloatTweenParameter), new TweenParameterDrawer() },
                {typeof(IntTweenParameter), new TweenParameterDrawer()},
                {typeof(ColorTweenParameter), new TweenParameterDrawer()}
            };
        }

        public static PropertyDrawer GetDrawer(System.Type fieldType)
        {
            return Drawers.TryGetValue(fieldType, out var drawer) ? drawer : null;
        }
    }
}