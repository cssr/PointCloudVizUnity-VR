using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPointsAnnotation : MonoBehaviour {

    public int ID { get; set; }
	public List<Transform> Bones { get; set; }
	public float Duration { get; set; }
	Material mat;
	public bool HasBeenDrawn { get; set; }
	public float TimeOfCreation { get; set; }
    private int _mpPos;
    private float[] _myPoints;
    public Color highlightColor { get; set; }

	public HighlightPointsAnnotation () {
		ID = 0;
		Bones = new List<Transform> ();
		Duration = 0.0f;
		mat = Resources.Load ("cloudmat") as Material;
		HasBeenDrawn = false;
        TimeOfCreation = 0.0f;

        _myPoints = new float[500];
	}

    void resetMyPoints()
    {
        for (int i = 0; i < _myPoints.Length; i++)
        {
            _myPoints[i] = 0;
        }
        _mpPos = 0;
    }

	public bool isTheSameAnnotation(int id){
		return (ID == id);
	}

	public bool isTheSameDuration(int duration){
		return (Duration == duration);
	}

	public bool IsAnnotationCloseToPosition(Vector3 position, float minDist){

		foreach (Transform t in Bones) {
			if ((t.position - position).magnitude < minDist)
				return true;
		}
		return false;
	}

	public void Draw(){

		if (!HasBeenDrawn) {
			List<float> bonePositionsf = new List<float> ();
			foreach (Transform bone in Bones) {

                _myPoints[_mpPos++] = (bone.transform.position.x);
                _myPoints[_mpPos++] = (bone.transform.position.y);
                _myPoints[_mpPos++] = (bone.transform.position.z);

                mat.SetFloatArray("_BonesPositions", _myPoints);
                mat.SetInt("_BonesPositionsLenght", _mpPos);
                mat.SetColor("_Color", highlightColor);	
			}
			HasBeenDrawn = true;
		}
	}

	public void EndDraw(){

        resetMyPoints();
        mat.SetFloatArray("_BonesPositions", _myPoints);
        mat.SetInt("_BonesPositionsLenght", _mpPos);
        mat.SetColor("_Color", highlightColor);	

	}

	public void ResetDrawState(){
		HasBeenDrawn = false;
	}
}
