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
	//private ChangeColor changeColor;
    private GameObject changeColorGO;

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

    private Color color;

	TrackerClientSimpleRobot trackerClientSimpleRobot;
    TrackerClientRecorded trackerClientRecorded;
    FileListener fileListener;
	RaycastHit hit;

    bool lmbclicked;
    float deltaLastClick;
    float deltaHoldTime;

	//time the annotation will be active during playback
	GameObject durationGO;
	float duration;
	bool isWheelActive;
	enum TypeOfAnnotation {SCRIBBLER, HIGHLIGHTPOINTS, TEXTTOSPEECH};

    float currentExecutionTime;
    int currentCloud;

    // Use this for initialization
    protected void Start () {
        lmbclicked = false;
        textToSpeech = GetComponent<TextToSpeech>();
        scribbler = GetComponent<Scribbler>();
        highlightPoints = GetComponent<HighlightPoints>();
        //changeColor = GetComponent<ChangeColor>();
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
        textToSpeechTextureActive = Resources.Load("speechToTextActive") as Texture;
        textToSpeechTextureInactive = Resources.Load("speechToText") as Texture;

        scribblerTextureActive = Resources.Load("scribblerActive") as Texture;
        scribblerTextureInactive = Resources.Load("scribbler") as Texture;

        highlightPointsTextureActive = Resources.Load("highlightPointsActive") as Texture;
        highlightPointsTextureInactive = Resources.Load("highlightPoints") as Texture;

		changeColorTextureActive = Resources.Load("changeColorActive") as Texture;
		changeColorTextureInactive = Resources.Load("changeColor") as Texture;

		deleteTextureActive = Resources.Load ("deleteActive") as Texture;
		deleteTextureInactive = Resources.Load ("delete") as Texture;

        changeColorGO = GameObject.FindGameObjectWithTag("ColorPicker");
        changeColorGO.SetActive(false);

		duration = 0;

		durationGO = GameObject.FindGameObjectWithTag ("Duration");
		durationGO.SetActive (false);
		isWheelActive = false;

        currentExecutionTime = 0.0f;
        currentCloud = 0;
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

		if (Input.GetMouseButtonDown (2))
			WheelMouseClickEvent ();
		
		if (isWheelActive) {
			durationGO.SetActive (true);
			//draw duration above right hand
			durationGO.transform.position = _rightHand.transform.position;
			Vector3 up = _rightHand.transform.up;
			durationGO.transform.Translate (up.x * 0.1f, up.y * 0.1f, up.z * 0.1f);
            durationGO.transform.localRotation = Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up);

            if (Input.GetAxisRaw ("Mouse ScrollWheel") > 0) {	
				duration += Input.GetAxisRaw ("Mouse ScrollWheel");
				durationGO.GetComponent<TextMesh> ().text = duration.ToString ();
			}

			if (Input.GetAxisRaw ("Mouse ScrollWheel") < 0) {
				duration += Input.GetAxisRaw ("Mouse ScrollWheel");
				if (duration < 0)
					duration = 0.0f;
				durationGO.GetComponent<TextMesh> ().text = duration.ToString ();
			}
			Debug.Log ("duration = " + duration);
		}
		else 
			durationGO.SetActive (false);
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
            pause();
            scribbler.createRenderer(color);
            scribbler.IsActive = true;
            Debug.Log("Testing scribbler");
        }

        if (highlightPointsButtonIsActive && !highlightPoints.IsActive)
        {
            highlightPoints.init(color);
            pause();
            highlightPoints.IsActive = true;
        
            Debug.Log("Testing highlight");
        }

    }


    private void LeftMouseReleaseEvent(){
		
		//pause a frame so you don't pick up the same mouse down event.
		//yield return new WaitForEndOfFrame();
        lmbclicked = false;

        Debug.Log("LMB RELEASE");
        if (scribblerButtonIsActive && deltaHoldTime > doubleClickTimeLimit) {
			scribbler.IsActive = false;
            scribbler.assignClosestBone(trackerClientRecorded.Humans);
			ScribblerAnnotation sbAnnotation = annotationManager.AddScribblerAnnotation (scribbler.lineRendererGO, Time.time,scribbler.center);
            sbAnnotation.TimeOfCreation = currentExecutionTime;
        }

		if (highlightPointsButtonIsActive && deltaHoldTime > doubleClickTimeLimit) { 
			highlightPoints.IsActive = false;
			HighlightPointsAnnotation hpAnnotation = annotationManager.AddHighlightPointsAnnotation(highlightPoints.bonesTransforms, Time.time);
            hpAnnotation.highlightColor = color;
            hpAnnotation.TimeOfCreation = currentExecutionTime;
        }
        deltaHoldTime = 0;

    }

    private void RightMouseClickEvent()
    {
            
        RightMouseButtonSingleClick();
    }

    private void WheelMouseClickEvent()
    {
		isWheelActive = !isWheelActive;
		if (!isWheelActive) {
			annotationManager.SetAnnotationDuration (duration, _rightHand.transform.position);
			duration = 0.0f;
		}

        //WheelMouseButtonSingleClick();
    }

    private void LeftMouseButtonSingleClick()
    {
        menu.SetActive( false);
        Debug.Log("Left Mouse Button Single Click");

        if (changeColorButtonIsActive) {
            if (changeColorGO.activeSelf)
            {
                changeColorButtonIsActive = false;
                changeColorGO.SetActive(false);

            }
            else
            {
                changeColorGO.SetActive(true);

                Transform headPosition = trackerClientSimpleRobot.getHead();
                Vector3 frontVector = Camera.main.transform.forward;
                changeColorGO.transform.position = Vector3.zero;
                changeColorGO.transform.rotation = Quaternion.identity;

                changeColorGO.transform.localPosition = headPosition.position;
                changeColorGO.transform.Translate(0.7f * frontVector.x, 0.70f * frontVector.y, 0.70f * frontVector.z);
                changeColorGO.transform.localRotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

            }
        }
    }

    private void pause()
    {
        fileListener.playing = false;

        foreach (PointCloud pc in clouds)
        {
            pc.playing = false;
        }
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
			menu.transform.Translate(0.7f * frontVector.x, 0.70f * frontVector.y, 0.70f * frontVector.z);
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
					textToSpeechButtonIsActive = true;
					
				} else
				{
					textToSpeechButton.GetComponent<Renderer> ().material.SetTexture ("_MainTex", textToSpeechTextureInactive);
                    textToSpeechButtonIsActive = false;

                }

                // HIGHLIGHT POINTS
                if (hit.collider.name.Equals("HighlightPoints")){
					highlightPointsButton.GetComponent<Renderer>().material.SetTexture("_MainTex", highlightPointsTextureActive);
					highlightPointsButtonIsActive = true;
				}
				else
				{
					highlightPointsButton.GetComponent<Renderer>().material.SetTexture("_MainTex", highlightPointsTextureInactive);
                    highlightPointsButtonIsActive = false;

                }

                // SCRIBBLER
                if (hit.collider.name.Equals("Scribbler")){
					scribblerButton.GetComponent<Renderer>().material.SetTexture("_MainTex",scribblerTextureActive);
                    scribblerButtonIsActive = true;
				}
				else
				{
					scribblerButton.GetComponent<Renderer>().material.SetTexture("_MainTex", scribblerTextureInactive);
                    scribblerButtonIsActive = false;

                }

                // CHANGE COLOR
                if (hit.collider.name.Equals("ChangeColor"))
                {
                    changeColorButton.GetComponent<Renderer>().material.SetTexture("_MainTex", changeColorTextureActive);
                    changeColorButtonIsActive = true;

                }
                else
                {
                    changeColorButton.GetComponent<Renderer>().material.SetTexture("_MainTex", changeColorTextureInactive);
                    changeColorButtonIsActive = false;

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
        if (changeColorButtonIsActive)
        {
            // draw raycast vector to interact with the 3D Menu
            //Vector3 forward = transform.TransformDirection (Vector3.forward) * 10;

            //    Vector3 handpos = _rightHand.transform.position;
            //    Bounds bounds = changeColorGO.GetComponent<Renderer>().bounds;
            //    //Vector3 changeColorGOCenter = new Vector3(bounds.size.x / 2, bounds.size.y / 2, bounds.size.z / 2); //c.center;
            //    Vector3 relPosition = (handpos - changeColorGO.transform.position);


            ////    Debug.Log("distance of hand position to change color center: " + relPosition.ToString());
            //    relPosition += changeColorGOCenter;
            //  //  Debug.Log("hand position relative change color center: " + relPosition.ToString());

            //    Texture2D mainTex = changeColorGO.GetComponent<Renderer>().material.mainTexture as Texture2D;
            //    Color color = mainTex.GetPixel(Math.Abs((int)relPosition.x), Math.Abs((int)relPosition.z));
            //    Debug.Log("the color picked = " + color.ToString());

            Vector3 forward = Camera.main.transform.forward;
            Debug.DrawRay(_rightHand.transform.position, forward, Color.green, 1, false);

            if (Physics.Raycast(_rightHand.transform.position, forward, out hit))
            {
                print("Found an object - name: " + hit.collider.name);

                // SPEECH TO TEXT
                if (hit.collider.name.Equals("ColorPicker"))
                {
                    Texture2D mainTex = changeColorGO.GetComponent<Renderer>().material.mainTexture as Texture2D;
                    Vector2 pixelUV = hit.textureCoord;
                    Debug.Log("PIXUV " + pixelUV);
                    pixelUV.x *= mainTex.width;
                    pixelUV.y *= mainTex.height;

                    Debug.Log("hit position = " + hit.transform.position.ToString());

                    color = mainTex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
                    Debug.Log("the color picked = " + color.ToString());
                }
            }
        }
    }

    // Set o Mathf.Approximately doesnt work, use this
    static bool RoughlyEqual(float a, float b)
    {
        float treshold = 2f; //how much roughly
        return (Math.Abs(a - b) < treshold);
    }

    public void HandleAnnotationPlayback()
    {
        // Reset execution time
        foreach (PointCloud pc in clouds)
            currentCloud = pc.currentCloud;

        if (currentCloud == 0)
        {
            annotationManager.ResetDrawState();
            currentExecutionTime = 0.0f;
        }

        // handle playback of annotations
        List<ScribblerAnnotation> scribblerAnnotationList = annotationManager.ScribblerAnnotationList;
        foreach (ScribblerAnnotation sbAnnotation in scribblerAnnotationList)
        {
            if (Mathf.Approximately((sbAnnotation.TimeOfCreation + sbAnnotation.Duration), currentExecutionTime))
                sbAnnotation.Draw();

            if (sbAnnotation.TimeOfCreation + duration > currentExecutionTime)
                sbAnnotation.EndDraw();
        }

        List<HighlightPointsAnnotation> highlightPointsList = annotationManager.HighlightPointsAnnotationList;
        foreach (HighlightPointsAnnotation hpAnnotation in highlightPointsList)
        {
            if (Mathf.Approximately((hpAnnotation.TimeOfCreation + duration), currentExecutionTime))
                hpAnnotation.Draw();

            if (hpAnnotation.TimeOfCreation + duration > currentExecutionTime)
                hpAnnotation.EndDraw();
        }
    }

    // Update is called once per frame
    void Update () {

        currentExecutionTime += Time.deltaTime;

        handleMouseInput();
		HandleMenuOptions ();
        HandleAnnotationPlayback();

        //debug
       // Debug.Log("number of scribbler annotations: " + annotationManager.ScribblerAnnotationList.Count);
     //   foreach (ScribblerAnnotation sbAnnotation in annotationManager.ScribblerAnnotationList)
      //  {
     //      Debug.Log("scribbler duration: " + sbAnnotation.Duration.ToString());
      //  }
        
      //  Debug.Log("number of highlight points annotations: " + annotationManager.HighlightPointsAnnotationList.Count); 

	}
}
