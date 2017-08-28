using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PointCloud : MonoBehaviour {
	public string meshDirPath;
	public string cameraFilePath;
	public int skipFirstX = 0;
	public int currentCloud = 0;
	public bool playing;
	public string calibrationName = "";
	public List<Transform> TestHighlight; 

	int maxclouds = 0;
	Dictionary<int,List<Mesh>> meshes;
	Material mat;


	// Use this for initialization
	void Start () {
		meshes = new Dictionary<int, List<Mesh>> ();
		string[] dira = Directory.GetFiles(meshDirPath);
		foreach (string f in dira) {
			char[] sepFile = { '\\' };
			string[] path = f.Split (sepFile);
			string name = path [path.Length - 1];
			if(name.Equals(".DS_Store"))
				continue;
			name = name.Replace ("outputCloud", "");
			int id = int.Parse (name);
			id-=skipFirstX;
			if(id < 0) continue;
			List<Mesh> thecloud = readFile (f);
			maxclouds = maxclouds < thecloud.Count?thecloud.Count:maxclouds;
			meshes.Add(id,thecloud);
		}
		
		mat = Resources.Load ("cloudmat") as Material;
		List<Mesh> first = meshes [0];
		for (int i = 0; i < maxclouds; i++) {
			gameObject.AddComponent<MeshFilter>();
			MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
			mr.material = mat;
		}
		
		// invert scale on x axis to compensate kinect mirroring the data
		transform.localScale = new Vector3(-1.0F, 1, 1);
		
		setCloudToRender (first,true);
		//setInitialPositionIni ();
		CalibrateLopesGraç();
		
	}
	
	private void setInitialPosition(){
		FileStream fs = new FileStream(cameraFilePath, FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		char[] sep = {' '};
		string pos = sr.ReadLine ();
		string[] p = pos.Split (sep);
		string rot = sr.ReadLine ();
		string[] r = rot.Split (sep);
		Vector3 position = new Vector3 (float.Parse (p [0]), float.Parse (p [1]), float.Parse (p [2]));
		Vector3 rotation = new Vector3 (float.Parse (r [0]), float.Parse (r [1]), float.Parse (r [2]));
		this.gameObject.transform.Translate (position);
		this.gameObject.transform.Rotate (rotation);
	}
	
	private void setInitialPositionIni(){
		FileStream fs = new FileStream (cameraFilePath, FileMode.Open);
		StreamReader sr = new StreamReader (fs);
		float[] values = new float[6];
		for (int i = 0; i < 6; i++) {
			char[] sep = {'='};
			string pos = sr.ReadLine ();
			string[] p = pos.Split (sep);
			values[i] = float.Parse(p[1]);
			
		}
		Vector3 position = new Vector3 (values[0],values[1],values[2]);
		Vector3 rotation = new Vector3 (values[3],values[4],values[5]);
		this.gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
		this.gameObject.transform.Translate (position);
		this.gameObject.transform.Rotate (rotation);
		
	}

	private void CalibrateLopesGraç(){

		FileStream fs = new FileStream (cameraFilePath, FileMode.Open);
		StreamReader sr = new StreamReader (fs);
		string delimiter = ";";
		string line = "";
		while((line = sr.ReadLine()) != null)
		{
			string[] lineSplited = line.Split (';');
			if (lineSplited.Length == 8) {
				string[] calibrationInfo = lineSplited [0].Split ('=');
				if (calibrationInfo [1].Equals (calibrationName)) {
					Vector3 position = new Vector3 (float.Parse(lineSplited[1]),float.Parse(lineSplited[2]),
						float.Parse(lineSplited[3]));
					Quaternion rotation = new Quaternion (float.Parse(lineSplited[4]),float.Parse(lineSplited[5]),
						float.Parse(lineSplited[6]),float.Parse(lineSplited[7]));

					this.gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
					this.gameObject.transform.position =position;
					this.gameObject.transform.rotation = rotation;
				}
			}
		}
	}



	
	
	
	private void setCloudToRender(List<Mesh> meshes, bool show){
		Renderer[] r = gameObject.GetComponents<Renderer> ();
		foreach (Renderer r1 in r) {
			if (show) {
				r1.enabled = true;
			} else {
				r1.enabled = false;
			}
		}
		MeshFilter[] filters = gameObject.GetComponents<MeshFilter> ();
		int i = 0;
		foreach (MeshFilter mf in filters) {
			if(i < meshes.Count){
				mf.sharedMesh = meshes[i++];
			}else{
				mf.sharedMesh.Clear();
			}
		}
	}
	
	private List<Mesh> readFile(string f)
	{
		FileStream fs = new FileStream(f, FileMode.Open);
		StreamReader sr = new StreamReader(fs);
		
		List<Vector3> points = new List<Vector3> ();
		List<int> ind = new List<int> ();
		List<Color> colors = new List<Color> ();
		List<Mesh> clouds = new List<Mesh> ();
		
		string line = "";
		int i = 0;
		Mesh m = new Mesh ();
		while (!sr.EndOfStream) {
			line = sr.ReadLine ();
			line = line.Replace(",",".");
			char[] sep = { ' ' };
			string[] lin = line.Split (sep);
			if (lin.Length == 6 && lin [0] != "" && lin [1] != "" && lin [2] != "") {
				float x = float.Parse (lin [0]);
				float y = float.Parse (lin [1]);
				float z = float.Parse (lin [2]);
				
				bool use = false;
				Color c = new Color ();
				if (lin [3] != "0" && lin [4] != "0" && lin [5] != "0" && lin [3] != "" && lin [4] != "" && lin [5] != "") {
					c.r = int.Parse (lin [5]) / 255.0f;
					c.g = int.Parse (lin [4]) / 255.0f;
					c.b = int.Parse (lin [3]) / 255.0f;
					use = true;
					
				}
				if (use) {
					points.Add(new Vector3(x,y,z));
					ind.Add(i);
					colors.Add (c);
					i++;
				}
			}
			if(i == 65000){
				m.vertices = points.ToArray ();
				m.colors = colors.ToArray ();
				m.SetIndices (ind.ToArray(), MeshTopology.Points, 0);
				clouds.Add(m);
				m = new Mesh();
				i = 0;
				points.Clear();
				colors.Clear();
				ind.Clear();
			}
		}
		m.vertices = points.ToArray ();
		m.colors = colors.ToArray ();
		m.SetIndices (ind.ToArray(), MeshTopology.Points, 0);
		clouds.Add(m);
		fs.Close ();
		return clouds;
	}
	
	void FixedUpdate(){
		Quaternion orient = transform.rotation;
		Quaternion cameraRot = Camera.main.transform.rotation;
		float ydiff = Mathf.Abs(orient.eulerAngles.y - cameraRot.eulerAngles.y);
		bool show = true;
		if (ydiff > 90 || cameraRot.eulerAngles.x < -30 || cameraRot.eulerAngles.z < -30) {
			//	show = false;
		}

		List<float> positions = new List<float> ();
		foreach (Transform t in TestHighlight) {
			positions.Add (t.position.x);
			positions.Add (t.position.y);
			positions.Add (t.position.z);
		}
		if (positions.Count != 0) {
			mat.SetFloatArray ("_BonesPositions", positions);
			mat.SetInt ("_BonesPositionsLenght", positions.Count);
		}
		if (currentCloud == meshes.Count)
			currentCloud = 0;
		setCloudToRender (meshes [currentCloud],show);
		if (playing)
			currentCloud++;


	}
	
}