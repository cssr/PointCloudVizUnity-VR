using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextToSpeechAnnotation {

	public int ID { get; set; }
	public string Text { get; set; }
	public Vector3 Position { get; set; }
	float Duration { get; set; }

	public TextToSpeechAnnotation () {
		Text = "";
		Position = Vector3.zero;
		Duration = 0.0f;
	}

	public bool isTheSameAnnotation(int id){
		return (ID == id);
	}

	public bool isTheSamePosition(Vector3 pos){
		return (Position.x == pos.x && Position.y == pos.y && Position.z == pos.z);
	}

	public bool isTheSameDuration(float duration){
		return (Duration == duration);
	}

	public bool isTheSameText(string text){
		return (Text.Equals (text));
	}
}
