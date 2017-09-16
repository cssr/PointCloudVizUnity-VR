using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPointsAnnotation : MonoBehaviour {

    public int ID { get; set; }
    public List<Transform> Bones { get; set; }
	public float Duration { get; set; }

	public HighlightPointsAnnotation () {
		ID = 0;
		Bones = new List<Transform> ();
		Duration = 0.0f;
	}

	public bool isTheSameAnnotation(int id){
		return (ID == id);
	}

	public bool isTheSameDuration(int duration){
		return (Duration == duration);
	}
}
