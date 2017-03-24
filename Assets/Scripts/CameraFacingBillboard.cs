using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour {


	public Camera camera;

	// Use this for initialization
	void Awake () {
		camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
		                 camera.transform.rotation * Vector3.up);
	}
}
