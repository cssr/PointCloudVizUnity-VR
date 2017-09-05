using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPoints : MonoBehaviour {


	public List<Transform> pointsToHighlight; 
	public bool IsActive { get; set; }
	Material mat;
	private Transform _rightHand;
	public GameObject character;
	private List<Vector3> _myPoints;

	// Use this for initialization
	void Start () {
		IsActive = false;
		mat = Resources.Load ("cloudmat") as Material;

		if (character != null) {
			TrackerClientSimpleRobot tcsr = character.GetComponent<TrackerClientSimpleRobot>();
			_rightHand = tcsr.getRightArm();
		}
		_myPoints = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!IsActive)
		{
			_myPoints.Clear();
			IsActive = true;
		}
		_myPoints.Add(_rightHand.position);

		if (IsActive) {
			List<float> positions = new List<float> ();
			foreach (Vector3 t in _myPoints) {
				positions.Add (t.x);
				positions.Add (t.y);
				positions.Add (t.z);
			}
			if (positions.Count != 0) {
				mat.SetFloatArray ("_BonesPositions", positions);
				mat.SetInt ("_BonesPositionsLenght", positions.Count);
			}
		}
	}
}
