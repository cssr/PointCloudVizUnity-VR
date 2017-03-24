using UnityEngine;
using System.Collections;

public class FilterProperties : MonoBehaviour 
{
	[Range(0, 1)]
	public float trendGain;

	[Range(0, 1)]
	public float gainLow;

	[Range(0, 1)]
	public float gainHigh;

	public float deltaLow;
	public float deltaHigh;

	public float lerpTime;

	// Update is called once per frame
	void Update() 
	{
		AdaptiveDoubleExponentialFilterFloat.GainLow = gainLow;
		AdaptiveDoubleExponentialFilterFloat.GainHigh = gainHigh;
		AdaptiveDoubleExponentialFilterFloat.DeltaLow = deltaLow;
		AdaptiveDoubleExponentialFilterFloat.DeltaHigh = deltaHigh;
		AdaptiveDoubleExponentialFilterFloat.TrendGain = trendGain;

		PointSmoothing.lerpTime = lerpTime;
	}
}