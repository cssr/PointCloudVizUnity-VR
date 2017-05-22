using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class ButtonManager : MonoBehaviour {
	
	
	GameObject buttonNumber;
	int maxFastFoward = 6;
	//bool playing = false;
    PointCloud[] clouds = null;
	float numberOfFrames = 0.5f;

	bool isPlaying = false;
	
	void Start() {

		clouds = GameObject.FindObjectsOfType<PointCloud> ();
	
	}
	
	public void OnClickNextFrame(){
		foreach (PointCloud pc in clouds)
		{
			pc.currentCloud++;
		}
	}
	
	public void OnClickPreviousFrame(){
		foreach (PointCloud pc in clouds)
		{
			pc.currentCloud--;
		}
	}
	
	public void OnClickNextJumpFrames(){
		foreach (PointCloud pc in clouds)
		{
			pc.currentCloud+=10;
		}
	}
	
	public void OnClickPreviousJumpFrames(){
		foreach (PointCloud pc in clouds)
		{
			pc.currentCloud-=10;
		}
	}
	
	public void OnClickPause(){
		foreach (PointCloud pc in clouds)
		{
			
			if(pc.playing) {
				pc.playing = false;
			} 
			else {
				pc.playing = true;
				
			}
		}
	}

	void Update(){
	
		if (Input.GetKeyUp (KeyCode.Space)) {
			foreach (PointCloud pc in clouds)
			{

				if(pc.playing) {
					pc.playing = false;
				} 
				else {
					pc.playing = true;
					Debug.Log ("is playing ?" + pc.playing);

				}
			}
		}
	
	}
	
}
