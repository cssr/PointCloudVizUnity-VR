using UnityEngine;
using System;
using System.Collections;

public class PointSmoothing
{
	private bool isInitializated;
	public static float lerpTime = 200.0f;

	// Lerp accumulated through time
	private float accumulatedLerp;

	// Points
	private Vector3 initPoint;
	private Vector3 currPoint;
	private AdaptiveDoubleExponentialFilterVector3 targetPoint;

	public Vector3 Value
	{
		get { return this.currPoint; }
	}

	public PointSmoothing()
	{
		isInitializated = false;

		accumulatedLerp = 0.0f;
		initPoint = new Vector3(float.NaN, float.NaN, float.NaN);
		currPoint = new Vector3(float.NaN, float.NaN, float.NaN);
		targetPoint = new AdaptiveDoubleExponentialFilterVector3();
	}

	/// <summary>
	/// Applies point filtering using a combination of linear interpolation
	/// and exponential filtering. Lerp is used to avoid motion leaps when 
	/// no frame data is available, while the exponential filter reduces 
	/// motion noise whenever a new body data frame is generated.
	/// </summary>
	public void ApplyFilter(Vector3 newPoint, bool isNewFrame, DateTime frameTime)
	{
		if (isInitializated) 
		{
			accumulatedLerp += (float)(DateTime.Now - frameTime).TotalMilliseconds / lerpTime;
			currPoint = Vector3.Lerp(initPoint, targetPoint.Value, accumulatedLerp);
		}  
		else { currPoint = newPoint; }

		if (isNewFrame) 
		{
			isInitializated = true;
			accumulatedLerp = 0.0f;
		
			initPoint = currPoint;
			targetPoint.Value = newPoint;
		}
	}
}