using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToSpeechAnnotation {

	public string Text { get; set;}
	public Vector3 Position { get; set;}

	public bool isTheSamePosition(Vector3 pos){
	
		if (Position.x == pos.x && Position.y == pos.y && Position.z == pos.z)
			return true;
		else
			return false;
	}
}
