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

	public HighlightPointsAnnotation () {
		ID = 0;
		Bones = new List<Transform> ();
		Duration = 0.0f;
		mat = Resources.Load ("cloudmat") as Material;
		HasBeenDrawn = false;
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
			foreach (Transform t in Bones) {
				Vector3 position = t.position;
				bonePositionsf.Add (position.x);
				bonePositionsf.Add (position.y);
				bonePositionsf.Add (position.z);
			}

			if (bonePositionsf.Count != 0) {
				mat.SetFloatArray ("_BonesPositions", bonePositionsf);
				mat.SetInt ("_BonesPositionsLenght", bonePositionsf.Count);
			}
			HasBeenDrawn = true;
		}
	}

	public void EndDraw(){

		//?
	}

	public void ResetDrawState(){
		HasBeenDrawn = false;
	}
}
