using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedBodyRepresentation  {

	public const float ballSize = 0.1f;
	public const float boneSize = 0.05f;
	// Body transforms and joints

	// Spine
	public Transform spineBase;
	public Transform spineShoulder;
	public Transform head;

	public PointSmoothing spineBaseJoint;
	public PointSmoothing spineShoulderJoint;
	public PointSmoothing headJoint;

	// Left arm
	public Transform leftShoulder;
	public Transform leftElbow;
	public Transform leftArm;

	public PointSmoothing leftShoulderJoint;
	public PointSmoothing leftElbowJoint;
	public PointSmoothing leftWristJoint;

	// Left leg
	public Transform leftHip;
	public Transform leftKnee;
	public Transform leftAnkle;

	public PointSmoothing leftHipJoint;
	public PointSmoothing leftKneeJoint;
	public PointSmoothing leftAnkleJoint;

	// Right arm
	public Transform rightShoulder;
	public Transform rightElbow;
	public Transform rightArm;

	public PointSmoothing rightShoulderJoint;
	public PointSmoothing rightElbowJoint;
	public PointSmoothing rightWristJoint;

	// Right leg
	public Transform rightHip;
	public Transform rightKnee;
	public Transform rightAnkle;


	public PointSmoothing rightHipJoint;
	public PointSmoothing rightKneeJoint;
	public PointSmoothing rightAnkleJoint;

	// Bones
	public Transform boneNeck, boneSpine;
	public Transform boneLeftShoulder, boneLeftArm, boneLeftForearm;
	public Transform boneRightShoulder, boneRightArm, boneRightForearm;
	public Transform boneLeftHip, boneLeftThigh, boneLeftCalf;
	public Transform boneRightHip, boneRightThigh, boneRightCalf;

	// Use this for initialization
	public TrackedBodyRepresentation () {

		spineBaseJoint = new PointSmoothing();
		spineShoulderJoint = new PointSmoothing();
		headJoint = new PointSmoothing();

		leftShoulderJoint = new PointSmoothing();
		leftElbowJoint = new PointSmoothing();
		leftWristJoint = new PointSmoothing();
		leftHipJoint = new PointSmoothing();
		leftKneeJoint = new PointSmoothing();
		leftAnkleJoint = new PointSmoothing();

		rightShoulderJoint = new PointSmoothing();
		rightElbowJoint = new PointSmoothing();
		rightWristJoint = new PointSmoothing();
		rightHipJoint = new PointSmoothing();
		rightKneeJoint = new PointSmoothing();
		rightAnkleJoint = new PointSmoothing();

		GameObject avatarGo = new GameObject();

		spineBase = createAvatarJoint(avatarGo.transform,"spineBase");
		spineShoulder = createAvatarJoint(avatarGo.transform,"spineShoulder");
		head = createAvatarJoint(avatarGo.transform,"head",0.20f);

		leftShoulder = createAvatarJoint(avatarGo.transform,"leftShoulder");
		leftElbow = createAvatarJoint(avatarGo.transform,"leftElbow");
		leftArm = createAvatarJoint(avatarGo.transform,"leftArm");
		leftHip = createAvatarJoint(avatarGo.transform,"leftHip");
		leftKnee = createAvatarJoint(avatarGo.transform,"leftKnee");
		leftAnkle = createAvatarJoint(avatarGo.transform,"leftAnkle");

		rightShoulder = createAvatarJoint(avatarGo.transform,"rightShoulder");
		rightElbow = createAvatarJoint(avatarGo.transform,"rightElbow");
		rightArm = createAvatarJoint(avatarGo.transform,"rightArm");
		rightHip = createAvatarJoint(avatarGo.transform,"rightHip");
		rightKnee = createAvatarJoint(avatarGo.transform,"rightKnee");
		rightAnkle = createAvatarJoint(avatarGo.transform,"rightAnkle");

		boneNeck = createAvatarBone(avatarGo.transform);
		boneSpine = createAvatarBone(avatarGo.transform);
		boneLeftShoulder = createAvatarBone(avatarGo.transform);
		boneLeftArm = createAvatarBone(avatarGo.transform);
		boneLeftForearm = createAvatarBone(avatarGo.transform);
		boneRightShoulder = createAvatarBone(avatarGo.transform);
		boneRightArm = createAvatarBone(avatarGo.transform);
		boneRightForearm = createAvatarBone(avatarGo.transform);
		boneLeftHip = createAvatarBone(avatarGo.transform);
		boneLeftThigh = createAvatarBone(avatarGo.transform);
		boneLeftCalf = createAvatarBone(avatarGo.transform);
		boneRightHip = createAvatarBone(avatarGo.transform);
		boneRightThigh = createAvatarBone(avatarGo.transform);
		boneRightCalf = createAvatarBone(avatarGo.transform);

	}


	Transform createAvatarJoint(Transform parent, string name, float scale = ballSize)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		gameObject.name = name;
		Rigidbody r = gameObject.AddComponent<Rigidbody>();
		r.isKinematic = true;
		r.useGravity = false;
		Transform transform = gameObject.transform;
		transform.parent = parent;
		transform.localScale *= scale;
		return transform;
	}

	Transform createAvatarBone(Transform parent, float scale = boneSize)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		Transform transform = gameObject.transform;
		transform.parent = parent;
		return transform;
	}
	// Update is called once per frame

	void updateAvatarBone(Transform bone, Vector3 joint1, Vector3 joint2)
	{
		if (bone == null)
			return;
		bone.localScale = new Vector3(boneSize, 0.5f * Vector3.Distance(joint1, joint2), boneSize);
		bone.position = (joint1 + joint2) * 0.5f;
		bone.up = joint2 - joint1;
	}

	public void updateAvatarBody()
	{
		Vector3 spineUp = MyTechnic.Utils.GetBoneDirection(spineShoulderJoint.Value, spineBaseJoint.Value);
		Vector3 spineRight = MyTechnic.Utils.GetBoneDirection(rightShoulderJoint.Value, leftShoulderJoint.Value);
		Vector3 spineForward = Vector3.Cross(spineRight, spineUp);
		// Spine
		spineBase.position = spineBaseJoint.Value;
		spineShoulder.position = spineShoulderJoint.Value;
		head.position = headJoint.Value;
		head.rotation = Quaternion.LookRotation(spineForward, spineUp);

		// Left Arm
		leftShoulder.position = leftShoulderJoint.Value;
		leftArm.position = leftElbowJoint.Value;
		leftElbow.position = leftWristJoint.Value;

		// Left Leg
		leftHip.position = leftHipJoint.Value;
		leftKnee.position = leftKneeJoint.Value;
		leftAnkle.position = leftAnkleJoint.Value;

		// Right Arm
		rightShoulder.position = rightShoulderJoint.Value;
		rightArm.position = rightElbowJoint.Value;
		rightElbow.position = rightWristJoint.Value;

		// Right Leg
		rightHip.position = rightHipJoint.Value;
		rightKnee.position = rightKneeJoint.Value;
		rightAnkle.position = rightAnkleJoint.Value;

		// Bones
		updateAvatarBone(boneNeck, head.position, spineShoulder.position);
		updateAvatarBone(boneSpine, spineBase.position, spineShoulder.position);

		updateAvatarBone(boneLeftShoulder, leftShoulder.position, spineShoulder.position);
		updateAvatarBone(boneLeftArm, leftShoulder.position, leftArm.position);
		updateAvatarBone(boneLeftForearm, leftArm.position, leftElbow.position);

		updateAvatarBone(boneRightShoulder, rightShoulder.position, spineShoulder.position);
		updateAvatarBone(boneRightArm, rightShoulder.position, rightArm.position);
		updateAvatarBone(boneRightForearm, rightArm.position, rightElbow.position);

		updateAvatarBone(boneLeftHip, spineBase.position, leftHip.position);
		updateAvatarBone(boneLeftThigh, leftHip.position, leftKnee.position);
		updateAvatarBone(boneLeftCalf, leftKnee.position, leftAnkle.position);

		updateAvatarBone(boneRightHip, spineBase.position, rightHip.position);
		updateAvatarBone(boneRightThigh, rightHip.position, rightKnee.position);
		updateAvatarBone(boneRightCalf, rightKnee.position, rightAnkle.position);
	}


}
