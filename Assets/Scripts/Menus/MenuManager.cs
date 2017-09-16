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

	private GameObject changeColorButton;
	private bool changeColorButtonIsActive;

	private GameObject deleteButton;
	private bool deleteButtonIsActive;

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

	private Texture changeColorTextureActive;
	private Texture changeColorTextureInactive;

	private Texture deleteTextureActive;
	private Texture deleteTextureInactive;

    private AnnotationManager annotationManager;

	TrackerClientSimpleRobot trackerClientSimpleRobot;
    TrackerClientRecorded trackerClientRecorded;
    FileListener fileListener;
	RaycastHit hit;

    bool lmbclicked;
    float deltaLastClick;
    float deltaHoldTime;

    // Use this for initialization
    protected void Start () {
        lmbclicked = false;
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
      
		textToSpeechButtonIsActive = false;
		scribblerButtonIsActive = false;
		highlightPointsButtonIsActive = false;
		changeColorButtonIsActive = false;
		deleteButtonIsActive = false;

        //Load menu buttons textures
        textToSpeechTextureActive = Resources.Load("textToSpeechActive") as Texture;
        textToSpeechTextureInactive = Resources.Load("textToSpeech") as Texture;

        scribblerTextureActive = Resources.Load("scribblerActive") as Texture;
        scribblerTextureInactive = Resources.Load("scribbler") as Texture;

        highlightPointsTextureActive = Resources.Load("highlightPointsActive") as Texture;
        highlightPointsTextureInactive = Resources.Load("highlightPoints") as Texture;

		changeColorTextureActive = Resources.Load("changeColorActive") as Texture;
		changeColorTextureInactive = Resources.Load("changeColor") as Texture;

		deleteTextureActive = Resources.Load ("deleteActive") as Texture;
		deleteTextureInactive = Resources.Load ("delete") as Texture;


        annotationManager = new AnnotationManager ();
    }

    // Update is called once per frame
    private void handleMouseInput()
    {
       
            deltaLastClick += Time.deltaTime;
            if (lmbclicked) 
                deltaHoldTime += Time.deltaTime;
            

            if (lmbclicked && deltaHoldTime > doubleClickTimeLimit) 
                  LeftMouseButtonHoldDown();

            if (Input.GetMouseButtonDown(0))
                  LeftMouseClickEvent();

			if (Input.GetMouseButtonUp (0)) 
                  LeftMouseReleaseEvent ();
		
            if (Input.GetMouseButtonDown(1))
                  RightMouseClickEvent();

            if(Input.GetMouseButtonDown(2))
                  WheelMouseClickEvent();

    
    }

    private void LeftMouseClickEvent()
    {
        //pause a frame so you don't pick up the same mouse down event.
      //  yield return new WaitForEndOfFrame();

        lmbclicked = true;

        if (deltaLastClick <= doubleClickTimeLimit)
        {
            LeftMouseButtonDoubleClick();
        }

        deltaLastClick = 0;


        Debug.Log("Scribbler button isActive = " + scribblerButtonIsActive);
        Debug.Log("Scribbler isActive = " + scribbler.IsActive);
       
        LeftMouseButtonSingleClick();

    }


    private void LeftMouseButtonHoldDown()
    {
        if (scribblerButtonIsActive && !scribbler.IsActive)
        {
            // todo change texture in 3D Menu
            LeftMouseButtonDoubleClick();
            scribbler.createRenderer();
            scribbler.IsActive = true;
            Debug.Log("Testing scribbler");
        }

        if (highlightPointsButtonIsActive && !highlightPoints.IsActive)
        {
            // todo change texture in 3D Menu
            LeftMouseButtonDoubleClick();
            highlightPoints.IsActive = true;
        
            Debug.Log("Testing highlight");
        }

        Debug.Log("mouse button down");

    }


    private void LeftMouseReleaseEvent(){
		
		//pause a frame so you don't pick up the same mouse down event.
		//yield return new WaitForEndOfFrame();
        lmbclicked = false;

        Debug.Log("LMB RELEASE");
        if (scribblerButtonIsActive && deltaHoldTime > doubleClickTimeLimit) {
			scribbler.IsActive = false;
            scribbler.assignClosestBone(trackerClientRecorded.Humans);
			annotationManager.AddScribblerAnnotation (scribbler.lineRendererGO);
		}

		if (highlightPointsButtonIsActive && deltaHoldTime > doubleClickTimeLimit) { 
			highlightPoints.IsActive = false;
            annotationManager.AddHighlightPointsAnnotation(highlightPoints.bonesTransforms);
        }
        deltaHoldTime = 0;

    }

    private void RightMouseClickEvent()
    {
       

     
        RightMouseButtonSingleClick();
    }

    private void WheelMouseClickEvent()
    {
       
       
        WheelMouseButtonSingleClick();
    }

    private void LeftMouseButtonSingleClick()
    {
        menu.SetActive( false);
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
            menu.transform.position = Vector3.zero;
            menu.transform.rotation = Quaternion.identity;

            menu.transform.position = headPosition.position;
			menu.transform.Translate(1.0f * frontVector.x, 1.0f * frontVector.y, 1.0f * frontVector.z);
            menu.transform.rotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);
         //   menu.transform.up = Camera.main.transform.up;

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

	private void HandleMenuOptions() {
	
		if (menu.activeSelf) {
			if (scribblerButton == null)
				scribblerButton = GameObject.FindGameObjectWithTag("Scribbler");

			if (highlightPointsButton == null)
				highlightPointsButton = GameObject.FindGameObjectWithTag("HighlightPoints");

			if(textToSpeechButton == null)
				textToSpeechButton = GameObject.FindGameObjectWithTag("TextToSpeech");

			if(changeColorButton == null)
				changeColorButton = GameObject.FindGameObjectWithTag("ChangeColor");

			if (deleteButton == null)
				deleteButton = GameObject.FindGameObjectWithTag ("Delete");

			// draw raycast vector to interact with the 3D Menu
			//Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;
			Vector3 forward = Camera.main.transform.forward;
			Debug.DrawRay (_rightHand.transform.position, forward, Color.green, 1, false);

			if (Physics.Raycast (_rightHand.transform.position, forward, out hit)) {
				print ("Found an object - name: " + hit.collider.name);


				// SPEECH TO TEXT
				if (hit.collider.name.Equals ("TextToSpeech")) {
					textToSpeechButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", textToSpeechTextureActive);
					if (textToSpeechButtonIsActive) {
						textToSpeechButtonIsActive = false;
					} else {
						textToSpeechButtonIsActive = true;
					}
				} else
				{
					textToSpeechButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", textToSpeechTextureInactive);
				}

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
					scribblerButton.GetComponent<Renderer>().material.SetTexture("_MainTex",scribblerTextureActive);
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

				// CHANGE COLOR
				if (hit.collider.name.Equals ("ChangeColor")) {
					changeColorButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", changeColorTextureActive);
					if (changeColorButtonIsActive) {
						changeColorButtonIsActive = false;
					} else 
					{
						changeColorButtonIsActive = true;
					}
				} 
				else
				{
					changeColorButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", changeColorTextureInactive);
				}

				// DELETE
				if (hit.collider.name.Equals ("Delete")) {
					deleteButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", deleteTextureActive);
					if (deleteButtonIsActive) {
						deleteButtonIsActive = false;
					} else {
						deleteButtonIsActive = true;
					}
				} 
				else 
				{
					deleteButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", deleteTextureInactive);
				}
			}
		}
	}

    // Update is called once per frame
    void Update () {

        handleMouseInput();
		HandleMenuOptions ();

		//DEBUG
		if (menu.activeSelf) {
			//changeColorButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", changeColorTextureActive);
		}

	}
}
