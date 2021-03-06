﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextToSpeechAnnotation {

	public int ID { get; set; }
	public string Text { get; set; }
	public Vector3 Position { get; set; }
	public float Duration { get; set; }
	public bool HasBeenDrawn { get; set; }
	public float TimeOfCreation { get; set; }

	public TextToSpeechAnnotation () {
		Text = "";
		Position = Vector3.zero;
		Duration = 0.0f;
		HasBeenDrawn = false;
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

	public bool IsAnnotationCloseToPosition(Vector3 pos, float minDist) {
		if ((Position - pos).magnitude < minDist)
			return true;

		return false;
	}

	public void Draw(){

		if (!HasBeenDrawn) {
	
			HasBeenDrawn = true;
		}
	}

	public void EndDraw(){
		//TODO
	}

	public void ResetDrawState(){
		HasBeenDrawn = false;
	}

}
