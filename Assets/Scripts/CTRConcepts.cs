using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CTRConcepts : MonoBehaviour {


	List<GameObject> clusterList;

	// Use this for initialization
	void Start () {
	
		clusterList = new List<GameObject>();
		clusterList.AddRange (GameObject.FindGameObjectsWithTag ("Cluster"));



	}
	
	// Update is called once per frame
	void Update () {

		foreach(GameObject cluster in clusterList)
            ChangeMeshVertices(cluster);
	
	}

	void ChangeColor(GameObject cluster){
		
		Debug.Log ("change color");
		
        // change color in our GS Billboard shader
        Material[] materials = cluster.GetComponent<Renderer>().materials;
		foreach(Material mat in materials) {
			mat.color = Color.red;
		}

        // change color in standard shader
		/*Material[] materials = cluster.GetComponent<Renderer>().materials;
		foreach(Material mat in materials) {
			mat.SetColor ("_EmissionColor", Color.blue);
			mat.color = Color.red;
		}*/
		
		//Renderer clusterRenderer = cluster.GetComponent<Renderer> ();
		//clusterRenderer.material.color = new Color(239, 48, 36); 
		
		/*Mesh clusterMesh = cluster.GetComponent<MeshFilter>().mesh;
		Color[] colours = clusterMesh.colors;
		for(int i = 0; i < colours.Length; i++)
		{
			colours[i] = new Color(2 * 10, 255, 2 * 20);
		}

		Vector3[] vertices = clusterMesh.vertices;
		Color[] colors = new Color[vertices.Length];
		for(int v = 0; v < vertices.Length; v++)
		colors[0] = new Color(2 * 10, 255, 2 * 20);
		clusterMesh.colors = colors;*/
	}


    void ChangeMeshVertices(GameObject cluster)
    {
        Mesh mesh = cluster.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int i = 0;
        while (i < vertices.Length) {
            vertices[i] += normals[i] * Mathf.Sin(Time.time);
            i++;
        }
        mesh.vertices = vertices;
    
    }

	void DrawTrail(GameObject cluster){
		
		Vector3 start = this.gameObject.GetComponent<BoxCollider> ().bounds.center;
		Vector3 end = cluster.GetComponent<BoxCollider>().bounds.center;


		Material mat = Resources.Load("trailMaterial") as Material;

		GameObject newLine = new GameObject("TrailRenderer");
		TrailRenderer trailRenderer = newLine.AddComponent<TrailRenderer> ();
		trailRenderer.material = mat;
		trailRenderer.material.SetColor ("greenColor", Color.green);

		trailRenderer.transform.position = cluster.transform.position;
	}
}
