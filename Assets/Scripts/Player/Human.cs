using UnityEngine;
using System;
using System.Collections;

public class Human 
{
	private string _id;
	public string id { get { return _id; } }

	private Body _body;
	public Body body { get { return _body; } }

	private DateTime _lastUpdated;
	public DateTime lastUpdated { get { return _lastUpdated; } }

	private TrackedBodyRepresentation _tbrepresentation;
	public TrackedBodyRepresentation tbr { get { return _tbrepresentation; } }

	public Human()
	{
		_body = null;
		_id = null;
		_tbrepresentation = new TrackedBodyRepresentation ();
	}


	public void UpdateAvatarBody(bool isNewFrame, DateTime frameTime)
	{
		ApplyFilterToJoints(isNewFrame,frameTime);
		tbr.updateAvatarBody ();
	}

	/// <summary>
	/// Applies the noise filter to joints.
	/// </summary>
	private void ApplyFilterToJoints(bool isNewFrame, DateTime frameTime)
	{
		// Spine
		tbr.spineBaseJoint.ApplyFilter(body.Joints[BodyJointType.spineBase], isNewFrame, frameTime);
		tbr.spineShoulderJoint.ApplyFilter(body.Joints[BodyJointType.spineShoulder], isNewFrame, frameTime);
		tbr.headJoint.ApplyFilter(body.Joints[BodyJointType.head], isNewFrame, frameTime);

		// Left arm
		tbr.leftShoulderJoint.ApplyFilter(body.Joints[BodyJointType.leftShoulder], isNewFrame, frameTime);
		tbr.leftElbowJoint.ApplyFilter(body.Joints[BodyJointType.leftElbow], isNewFrame, frameTime);
		tbr.leftWristJoint.ApplyFilter(body.Joints[BodyJointType.leftWrist], isNewFrame, frameTime);

		// Left leg
		tbr.leftHipJoint.ApplyFilter(body.Joints[BodyJointType.leftHip], isNewFrame, frameTime);
		tbr.leftKneeJoint.ApplyFilter(body.Joints[BodyJointType.leftKnee], isNewFrame, frameTime);
		tbr.leftAnkleJoint.ApplyFilter(body.Joints[BodyJointType.leftFoot], isNewFrame, frameTime);

		// Right arm
		tbr.rightShoulderJoint.ApplyFilter(body.Joints[BodyJointType.rightShoulder], isNewFrame, frameTime);
		tbr.rightElbowJoint.ApplyFilter(body.Joints[BodyJointType.rightElbow], isNewFrame, frameTime);
		tbr.rightWristJoint.ApplyFilter(body.Joints[BodyJointType.rightWrist], isNewFrame, frameTime);

		// Right leg
		tbr.rightHipJoint.ApplyFilter(body.Joints[BodyJointType.rightHip], isNewFrame, frameTime);
		tbr.rightKneeJoint.ApplyFilter(body.Joints[BodyJointType.rightKnee], isNewFrame, frameTime);
		tbr.rightAnkleJoint.ApplyFilter(body.Joints[BodyJointType.rightFoot], isNewFrame, frameTime);
	}





	public void Update(Body newBody)
	{
		_body = newBody;
		_id = _body.Properties[BodyPropertiesType.UID];
		_lastUpdated = DateTime.Now;
	}
}