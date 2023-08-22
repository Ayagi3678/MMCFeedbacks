using UnityEngine;

namespace MMCFeedbacks.etc
{
    public static class FeedbackStyling
    {
        public static Color volumeFeedbackColor => Color.Lerp(Color.cyan, Color.green, .5f);
        public static Color cameraFeedbackColor => Color.red;
    }
}