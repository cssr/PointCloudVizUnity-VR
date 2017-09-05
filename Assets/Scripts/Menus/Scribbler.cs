using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scribbler : MonoBehaviour {

    //private UDPHandheldListener _handheldListener;
    private List<LineRenderer> _lineRenderers;
    private int _currentRenderer = -1;
    private Transform _rightHand;
	public bool IsActive { get; set; }
    private List<Vector3> _myPoints;
    public GameObject character;


    // Use this for initialization
    void Start () {
        //_handheldListener = new UDPHandheldListener(1998, "negativespace");
       
		IsActive = false;

        if (character != null) {
            TrackerClientSimpleRobot tcsr = character.GetComponent<TrackerClientSimpleRobot>();
            _rightHand = tcsr.getRightArm();
        }
        _lineRenderers = new List<LineRenderer>();
        _myPoints = new List<Vector3>();
    }

    void createRenderer()
    {
        GameObject go = new GameObject("lineRenderer " + _currentRenderer);
        go.transform.parent = gameObject.transform;
        LineRenderer lineRenderer = go.AddComponent<LineRenderer>();
		//lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
		lineRenderer.material = Resources.Load("ParticleGlow") as Material;
        lineRenderer.widthMultiplier = 0.05f;
        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Color c1 = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        Color c2 = new Color(c1.r / 2.0f, c1.g / 2.0f, c1.b /2.0f);
        lineRenderer.startColor =c1;
        lineRenderer.endColor = c2;
        lineRenderer.positionCount = 0;
        _lineRenderers.Add(lineRenderer);
        _currentRenderer++;
    }
    // Update is called once per frame
    void Update () {
        //if (_handheldListener.Message.Click)
		//if(Input.GetMouseButton(0))
        //{
            if (!IsActive)
            {
                _myPoints.Clear();
				IsActive = true;
                createRenderer();
            }
            _myPoints.Add(_rightHand.position);

		if (IsActive)
            {
                if (_myPoints != null)
                {
                    _lineRenderers[_currentRenderer].positionCount = _myPoints.Count;
                    for (int i = 0; i < _myPoints.Count; i++)
                    {
                        _lineRenderers[_currentRenderer].SetPosition(i, _myPoints[i]);
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
