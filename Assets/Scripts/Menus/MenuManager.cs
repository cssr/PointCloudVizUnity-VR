using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour {
    
	public GameObject character;
    public GameObject menu;

	private GameObject textToSpeechButton;
	private bool textToSpeechButtonIsActive;

	private GameObject scribblerButton;
	private bool scribblerButtonIsActive;

	private GameObject highlightPointsButton;
	private bool highlightPointsButtonIsActive;

    private Transform _rightHand;
    private float doubleClickTimeLimit = 0.25f;
    private PointCloud[] clouds = null;

	private TextToSpeech textToSpeech;
	private Scribbler scribbler;
	private HighlightPoints highlightPoints;
	private ChangeColor changeColor;

	private Texture textToSpeechTextureActive;
	private Texture textToSpeechTextureInactive;

	private Texture scribblerTextureActive;
	private Texture scribblerTextureInactive;

	private Texture highlightPointsTextureActive;
	private Texture highlightPointsTextureInactive;
   
	private AnnotationManager annotationManager;

	RaycastHit hit;

    // Use this for initialization
    protected void Start () {

        textToSpeech = GetComponent<TextToSpeech>();
        scribbler = GetComponent<Scribbler>();
        highlightPoints = GetComponent<HighlightPoints>();
        changeColor = GetComponent<ChangeColor>();

        if (character != null)
		{
			TrackerClientSimpleRobot tcsr = character.GetComponent<TrackerClientSimpleRobot>();
			_rightHand = tcsr.getRightArm();
		}

        clouds = GameObject.FindObjectsOfType<PointCloud>();
        StartCoroutine(InputListener());

		// get menu gameobject choices

	/*	textToSpeechButton = GameObject.FindGameObjectWithTag("TextToSpeech");
		scribblerButton = GameObject.FindGameObjectWithTag("Scribbler");
		highlightPointsButton = GameObject.FindGameObjectWithTag ("HighlightPoints"); */
		textToSpeechButtonIsActive = false;
		scribblerButtonIsActive = false;
		highlightPointsButtonIsActive = false;

		//Load menu buttons textures
		textToSpeechTextureActive = Resources.Load("textToSpeechTexActive") as Texture;
		textToSpeechTextureInactive = Resources.Load("textToSpeechTexInactive") as Texture;

		scribblerTextureActive = Resources.Load("scribblerTexActive") as Texture;
		scribblerTextureInactive = Resources.Load("scribblerTexInactive") as Texture;

		highlightPointsTextureActive = Resources.Load("highlightPointsTexActive") as Texture;
		highlightPointsTextureInactive = Resources.Load("highlightPointsTexInactive") as Texture;

		annotationManager = new AnnotationManager ();
    }

    // Update is called once per frame
    private IEnumerator InputListener()
    {
        while (enabled)
        { //Run as long as this is activ

            if (Input.GetMouseButtonDown(0))
                yield return LeftMouseClickEvent();

			if (Input.GetMouseButtonUp (0))
				yield return LeftMouseReleaseEvent ();

            if(Input.GetMouseButtonDown(1))
                yield return RightMouseClickEvent();

            if(Input.GetMouseButtonDown(2))
                yield return WheelMouseClickEvent();

            yield return null;
        }
    }

    private IEnumerator LeftMouseClickEvent()
    {
        //pause a frame so you don't pick up the same mouse down event.
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < doubleClickTimeLimit)
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeftMouseButtonDoubleClick();
                yield break;
            }
            count += Time.deltaTime;// increment counter by change in time between frames
            yield return null; // wait for the next frame
        }
        LeftMouseButtonSingleClick();
    }

	private IEnumerator LeftMouseReleaseEvent(){
		
		//pause a frame so you don't pick up the same mouse down event.
		yield return new WaitForEndOfFrame();

		if (scribblerButtonIsActive)
			scribbler.IsActive = false; 

		if (highlightPointsButtonIsActive)
			highlightPoints.IsActive = false;
	}

    private IEnumerator RightMouseClickEvent()
    {
        //pause a frame so you don't pick up the same mouse down event.
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < doubleClickTimeLimit)
        {
            if (Input.GetMouseButtonDown(1))
            {
                RightMouseButtonDoubleClick();
                yield break;
            }
            count += Time.deltaTime;// increment counter by change in time between frames
            yield return null; // wait for the next frame
        }
        RightMouseButtonSingleClick();
    }

    private IEnumerator WheelMouseClickEvent()
    {
        //pause a frame so you don't pick up the same mouse down event.
        yield return new WaitForEndOfFrame();

        float count = 0f;
        while (count < doubleClickTimeLimit)
        {
            if (Input.GetMouseButtonDown(2))
            {
                WheelMouseButtonDoubleClick();
                yield break;
            }
            count += Time.deltaTime;// increment counter by change in time between frames
            yield return null; // wait for the next frame
        }
        WheelMouseButtonSingleClick();
    }

    private void LeftMouseButtonSingleClick()
    {
		// TODO: check if this is the best place to do this. It doesnt work on the start because the 3DMenu gameobject is disable!
		if(scribblerButton == null)
			scribblerButton = GameObject.FindGameObjectWithTag("Scribbler");

		if(highlightPointsButton == null)
			highlightPointsButton = GameObject.FindGameObjectWithTag ("HighlightPoints");

		if(textToSpeechButton == null)
			textToSpeechButton = GameObject.FindGameObjectWithTag("TextToSpeech");

		if (scribblerButtonIsActive) {
			// todo change texture in 3D Menu
			scribbler.IsActive = true;
			Debug.Log ("Testing scribbler");
		}

		if (highlightPointsButtonIsActive) {
			// todo change texture in 3D Menu
			highlightPoints.IsActive = true;
			Debug.Log ("Testing highlight");
		}

		if (textToSpeechButtonIsActive) {
			textToSpeech.IsActive = true;
			Debug.Log ("Testing Speech to Text");
		} 
		else {
			textToSpeech.IsActive = false;
			annotationManager.AddTextToSpeechAnnotation (textToSpeech.text, textToSpeech.position);
		}
	
    }

    private void LeftMouseButtonDoubleClick()
    {
        Debug.Log("Left Mouse Button Double Click");
        foreach (PointCloud pc in clouds)
        {

            if (pc.playing)
            {
                //pause - change texture to pause
                pc.playing = false;
               
            }
            else
            {
                // play - change texture to play
                pc.playing = true;

            }
        }
			
		Debug.Log("Left Mouse Button Single Click");
    }

    private void RightMouseButtonSingleClick()
    {
        Debug.Log("Right Mouse Button Single Click");
        if (menu.activeSelf)
        {
			Debug.Log ("3D menu is " + menu.activeSelf);
            menu.SetActive(false);
        }
        else
        {
			Debug.Log ("3D menu is " + menu.activeSelf);
            menu.SetActive(true);
			menu.transform.position = character.transform.position;
			//menu.transform.Translate(10.0f, 0.0f, 0.0f);
			menu.transform.rotation = character.transform.rotation;
        }
    }

    private void RightMouseButtonDoubleClick()
    {
        Debug.Log("Right Mouse Button Double Click");
    }

    private void WheelMouseButtonSingleClick()
    {
        Debug.Log(" Wheel Mouse Button Single Click");
    }

    private void WheelMouseButtonDoubleClick()
    {
        Debug.Log(" Wheel Mouse Button Double Click");
       
    }

    // Update is called once per frame
    void Update () {
    
		if (menu.activeSelf) {
			// draw raycast vector to interact with the 3D Menu
			Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;
			Debug.DrawRay (_rightHand.transform.position, forward, Color.green, 1, false);

			if (Physics.Raycast (_rightHand.transform.position, forward, out hit)) {
				print ("Found an object - name: " + hit.collider.name);


				// SPEECH TO TEXT
				if (hit.collider.name.Equals ("TextToSpeech")) {

					if (textToSpeechButtonIsActive) {
						textToSpeechButtonIsActive = false;
						textToSpeechButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", textToSpeechTextureInactive);
					} 
					else 
					{
						textToSpeechButtonIsActive = true;
						textToSpeechButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", textToSpeechTextureActive);
					}
				}

				// HIGHLIGHT POINTS
				if (hit.collider.name.Equals("HighlightPoints")){

					if (highlightPointsButtonIsActive) {
						highlightPointsButtonIsActive = false;
						highlightPointsButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", highlightPointsTextureInactive);
					} 
					else 
					{
						highlightPointsButtonIsActive = true;
						highlightPointsButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", highlightPointsTextureActive);
					}

				}

				// SCRIBBLER
				if(hit.collider.name.Equals("Scribbler")){

					if (scribblerButtonIsActive) {
						scribblerButtonIsActive = false;
						scribblerButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", scribblerTextureInactive);
					} 
					else 
					{
						scribblerButtonIsActive = true;
						scribblerButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", scribblerTextureActive);
					}
				}	
			}
		}
	}
}
