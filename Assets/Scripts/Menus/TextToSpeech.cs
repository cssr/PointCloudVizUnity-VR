using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class TextToSpeech : MonoBehaviour {


	public GameObject character;
	private Transform _rightHand;

	private Text m_Hypotheses;
	private Text m_Recognitions { get; set;}

	public string text { get; set; }
	public Vector3 position { get; set; }

	TextMesh text3D;

	private DictationRecognizer m_DictationRecognizer;

	public bool IsActive { get; set; }

	// Use this for initialization
	void Start () {
		IsActive = false;
		m_DictationRecognizer = new DictationRecognizer();

		text3D = gameObject.AddComponent<TextMesh> ();

		if (character != null) {
			TrackerClientSimpleRobot tcsr = character.GetComponent<TrackerClientSimpleRobot>();
			_rightHand = tcsr.getRightArm();
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (IsActive) {

			m_DictationRecognizer.DictationResult += (text, confidence) =>
			{
				m_Recognitions.text += text + "\n";
			};

			m_DictationRecognizer.DictationHypothesis += (text) =>
			{
				Debug.LogFormat("Dictation hypothesis: {0}", text);
				m_Hypotheses.text += text;
			};

			m_DictationRecognizer.DictationComplete += (completionCause) =>
			{
				if (completionCause != DictationCompletionCause.Complete)
					Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
			};

			m_DictationRecognizer.DictationError += (error, hresult) =>
			{
				Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
			};

			m_DictationRecognizer.Start ();

			text3D.text = m_Recognitions.text;
			text3D.transform.position = _rightHand.transform.position;

			Debug.Log ("Dictation started!");
		} 
		else 
		{
			m_DictationRecognizer.Stop ();
			Debug.Log ("Dictation stoped!");
		}
	}
}
