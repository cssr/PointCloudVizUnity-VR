using UnityEngine;
using UnityEngine.VR;
using System.Collections;

/// <summary>
/// This class is responsible for updating the caracter head 
/// rotation according to the HMD (Head Mounted Display) rotation.
/// </summary>
public class HeadCameraController : MonoBehaviour
{
    public Vector3 threshold;
    //public Transform CarlHip;

    public Transform headTransform;
    public GameObject character;
    public bool thirdPerson;

    private Transform pivot1st;
    private Transform pivot1stAux;
    private Transform pivot3rd;
    private Transform pivot3rdAux;

    void Start()
    {
        TrackerClientSimpleRobot tcsr = null;
        if (character != null)
        tcsr = character.GetComponent<TrackerClientSimpleRobot>();

        GameObject go = new GameObject("pivot1stAux");
        pivot1stAux = go.transform;
        pivot1stAux.parent = tcsr == null? headTransform : tcsr.getHead();
        pivot1stAux.localPosition = Vector3.zero;
        pivot1stAux.localScale = Vector3.one;

        go = new GameObject("pivot1st");
        pivot1st = go.transform;
        pivot1st.parent = pivot1stAux;
        pivot1st.localPosition = threshold;
        pivot1st.localScale = Vector3.one;

        go = new GameObject("pivot3rdAux");
        pivot3rdAux = go.transform;
        pivot3rdAux.parent = tcsr == null ? headTransform : tcsr.getHead();
        pivot3rdAux.localPosition = new Vector3(0, 2.54f, 0); // PARAMETRIZAR
        pivot3rdAux.localScale = Vector3.one;

        go = new GameObject("pivot3rd");
        pivot3rd = go.transform;
        pivot3rd.parent = pivot3rdAux;
        pivot3rd.localPosition = new Vector3(0, 0, -1); // PARAMETRIZAR
        pivot3rd.localScale = Vector3.one;

    }

    void LateUpdate()
    {


        //CarlHip.position = new Vector3(UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.Head).x,
        // CarlHip.position = new Vector3(CarlHip.position.x,
        //   CarlHip.position.y,
        // UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.Head).z);
        pivot1st.localPosition = threshold;
        pivot3rd.localPosition = threshold;
       // pivot3rdAux.rotation = /*pivot1stAux.rotation =*/ UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye);
      //  pivot3rdAux.eulerAngles = new Vector3(0, pivot3rd.eulerAngles.y, 0);

        this.transform.position = new Vector3((-UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye).x),
                 (-UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye).y),
                 (-UnityEngine.VR.InputTracking.GetLocalPosition(UnityEngine.VR.VRNode.CenterEye).z)) +
                 (thirdPerson ? pivot3rd.position : pivot1st.position);
        if (thirdPerson)
        {
         //   transform.rotation = Quaternion.Inverse(UnityEngine.VR.InputTracking.GetLocalRotation(UnityEngine.VR.VRNode.CenterEye));
        }

    }
}