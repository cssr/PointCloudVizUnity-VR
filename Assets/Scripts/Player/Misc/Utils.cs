using UnityEngine;

namespace MyTechnic
{
    public static class Utils
    {
        /// <summary>
        /// Edits game object's color and mantains alpha.
        /// </summary>
        public static void EditColor(GameObject obj, Color cValue)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            float alpha = renderer.material.color.a;
            renderer.material.color = new Color(cValue.r, cValue.g, cValue.b, alpha);
        }

        /// <summary>
        /// Edits game object's outline width and alpha
        /// </summary>
        public static void EditOutline(GameObject obj, float oValue, float aValue)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            renderer.material.SetFloat("_Outline", oValue);

            Color outlineColor = renderer.material.GetColor("_OutlineColor");
            renderer.material.SetColor("_OutlineColor", new Color(outlineColor.r, outlineColor.g, outlineColor.b, aValue));
        }


        /// <summary>
        /// Edits game object's outline color
        /// </summary>
        public static void EditOutlineColor(GameObject obj, Color cValue)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            float alpha = renderer.material.color.a;
            renderer.material.SetColor("_OutlineColor", new Color(cValue.r, cValue.g, cValue.b, alpha));
        }

        /// <summary>
        /// Edits game object's opacity.
        /// </summary>
        public static void EditOpacity(GameObject obj, float aValue)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            Color color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, aValue);
        }

        // Human body functions
        public static Vector3 GetBoneDirection(Vector3 joint, Vector3 parentJoint)
        {
            return (joint - parentJoint).normalized;
        }

        public static Quaternion GetQuaternionFromRightUp(Vector3 right, Vector3 up)
        {
            Vector3 forward = Vector3.Cross(right, up);
            return Quaternion.LookRotation(forward, Vector3.Cross(forward, right));
        }

        public static Quaternion GetQuaternionFromUpRight(Vector3 up, Vector3 right)
        {
            Vector3 forward = Vector3.Cross(right, up);
            return Quaternion.LookRotation(forward, up);
        }

        public static Quaternion GetQuaternionFromForwardUp(Vector3 forward, Vector3 up)
        {
            Vector3 right = Vector3.Cross(up, forward);
            return Quaternion.LookRotation(forward, Vector3.Cross(forward, right));
        }

        public static Quaternion GetQuaternionFromRightForward(Vector3 right, Vector3 forward)
        {
            Vector3 up = Vector3.Cross(forward, right);
            return Quaternion.LookRotation(Vector3.Cross(right, up), up);
        }

        public static void PlaySound(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.Stop();
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        /// <summary>
        /// Verify the similarity between a vector and the scene Axes (World and Object Axes)
        /// If the angle between the vector and any Axis is bellow a threshold,
        /// applies snap to the closest Axis.
        /// </summary>
        /// <param name="vectorToBeSnapped">The vector to be snapped.</param>
        /// <param name="objectAxes">The object Transform that contains the Object Axes</param>
        /// <returns>True if the vector has been snapped, false otherwise</returns>
        public static bool ApplySnapToAxes(ref Vector3 vectorToBeSnapped, Transform objectAxes)
        {
            float minorAngle = float.PositiveInfinity;
            Vector3 snappedVector = vectorToBeSnapped;
            float angle;

            //WORLD
            //try snapping with WORLD POSITIVE X axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, Vector3.right)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = Vector3.right;
            }

            //try snapping with WORLD NEGATIVE X axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, Vector3.left)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = Vector3.left;
            }

            //try snapping with WORLD POSITIVE Y axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, Vector3.up)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = Vector3.up;
            }

            //try snapping with WORLD NEGATIVE Y axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, Vector3.down)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = Vector3.down;
            }

            //try snapping with WORLD POSITIVE Z axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, Vector3.forward)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = Vector3.forward;
            }

            //try snapping with WORLD NEGATIVE Z axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, Vector3.back)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = Vector3.back;
            }

            //OBJECT
            //try snapping with OBJECT POSITIVE X axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, objectAxes.right)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = objectAxes.right;
            }

            //try snapping with OBJECT NEGATIVE X axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, -objectAxes.right)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = -objectAxes.right;
            }

            //try snapping with OBJECT POSITIVE Y axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, objectAxes.up)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = objectAxes.up;
            }

            //try snapping with OBJECT NEGATIVE Y axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, -objectAxes.up)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = -objectAxes.up;
            }

            //try snapping with OBJECT POSITIVE Z axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, objectAxes.forward)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = objectAxes.forward;
            }

            //try snapping with OBJECT NEGATIVE Z axis
            if ((angle = Vector3.Angle(vectorToBeSnapped, -objectAxes.forward)) < Constants.AXIS_SNAP_OFFSET && angle < minorAngle)
            {
                minorAngle = angle;
                snappedVector = -objectAxes.forward;
            }

            if (snappedVector != vectorToBeSnapped)
            {
                vectorToBeSnapped = snappedVector.normalized;
                return true;
            }
            return false;
        }
    }
}
