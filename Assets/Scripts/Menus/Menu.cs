using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu {

    public GameObject menu;

    private GameObject textToSpeech;
    public GameObject TextToSpeech
    {
        get
        {
            return textToSpeech;
        }

        set
        {
            textToSpeech = value;
        }
    }

    private GameObject scribbler;
    public GameObject Scribbler
    {
        get
        {
            return scribbler;
        }

        set
        {
            scribbler = value;
        }
    }

    private GameObject highlightPoints;
    public GameObject HighlightPoints
    {
        get
        {
            return highlightPoints;
        }

        set
        {
            highlightPoints = value;
        }
    }

    private GameObject changeColor;
    public GameObject ChangeColor
    {
        get
        {
            return changeColor;
        }

        set
        {
            changeColor = value;
        }
    }

    // Use this for initialization
    void Init () {

        TextToSpeech = GameObject.Find("TextToSpeech");
        Scribbler = GameObject.Find("Scribbler");
        HighlightPoints = GameObject.Find("HighlightPoints");
        ChangeColor = GameObject.Find("ChangeColor");
	}

    public void ChangeTextToSpeechTexture(string texture) {
       // Renderer renderer = textToSpeech.GetComponent<Renderer>();
       // renderer.material = 
    }
	
}
