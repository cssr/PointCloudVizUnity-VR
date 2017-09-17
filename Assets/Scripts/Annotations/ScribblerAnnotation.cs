﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScribblerAnnotation {

	public int ID { get; set; }
	public GameObject LineRendererGO { get; set; }
	public float Duration { get; set; }

    public ScribblerAnnotation() {
        ID = 0;
		LineRendererGO = null;
		Duration = 0.0f;
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
		if ((LineRendererGO.transform.position - position).magnitude < minDist)
			return true;
		
		return false;
	}

}
