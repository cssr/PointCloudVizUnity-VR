using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour {
    
	public GameObject character;
    public GameObject menu;

	private GameObject textToSpeechButton;
	private GameObject scribblerButton;
	private GameObject highlightPointsButton;

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

		//Load menu buttons textures
		textToSpeechTextureActive = Resources.Load("textToSpeechTexActive") as Texture;
		textToSpeechTextureInactive = Resources.Load("textToSpeechTexInactive") as Texture;

		scribblerTextureActive = Resources.Load("scribblerTexActive") as Texture;
		scribblerTextureInactive = Resources.Load("scribblerTexInactive") as Texture;

		highlightPointsTextureActive = Resources.Load("highlightPointsTexActive") as Texture;
		highlightPointsTextureInactive = Resources.Load("highlightPointsTexInactive") as Texture;
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

		if (scribbler.IsActive)
			scribbler.IsActive = false;

		if (highlightPoints.IsActive)
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

		if (scribbler.IsActive) {
			// todo change texture in 3D Menu
			scribbler.IsActive = false;
			scribblerButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", scribblerTextureInactive);
		} else {
			scribbler.IsActive = true;
			scribblerButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", scribblerTextureActive);
		}

		if (highlightPoints.IsActive) {
			// todo change texture in 3D Menu
			highlightPoints.IsActive = false;
			highlightPointsButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", highlightPointsTextureInactive);
		} else {
			highlightPoints.IsActive = true;
			highlightPointsButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", highlightPointsTextureActive);
		}
			
        Debug.Log("Left Mouse Button Single Click");
        
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
		}

	}
}
