using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class FileListener : MonoBehaviour {

	public static string NoneMessage = "0";

	public string fileLocation;
	StreamReader file;
	private List<string> _stringsToParse;
    public bool playing;

	void Start()
	{ 
		_stringsToParse = new List<string>();
		file = new StreamReader (fileLocation);
		Debug.Log("[FileListener] reading from: " + fileLocation);
        playing = true;
	}


	void FixedUpdate()
    {
        if (playing)
        {
            string stringToParse = file.ReadLine();
            if (stringToParse == null)
            {
                file.BaseStream.Position = 0;
                file.DiscardBufferedData();
                stringToParse = file.ReadLine();
            }

            List<Body> bodies = new List<Body>();
            if (stringToParse.Length != 1)
            {
                List<string> bstrings = new List<string>(stringToParse.Split(MessageSeparators.L1));
                bstrings.RemoveAt(0); // first statement is not a body
                foreach (string b in bstrings)
                {
                    if (b != NoneMessage) bodies.Add(new Body(b));
                }
            }
            gameObject.GetComponent<TrackerClientRecorded>().SetNewFrame(bodies.ToArray());
        }
    }

	void OnApplicationQuit()
	{
		file.Close ();
	}

	void OnQuit()
	{
		OnApplicationQuit();
	}
}
