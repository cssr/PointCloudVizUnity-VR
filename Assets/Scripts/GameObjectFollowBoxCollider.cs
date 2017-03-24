using UnityEngine;
using System.Collections;

public class GameObjectFollowBoxCollider : MonoBehaviour {

	// Use this for initialization
    BoxCollider parentBoxCollider;
	TrailRenderer trail;

	void Start () {
        //parentBoxCollider = transform.parent.GetComponent<BoxCollider>();
		//trail = gameObject.GetComponent<TrailRenderer> ();
	
	
	}
	
	// Update is called once per frame
	void Update () {

		parentBoxCollider = transform.parent.GetComponent<BoxCollider>();

        transform.position = new Vector3(parentBoxCollider.center.x, parentBoxCollider.center.y, 
            parentBoxCollider.center.z);

		//trail.startWidth = parentBoxCollider.size.x;

		Debug.Log ("parent = " + transform.parent.name);
	}
}
