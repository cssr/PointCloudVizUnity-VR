using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PointCloudSimple))]
public class PointCloudSimpleEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		if(GUILayout.Button("Save Position")){
			PointCloudSimple c = (PointCloudSimple) target;
			string lines = "posx="+c.transform.position.x + "\r\nposy=" + c.transform.position.y + "\r\nposz= "+c.transform.position.z+ "\r\nrotx="
				+ c.transform.rotation.eulerAngles.x + "\r\nroty= " + c.transform.rotation.eulerAngles.y +"\r\nrotz="+ c.transform.rotation.eulerAngles.z;
			
			// Write the string to a file.
			System.IO.StreamWriter file = new System.IO.StreamWriter(target.name+".ini");
			file.WriteLine(lines);
			file.Close();
		}
	}
}