using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationManager {


	public List<ScribblerAnnotation> ScribblerAnnotationList { get; set;}
	public List<TextToSpeechAnnotation> TextToSpeechAnnotationList { get; set;}
	public List<HighlightPointsAnnotation> HighlightPointsAnnotationList { get; set;}

	// Use this for initialization
	public AnnotationManager() {
		ScribblerAnnotationList = new List<ScribblerAnnotation>();
		TextToSpeechAnnotationList = new List<TextToSpeechAnnotation>();
		HighlightPointsAnnotationList = new List<HighlightPointsAnnotation>();
	}


	public ScribblerAnnotation AddScribblerAnnotation(GameObject lineRendererGO, float timeOfCreation, Vector3 center) {
		ScribblerAnnotation sbAnnotation = new ScribblerAnnotation();
		sbAnnotation.ID = ScribblerAnnotationList.Count + 1;
		sbAnnotation.LineRendererGO = lineRendererGO;
		sbAnnotation.TimeOfCreation = timeOfCreation;
        sbAnnotation.center = center;
		ScribblerAnnotationList.Add (sbAnnotation);
        return sbAnnotation;
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

	public void AddTextToSpeechAnnotation(string text, Vector3 pos, float timeOfCreation){
		TextToSpeechAnnotation ttsAnnotation = new TextToSpeechAnnotation();
		ttsAnnotation.Text = text;
		ttsAnnotation.Position = pos;
		ttsAnnotation.TimeOfCreation = timeOfCreation;
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

	public HighlightPointsAnnotation AddHighlightPointsAnnotation(List<Transform> bones, float timeOfCreation){
        HighlightPointsAnnotation hpAnnotation = new HighlightPointsAnnotation();
        hpAnnotation.ID = HighlightPointsAnnotationList.Count + 1;
        hpAnnotation.Bones = bones;
		hpAnnotation.TimeOfCreation = timeOfCreation;
        HighlightPointsAnnotationList.Add (hpAnnotation);
        return hpAnnotation;
	}

	public void SetAnnotationDuration(float duration, Vector3 position){
	
		float minDist = 0.5f;
		foreach(ScribblerAnnotation sa in ScribblerAnnotationList) { 
			if (sa.IsAnnotationCloseToPosition (position, minDist)) {
				sa.Duration = duration;
				return;
			}
		}

		foreach(TextToSpeechAnnotation ta in TextToSpeechAnnotationList) { 
			if (ta.IsAnnotationCloseToPosition (position, minDist)) {
				ta.Duration = duration;
				return;
			}
		}

		foreach(HighlightPointsAnnotation ha in HighlightPointsAnnotationList) {
			if (ha.IsAnnotationCloseToPosition (position, minDist)) {
				ha.Duration = duration;
				return;
			}	
		}
	}

	public void DrawScribblerAnnotations(){
		foreach(ScribblerAnnotation sa in ScribblerAnnotationList) {
			sa.Draw ();
		}
	}

	public void DrawTextToSpeechAnnotations(){
		foreach(TextToSpeechAnnotation ta in TextToSpeechAnnotationList) { 
			ta.Draw ();
		}
	}

	public void DrawHighlightPointsAnnotations(){
		foreach(HighlightPointsAnnotation ha in HighlightPointsAnnotationList) {
			ha.Draw ();	
		}
	}

	public void Draw(){
	
		foreach(ScribblerAnnotation sa in ScribblerAnnotationList) {
			sa.Draw ();
		}
			
		foreach(HighlightPointsAnnotation ha in HighlightPointsAnnotationList) {
			ha.Draw ();	
		}
	}

	public void ResetDrawState(){
	
		foreach(ScribblerAnnotation sa in ScribblerAnnotationList) {
				sa.ResetDrawState ();
		}

		foreach(TextToSpeechAnnotation ta in TextToSpeechAnnotationList) { 
				ta.ResetDrawState ();
		}

		foreach(HighlightPointsAnnotation ha in HighlightPointsAnnotationList) {
				ha.ResetDrawState ();	
		}
	}

    public ScribblerAnnotation GetScribblerAnnotationByID(int ID)
    {
        foreach(ScribblerAnnotation sbAnnotation in ScribblerAnnotationList)
        {
            if (sbAnnotation.ID == ID)
                return sbAnnotation;
        }
        return null;
    }

    public HighlightPointsAnnotation GetHighlightPointsAnnotationByID(int ID)
    {
        foreach(HighlightPointsAnnotation hpAnnotation in HighlightPointsAnnotationList)
        {
            if (hpAnnotation.ID == ID)
                return hpAnnotation;
        }
            return null;
    }
}
