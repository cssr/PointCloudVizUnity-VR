using UnityEngine;
using UnityEngine.VR;
using System;
using MyTechnic;
using System.Collections.Generic;

public class TrackerClientSimpleRobot : MonoBehaviour
{
	// Filter parameters
	private bool isNewFrame;
	private DateTime frameTime;

	// Human
	private Human trackedHuman;
	private string trackedHumanId;
	private Dictionary<string, Human> humans;
    public const float ballSize = 0.1f;
    public const float boneSize = 0.05f;
    public bool showHead = true;


    // Body transforms and joints

    // Spine
    private Transform spineBase;
    private Transform spineShoulder;
    private Transform head;

    private PointSmoothing spineBaseJoint;
	private PointSmoothing spineShoulderJoint;
    private PointSmoothing headJoint;

    // Left arm
    private Transform leftShoulder;
    private Transform leftElbow;
    private Transform leftArm;

	private PointSmoothing leftShoulderJoint;
	private PointSmoothing leftElbowJoint;
	private PointSmoothing leftWristJoint;

    // Left leg
    private Transform leftHip;
    private Transform leftKnee;
    private Transform leftAnkle;

    private PointSmoothing leftHipJoint;
	private PointSmoothing leftKneeJoint;
	private PointSmoothing leftAnkleJoint;

    // Right arm
    private Transform rightShoulder;
    private Transform rightElbow;
    private Transform rightArm;

	private PointSmoothing rightShoulderJoint;
	private PointSmoothing rightElbowJoint;
	private PointSmoothing rightWristJoint;

    // Right leg
    private Transform rightHip;
    private Transform rightKnee;
    private Transform rightAnkle;


    private PointSmoothing rightHipJoint;
	private PointSmoothing rightKneeJoint;
	private PointSmoothing rightAnkleJoint;

    // Bones
    private Transform boneNeck, boneSpine;
    private Transform boneLeftShoulder, boneLeftArm, boneLeftForearm;
    private Transform boneRightShoulder, boneRightArm, boneRightForearm;
    private Transform boneLeftHip, boneLeftThigh, boneLeftCalf;
    private Transform boneRightHip, boneRightThigh, boneRightCalf;



    void Awake()
	{
		isNewFrame = false;
		frameTime = DateTime.Now;

		trackedHumanId = string.Empty;
		humans = new Dictionary<string, Human>();

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
        head.gameObject.GetComponent<Renderer>().enabled = showHead;

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
        boneNeck.gameObject.GetComponent<Renderer>().enabled = showHead;
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

    public Transform getHead()
    {
        return head;
    }

    public Transform getRightArm()
    {
        return rightElbow;
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

    void updateAvatarBone(Transform bone, Vector3 joint1, Vector3 joint2)
    {
        if (bone == null)
            return;
        bone.localScale = new Vector3(boneSize, 0.5f * Vector3.Distance(joint1, joint2), boneSize);
        bone.position = (joint1 + joint2) * 0.5f;
        bone.up = joint2 - joint1;
    }

    void Update()
	{
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C)) // Mouse tap
		{
			string currentHumanId = GetHumanIdWithHandUp();

			if (humans.ContainsKey(currentHumanId)) 
			{
				trackedHumanId = currentHumanId;
				trackedHuman = humans[trackedHumanId];
               
                InputTracking.Recenter();
			}
		}

		if (humans.ContainsKey(trackedHumanId)) 
		{
			trackedHuman = humans[trackedHumanId];
			UpdateAvatarBody();
		}

		// Finally
		CleanDeadHumans();
		isNewFrame = false;
	}

	/// <summary>
	/// Gets the first human identifier with a hand above the head.
	/// </summary>
	private string GetHumanIdWithHandUp()
	{
		foreach (Human h in humans.Values) 
		{
			if (h.body.Joints[BodyJointType.leftHand].y  > h.body.Joints[BodyJointType.head].y ||
				h.body.Joints[BodyJointType.rightHand].y > h.body.Joints[BodyJointType.head].y)
			{
				return h.id;
			}
		}
		return string.Empty;
	}

	/// <summary>
	/// Updates the avatar body by filtering body 
	/// joints and applying them through rotations.
	/// </summary>
	private void UpdateAvatarBody()
	{
		ApplyFilterToJoints();

        Vector3 spineUp = Utils.GetBoneDirection(spineShoulderJoint.Value, spineBaseJoint.Value);
        Vector3 spineRight = Utils.GetBoneDirection(rightShoulderJoint.Value, leftShoulderJoint.Value);
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

    /// <summary>
    /// Applies the noise filter to joints.
    /// </summary>
    private void ApplyFilterToJoints()
	{
		// Spine
		spineBaseJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.spineBase], isNewFrame, frameTime);
		spineShoulderJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.spineShoulder], isNewFrame, frameTime);
        headJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.head], isNewFrame, frameTime);

        // Left arm
        leftShoulderJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftShoulder], isNewFrame, frameTime);
		leftElbowJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftElbow], isNewFrame, frameTime);
		leftWristJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftWrist], isNewFrame, frameTime);

		// Left leg
		leftHipJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftHip], isNewFrame, frameTime);
		leftKneeJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftKnee], isNewFrame, frameTime);
		leftAnkleJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.leftFoot], isNewFrame, frameTime);

		// Right arm
		rightShoulderJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightShoulder], isNewFrame, frameTime);
		rightElbowJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightElbow], isNewFrame, frameTime);
		rightWristJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightWrist], isNewFrame, frameTime);

		// Right leg
		rightHipJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightHip], isNewFrame, frameTime);
		rightKneeJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightKnee], isNewFrame, frameTime);
		rightAnkleJoint.ApplyFilter(trackedHuman.body.Joints[BodyJointType.rightFoot], isNewFrame, frameTime);
	}

	/// <summary>
	/// Updates frame with new body data if any.
	/// </summary>
	public void SetNewFrame(Body[] bodies)
	{
		isNewFrame = true;
		frameTime = DateTime.Now;

		foreach (Body b in bodies) 
		{
            try
            {  
				string bodyID = b.Properties[BodyPropertiesType.UID];
				if (!humans.ContainsKey(bodyID)) 
				{
					humans.Add(bodyID, new Human(false));
				}
				humans[bodyID].Update(b);
			} 
			catch (Exception e) 
			{
				Debug.LogError("[TrackerClient] ERROR: " + e.StackTrace);
			}
		}
	}

	void CleanDeadHumans()
	{
		List<Human> deadhumans = new List<Human>();

		foreach (Human h in humans.Values) 
		{
			if (DateTime.Now > h.lastUpdated.AddMilliseconds(1000))
			{
				deadhumans.Add(h);
			}
		}
		foreach (Human h in deadhumans) 
		{
			humans.Remove(h.id);
		}
	}
}