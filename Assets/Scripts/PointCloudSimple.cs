using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PointCloudSimple : MonoBehaviour {
	public string meshDirPath;
	public string cameraFilePath;
	Mesh[] cloud;
	int nclouds;
	
	public void setPoints(List<Point3DRGB> input){
		List<Vector3> points = new List<Vector3> ();
		List<int> ind = new List<int> ();
		List<Color> colors = new List<Color> ();
		cloud = new Mesh[4];
		nclouds = 0;
		Mesh m = new Mesh ();
		int i = 0;
		foreach(Point3DRGB p in input){
			points.Add(p.point);
			colors.Add(p.color);
			ind.Add (i);
			i++;
			if(i == 65000){
				m.vertices = points.ToArray ();
				m.colors = colors.ToArray ();
				m.SetIndices (ind.ToArray(), MeshTopology.Points, 0);
				cloud[nclouds] = m;
				m = new Mesh();
				i = 0;
				points.Clear();
				colors.Clear();
				ind.Clear();
				nclouds++;
			}
		}
		
		m.vertices = points.ToArray ();
		m.colors = colors.ToArray ();
		m.SetIndices (ind.ToArray(), MeshTopology.Points, 0);
		cloud[nclouds] = m;
	}

	public void setToView(){
		MeshFilter[] filters = GetComponentsInChildren<MeshFilter> ();
		int i = 0;
		foreach (MeshFilter mf in filters) {
			if(i <= nclouds){
				mf.mesh = cloud[i++];
			}else{
				mf.mesh.Clear();
			}
		}
	}

	public void hideFromView(){
		MeshFilter[] filters = GetComponentsInChildren<MeshFilter> ();
		foreach (MeshFilter mf in filters) {
			mf.mesh.Clear();
		}	
	}

	// Use this for initialization
	void Start () {
		Material mat = Resources.Load ("cloudmat") as Material;
		for (int i = 0; i < 4; i++) {
			GameObject a = new GameObject();
			MeshFilter mf = a.AddComponent<MeshFilter>();
			MeshRenderer mr = a.AddComponent<MeshRenderer>();
			mr.material = mat;
			a.transform.parent = this.gameObject.transform;
		}
	}

}