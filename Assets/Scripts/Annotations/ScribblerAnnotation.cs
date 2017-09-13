using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScribblerAnnotation {

	public int ID { get; set; }
	public List<LineRenderer> lineRenderers { get; set; }
	public Dictionary<BodyJointType, Vector3> Joints { get; set; }

}
