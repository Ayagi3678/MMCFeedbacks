using UnityEngine;

namespace MMCFeedbacks.Core
{
    public static class FeedbackStyling
    {
        public static Color VolumeFXColor => Color.Lerp(Color.cyan, Color.green, .5f);
        public static Color GraphicFXColor => Color.magenta;
        public static Color UIFXColor => Color.Lerp(Color.yellow, Color.green, .5f);
        public static Color AudioFXColor => Color.yellow;
        public static Color CameraFXColor => Color.red;
        public static Color TransformFXColor => Color.cyan;
        public static Color RectTransformFXColor => Color.Lerp(Color.cyan, Color.blue, .5f);
        public static Color EtcFXColor => Color.white;
    }
}