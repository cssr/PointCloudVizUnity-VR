using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scribbler : MonoBehaviour {

    //private UDPHandheldListener _handheldListener;
    public GameObject lineRendererGO { get; set; }
    public LineRenderer lineRenderer { get; set;}
    private int _currentRenderer = -1;
    private Transform _rightHand;
	public bool IsActive { get; set; }
    private List<Vector3> _myPoints;
    public GameObject character;
	private GameObject pointCloudSkeleton { get; set; }
    public Vector3 center;
    private TrackerClientRecorded trackerClientRecorded;


    // Use this for initialization
    void Start () {
        //_handheldListener = new UDPHandheldListener(1998, "negativespace");
       
		IsActive = false;

        if (character != null) {
            TrackerClientSimpleRobot tcsr = character.GetComponent<TrackerClientSimpleRobot>();
            _rightHand = tcsr.getRightArm();
        }

		if (pointCloudSkeleton != null) {
			trackerClientRecorded = pointCloudSkeleton.GetComponent<TrackerClientRecorded> ();
		
			Debug.Log ("number of humans = " + trackerClientRecorded.Humans.Count);
		}

        lineRenderer = null;

        _myPoints = new List<Vector3>();
    }

    public void createRenderer(Color c)
    {
        lineRendererGO = new GameObject("lineRenderer " + _currentRenderer);
        lineRendererGO.transform.parent = gameObject.transform;
        lineRenderer = lineRendererGO.AddComponent<LineRenderer>();
		//lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.material = Resources.Load("ParticleAfterburner") as Material;
        lineRenderer.widthMultiplier = 0.05f;
        // A simple 2 color gradient with a fixed alpha of 1.0f.
       // Color c1 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        Color c2 = new Color(c.r / 2.0f, c.g / 2.0f, c.b /2.0f);
        lineRenderer.startColor =c;
        lineRenderer.endColor = c2;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 0;
        _currentRenderer++;
    }

    public void assignClosestBone(Dictionary<string,Human> humans)
    {
        float minDist = float.MaxValue;
        float maxDist = 1f;
        Transform minTransf = null;
        center = Vector3.zero;
        foreach (Vector3 v in _myPoints)
        {
            center += v;
        }
        center = new Vector3(center.x / _myPoints.Count, center.y / _myPoints.Count, center.z / _myPoints.Count);
        foreach (Human h in humans.Values)
        {
            List<Transform> transfs = h.tbr.bodyTransforms;
            foreach (Transform t in transfs)
            {
                float dist = (center - t.position).magnitude;
                if (dist < minDist && dist < maxDist)
                {
                    minDist = dist;
                    minTransf = t;
                }
            }
        }
        lineRendererGO.transform.parent = minTransf;
    }

    // Update is called once per frame
    void Update () {
        //if (_handheldListener.Message.Click)
		//if(Input.GetMouseButton(0))
        //{
            if (!IsActive)
            {
                _myPoints.Clear();
            }

		if (IsActive)
            {
            _myPoints.Add(_rightHand.position);
            if (_myPoints != null)
                {
                    lineRenderer.positionCount = _myPoints.Count;
                    for (int i = 0; i < _myPoints.Count; i++)
                    {
                    lineRenderer.SetPosition(i, _myPoints[i]);
                    }
                }
            }
      /*  }
        else
        {
            _drawing = false;
        }
        */

       
    }
}
