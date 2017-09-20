using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPoints : MonoBehaviour {

    public int ID { get; set; }
    public List<Transform> bonesTransforms;
	//public List<Transform> pointsToHighlight; 
	public bool IsActive { get; set; }
	Material mat;
	private Transform _rightHand;
	public GameObject character;
    public GameObject pointCloudSkeleton;
    private float[] _myPoints;
    private int _mpPos;
    public Color highlightColor;
    Transform Bone { get; set; }
    private TrackerClientRecorded trackerClientRecorded;

    void resetMyPoints()
    {
        for(int i = 0; i < _myPoints.Length; i++)
        {
            _myPoints[i] = 0;
        }
        _mpPos = 0;
    }

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
        _myPoints = new float[500];
        resetMyPoints();
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
    public void init(Color c)
    {
        highlightColor = c;
    }
    // Update is called once per frame
    void Update () {

		if (!IsActive)
		{
            bonesTransforms.Clear();
            resetMyPoints();
		}
		
		if (IsActive) {


            Bone = FindNearestBone(_rightHand);
            if (Bone != null && !bonesTransforms.Contains(Bone))
            {

                Debug.Log(Bone.name);
                _myPoints[_mpPos++] = (Bone.transform.position.x);
                _myPoints[_mpPos++] = (Bone.transform.position.y);
                _myPoints[_mpPos++] = (Bone.transform.position.z);

                mat.SetFloatArray("_BonesPositions", _myPoints);
                mat.SetInt("_BonesPositionsLenght", _mpPos);
                mat.SetColor("_Color", highlightColor);
              
                bonesTransforms.Add(Bone);
            }
            //else
            //    Debug.Log("No Bone was found!");
        }
    }
}
