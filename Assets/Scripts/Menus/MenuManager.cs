using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class MenuManager : MonoBehaviour {
    
	public GameObject character;
    public GameObject menu;

    private Transform _rightHand;
    private float doubleClickTimeLimit = 0.25f;
    private PointCloud[] clouds = null;

    TextToSpeech textToSpeech;
    Scribbler scribbler;
    HighlightPoints highlightPoints;
    ChangeColor changeColor;
   

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
    }

    // Update is called once per frame
    private IEnumerator InputListener()
    {
        while (enabled)
        { //Run as long as this is activ

            if (Input.GetMouseButtonDown(0))
                yield return LeftMouseClickEvent();

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
        scribbler.Drawing = true;
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
            menu.SetActive(false);
        }
        else
        {
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
    
        // draw raycast vector to interact with the 3D Menu
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
		Debug.DrawRay(_rightHand.transform.position, forward, Color.green, 1, false);
	}
}
