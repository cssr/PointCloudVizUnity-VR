using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPoints : MonoBehaviour {



    public List<Transform> bonesTransforms;
	//public List<Transform> pointsToHighlight; 
	public bool IsActive { get; set; }
	Material mat;
	private Transform _rightHand;
	public GameObject character;
    public GameObject pointCloudSkeleton { get; set; }
    private List<Vector3> _myPoints;

    Transform Bone { get; set; }
    private TrackerClientRecorded trackerClientRecorded;

    // Use this for initialization
    void Start () {
		IsActive = false;
		mat = Resources.Load ("cloudmat") as Material;

		if (character != null) {
			TrackerClientSimpleRobot tcsr = character.GetComponent<TrackerClientSimpleRobot>();
			_rightHand = tcsr.getRightArm();
		}

        if (pointCloudSkeleton != null)
        {
            trackerClientRecorded = pointCloudSkeleton.GetComponent<TrackerClientRecorded>();

            Debug.Log("number of humans = " + trackerClientRecorded.Humans.Count);
        }

        bonesTransforms = new List<Transform>();
        _myPoints = new List<Vector3>();
	}

    Transform FindNearestBone(Transform handPosition)
    {
        float minDist = float.MaxValue;
        float maxDist = 1f;
        Transform minTransf = null;
        Dictionary<string, Human> humans = trackerClientRecorded.Humans;
        foreach (Human h in humans.Values)
        {
            List<Transform> transfs = h.tbr.bodyTransforms;
            foreach (Transform t in transfs)
            {
                float dist = (handPosition.position - t.position).magnitude;
                if (dist < minDist && dist < maxDist)
                {
                    minDist = dist;
                    minTransf = t;
                }
            }
        }
        return minTransf;
    }

    // Update is called once per frame
    void Update () {

		if (!IsActive)
		{
            bonesTransforms.Clear();
			//yPoints.Clear();
		}
		
		if (IsActive) {

            /* _myPoints.Add(_rightHand.position);
             List<float> positions = new List<float> ();
             foreach (Vector3 t in _myPoints) {
                 positions.Add (t.x);
                 positions.Add (t.y);
                 positions.Add (t.z);
             }*/

            Bone = FindNearestBone(_rightHand);
            List<float> bonePosition = new List<float>();
            bonePosition.Add(Bone.transform.position.x);
            bonePosition.Add(Bone.transform.position.y);
            bonePosition.Add(Bone.transform.position.z);

            if (bonePosition.Count != 0) {
				mat.SetFloatArray ("_BonesPositions", bonePosition);
				mat.SetInt ("_BonesPositionsLenght", bonePosition.Count);
			}

            bonesTransforms.Add(Bone);
		}
	}
}
