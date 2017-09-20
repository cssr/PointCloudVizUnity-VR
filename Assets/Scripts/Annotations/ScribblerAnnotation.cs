using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScribblerAnnotation {

	public int ID { get; set; }
	public GameObject LineRendererGO { get; set; }
    public Vector3 center;
	public float Duration { get; set; }
	public bool HasBeenDrawn{ get; set; }
	public float TimeOfCreation { get; set; }
    public Color scribblerColor { get; set; }

    public ScribblerAnnotation() {
        ID = 0;
		LineRendererGO = null;
		Duration = 0.0f;
		HasBeenDrawn = false;
        TimeOfCreation = 0.0f;
    }

	public bool isTheSameAnnotation(int id){
		return (ID == id);
	}

	public bool isTheSameDuration(float duration){
		return (Duration == duration);
	}

	public Vector3 GetPosition(){
		return LineRendererGO.transform.localPosition; //not sure if is the local position or position
	}

	public bool IsAnnotationCloseToPosition(Vector3 position, float minDist){
		if ((center - position).magnitude < minDist)
			return true;
		
		return false;
	}

	public void Draw(){
	
		if (!HasBeenDrawn) {
			LineRendererGO.SetActive (true);
			HasBeenDrawn = true;
		}
	}

	public void EndDraw(){
		LineRendererGO.SetActive (false);
	}

	public void ResetDrawState(){
		HasBeenDrawn = false;
	}

}
