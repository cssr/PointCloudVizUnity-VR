using UnityEngine;
using UnityEngine.VR;
using System;
using MyTechnic;
using System.Collections.Generic;

public class TrackerClientRecorded : MonoBehaviour
{
	// Filter parameters
	private bool isNewFrame;
	private DateTime frameTime;

	// Human

	public Dictionary<string, Human> Humans { get; set; }


	void Awake()
	{
		isNewFrame = false;
		frameTime = DateTime.Now;


		Humans = new Dictionary<string, Human>();

	}




	void Update()
	{	
		foreach(Human h in Humans.Values) 
		{
			h.UpdateAvatarBody(isNewFrame,frameTime);
		}

		// Finally
		CleanDeadHumans();
		isNewFrame = false;
	}

	/// <summary>
	/// Gets the first human identifier with a hand above the head.
	/// </summary>
	private string GetHumanIdWithHandUp()
	{
		foreach (Human h in Humans.Values) 
		{
			if (h.body.Joints[BodyJointType.leftHand].y  > h.body.Joints[BodyJointType.head].y ||
				h.body.Joints[BodyJointType.rightHand].y > h.body.Joints[BodyJointType.head].y)
			{
				return h.id;
			}
		}
		return string.Empty;
	}

	/// <summary>
	/// Updates the avatar body by filtering body 
	/// joints and applying them through rotations.
	/// </summary>


	/// <summary>
	/// Updates frame with new body data if any.
	/// </summary>
	public void SetNewFrame(Body[] bodies)
	{
		isNewFrame = true;
		frameTime = DateTime.Now;

		foreach (Body b in bodies) 
		{
			try
			{  
				string bodyID = b.Properties[BodyPropertiesType.UID];
				if (!Humans.ContainsKey(bodyID)) 
				{
					Humans.Add(bodyID, new Human(true));
				}
				Humans[bodyID].Update(b);
			} 
			catch (Exception e) 
			{
				Debug.LogError("[TrackerClient] ERROR: " + e.StackTrace);
			}
		}
	}

	void CleanDeadHumans()
	{
		List<Human> deadhumans = new List<Human>();

		foreach (Human h in Humans.Values) 
		{
			if (DateTime.Now > h.lastUpdated.AddMilliseconds(1000))
			{
				deadhumans.Add(h);
			}
		}
		foreach (Human h in deadhumans) 
		{
			Humans.Remove(h.id);
		}
	}
}