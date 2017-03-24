using UnityEngine;

namespace MyTechnic
{
    public static class Constants
    {
        // Alpha
        public static float FADE_ALPHA = 0.5f;
        public static float OPAQUE_ALPHA = 1.0f;

        // Object outline width
        public static float OBJ_MIN_OUTLINE_WIDTH = 0.0f;
        public static float OBJ_MAX_OUTLINE_WIDTH = 0.3f;

        // Object outline alpha
        public static float OBJ_MIN_OUTLINE_ALPHA = 0.0f;
        public static float OBJ_MAX_OUTLINE_ALPHA = 1.0f;

        //Object outline Color
        public static Color32 OBJ_OUTLINE_COLOR = new Color32(255, 255, 0, 255);

        // Widget outline width
        public static float WID_MIN_OUTLINE_WIDTH = 0.0f;
        public static float WID_MAX_OUTLINE_WIDTH = 0.001f;

        // Widget outline alpha
        public static float WID_MIN_OUTLINE_ALPHA = 0.0f;
        public static float WID_MAX_OUTLINE_ALPHA = 1.0f;

        // Open hand box collider
        public static Vector3 OPEN_HAND_SIZE = new Vector3(0.18f, 0.06f, 0.11f);
        public static Vector3 OPEN_HAND_CENTER = new Vector3(0.12f, -0.006f, 0.005f);

        public static Vector3 CLOSED_HAND_SIZE = new Vector3(0.075f, 0.095f, 0.11f);
        public static Vector3 CLOSED_HAND_CENTER = new Vector3(0.08f, -0.02f, 0.005f);

        //Axis Snapping Offset
        public static float AXIS_SNAP_OFFSET = 12.0f;

        //Angle Offset to calculate the Rotation Vector
        public static float ROTATION_VECTOR_ANGLE_OFFSET = 8.0f;

        //Unlock Angles Offset
        public static float SIXDOF_UNLOCK_ANGLE_OFFSET = 30.0f;
        public static float THREEDOF_UNLOCK_ANGLE_OFFSET = 30.0f;

        //Scaling Properties
        public static float SCALING_CONSTANT = 0.3f;
        public static Color32 SCALING_OUTLINE_COLOR = new Color32(255, 0, 255, 255);

        //Translation Widget Properties
        public static float DRAW_MINIMUM_DISTANCE = 0.001f;
        public static float ERASE_SPEED = 30.0f;
        public static float TRANSLATION_WIDGET_LENGTH = 20.0f;
        public static Color32 SNAP_COLOR = new Color32(125, 125, 0, 255);
        public static Color32 NO_SNAP_COLOR = new Color32(0, 125, 0, 255);
    }
}