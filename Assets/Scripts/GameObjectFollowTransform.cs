using UnityEngine;
using System.Collections;

public class GameObjectFollowTransform : MonoBehaviour {

	BoxCollider parentBoxCollider;
	//Transform parentTransform;
	public float maxDistance = 1.0f;
    float lerpStep = 0.05f;

	// Use this for initialization
	void Start () {

		//parentTransform = transform.parent.transform;
		parentBoxCollider = transform.parent.GetComponent<BoxCollider>();

	}
	
	// Update is called once per frame
	void Update () {
	
		//float lerpStep = Vector3.Distance (transform.position, parentTransform.position) / Time.deltaTime;
	    float distance = Vector3.Distance (transform.position, parentBoxCollider.center);
		Debug.Log ("distance = " + distance);

		if(distance < maxDistance)
            transform.position = Vector3.Lerp(transform.position, parentBoxCollider.center, lerpStep);
            //  transform.position = parentBoxCollider.center;

		// interpolate position
        
	}
}
