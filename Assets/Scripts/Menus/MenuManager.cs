using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour {
    
	public GameObject character;
    public GameObject menu;
    public GameObject skeletonPlayer;

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

	TrackerClientSimpleRobot trackerClientSimpleRobot;
    TrackerClientRecorded trackerClientRecorded;
    FileListener fileListener;
	RaycastHit hit;

    // Use this for initialization
    protected void Start () {

        textToSpeech = GetComponent<TextToSpeech>();
        scribbler = GetComponent<Scribbler>();
        highlightPoints = GetComponent<HighlightPoints>();
        changeColor = GetComponent<ChangeColor>();
        if (character != null)
		{
			trackerClientSimpleRobot = character.GetComponent<TrackerClientSimpleRobot>();
            _rightHand = trackerClientSimpleRobot.getRightArm();
		}
        if(skeletonPlayer != null)
        {
            trackerClientRecorded = skeletonPlayer.GetComponent<TrackerClientRecorded>();
            fileListener = skeletonPlayer.GetComponent<FileListener>();
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

            if (Input.GetMouseButtonDown(1))
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

        Debug.Log("Scribbler button isActive = " + scribblerButtonIsActive);
        Debug.Log("Scribbler isActive = " + scribbler.IsActive);
        if (scribblerButtonIsActive)
        {
            // todo change texture in 3D Menu
            LeftMouseButtonDoubleClick();
            scribbler.createRenderer();
            scribbler.IsActive = true;
            menu.SetActive(false);
            Debug.Log("Testing scribbler");
        }

        if (highlightPointsButtonIsActive)
        {
            // todo change texture in 3D Menu
            LeftMouseButtonDoubleClick();
            highlightPoints.IsActive = true;
            menu.SetActive(false);
            Debug.Log("Testing highlight");
        }

        Debug.Log("mouse button down");
        
        //LeftMouseButtonSingleClick();
    }

	private IEnumerator LeftMouseReleaseEvent(){
		
		//pause a frame so you don't pick up the same mouse down event.
		yield return new WaitForEndOfFrame();

        Debug.Log("release Scribbler button isActive = " + scribblerButtonIsActive);
        Debug.Log("release Scribbler isActive = " + scribbler.IsActive);
        if (scribblerButtonIsActive) {
			scribbler.IsActive = false;
           // scribbler.assignClosestBone(trackerClientRecorded.Humans);
			annotationManager.AddScribblerAnnotation (scribbler.lineRendererGO);
		}

		if (highlightPointsButtonIsActive)
			highlightPoints.IsActive = false;
            //annotationManager.AddHighlightPointsAnnotation();

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
        
        Debug.Log("Left Mouse Button Single Click");
    }

    
    private void LeftMouseButtonDoubleClick()
    {
        Debug.Log("Left Mouse Button Double Click");
        fileListener.playing = !fileListener.playing;

        foreach (PointCloud pc in clouds)
        {
            pc.playing = !pc.playing;
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
			Transform headPosition = trackerClientSimpleRobot.getHead ();
			Vector3 frontVector = Camera.main.transform.forward;

			menu.transform.position = headPosition.position;
			menu.transform.Translate(1.0f * frontVector.x, 1.0f * frontVector.y, 1.0f * frontVector.z);

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
            if (scribblerButton == null)
                scribblerButton = GameObject.FindGameObjectWithTag("Scribbler");

            if (highlightPointsButton == null)
                highlightPointsButton = GameObject.FindGameObjectWithTag("HighlightPoints");

            //	if(textToSpeechButton == null)
            //	textToSpeechButton = GameObject.FindGameObjectWithTag("TextToSpeech");

            // draw raycast vector to interact with the 3D Menu
            //Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;
            Vector3 forward = Camera.main.transform.forward;
			Debug.DrawRay (_rightHand.transform.position, forward, Color.green, 1, false);

			if (Physics.Raycast (_rightHand.transform.position, forward, out hit)) {
				print ("Found an object - name: " + hit.collider.name);


				// SPEECH TO TEXT
				/*if (hit.collider.name.Equals ("TextToSpeech")) {

					if (textToSpeechButtonIsActive) {
						textToSpeechButtonIsActive = false;
						//textToSpeechButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", textToSpeechTextureInactive);
					} 
					else 
					{
						textToSpeechButtonIsActive = true;
						//textToSpeechButton.GetComponent<Renderer> ().material.SetTexture("_MainTex", textToSpeechTextureActive);
					}
				}*/

				// HIGHLIGHT POINTS
				if (hit.collider.name.Equals("HighlightPoints")){
                    highlightPointsButton.GetComponent<Renderer>().material.SetTexture("_MainTex", highlightPointsTextureActive);

                    if (highlightPointsButtonIsActive) {
						highlightPointsButtonIsActive = false;
                    } 
					else 
					{
						highlightPointsButtonIsActive = true;
                    }
                }
                else
                {
                    highlightPointsButton.GetComponent<Renderer>().material.SetTexture("_MainTex", highlightPointsTextureInactive);

                }

                // SCRIBBLER
                if (hit.collider.name.Equals("Scribbler")){
                    scribblerButton.GetComponent<Renderer>().material.SetTexture("_MainTex", scribblerTextureActive);
                    if (scribblerButtonIsActive) {
						scribblerButtonIsActive = false;
                    } 
					else 
					{
						scribblerButtonIsActive = true;
                    }
                }
                else
                {
                    scribblerButton.GetComponent<Renderer>().material.SetTexture("_MainTex", scribblerTextureInactive);

                }
            }
		}
	}
}
