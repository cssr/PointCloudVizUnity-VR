using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationManager {


	List<ScribblerAnnotation> ScribblerAnnotationList { get; set;}
	List<TextToSpeechAnnotation> TextToSpeechAnnotationList { get; set;}
	List<HighlightPointsAnnotation> HighlightPointsAnnotationList { get; set;}

	// Use this for initialization
	public AnnotationManager() {
		ScribblerAnnotationList = new List<ScribblerAnnotation>();
		TextToSpeechAnnotationList = new List<TextToSpeechAnnotation>();
		HighlightPointsAnnotationList = new List<HighlightPointsAnnotation>();
	}


	public void AddScribblerAnnotation(GameObject lineRendererGO) {
		ScribblerAnnotation sbAnnotation = new ScribblerAnnotation();
		sbAnnotation.ID = ScribblerAnnotationList.Count + 1;
		sbAnnotation.LineRendererGO = lineRendererGO;
		ScribblerAnnotationList.Add (sbAnnotation);
	}

	public void DeleteScribblerAnnotation(ScribblerAnnotation sbAnnotation){
		if(sbAnnotation == null){
			Debug.Log("ERROR: ScribblerAnnotation object is null");
			return;
		}

		foreach (ScribblerAnnotation annotation in ScribblerAnnotationList) {
			if (annotation.ID == sbAnnotation.ID) {
				ScribblerAnnotationList.Remove (annotation);
				Debug.Log ("ScribblerAnnotation - " + sbAnnotation.ID + " was delete from list");
			}
			else
				Debug.Log ("ERROR: Unable to delete ScribblerAnnotation - " + sbAnnotation.ID);
		}
		Debug.Log ("ERROR: Unable to find the ScribblerAnnotation - " + sbAnnotation.ID);
	}

	public void AddTextToSpeechAnnotation(string text, Vector3 pos){
		TextToSpeechAnnotation ttsAnnotation = new TextToSpeechAnnotation();
		ttsAnnotation.Text = text;
		ttsAnnotation.Position = pos;
		TextToSpeechAnnotationList.Add (ttsAnnotation);
	}

	public void DeleteTextToSpeechAnnotation(TextToSpeechAnnotation ttsAnnotation){
		if(ttsAnnotation == null) {
			Debug.Log("ERROR: TextToSpeechAnnotation object is null");
			return;
		}

		foreach (TextToSpeechAnnotation annotation in TextToSpeechAnnotationList) {
			if(annotation.Text.Equals(ttsAnnotation.Text) && annotation.isTheSamePosition(ttsAnnotation.Position)){
				if (TextToSpeechAnnotationList.Remove (ttsAnnotation))
					Debug.Log ("TextToSpeechAnnotation - " + ttsAnnotation.Text + " was delete from list");
				else
					Debug.Log ("ERROR: Unable to delete TextToSpeechAnnotation - " + ttsAnnotation.Text);

				return;
			}
		}
		Debug.Log ("ERROR: Unable to find the TextToSpeechAnnotation - " + ttsAnnotation.Text);
	}

	public void AddHighlightPointsAnnotation(List<Transform> bones){
        HighlightPointsAnnotation hpAnnotation = new HighlightPointsAnnotation();
        hpAnnotation.Bones = bones;
        HighlightPointsAnnotationList.Add (hpAnnotation);
	}
}
