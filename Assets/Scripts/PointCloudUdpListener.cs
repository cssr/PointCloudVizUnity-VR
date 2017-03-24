using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Text;
using System.IO;

public class PointCloudUdpListener : MonoBehaviour {

	public int port;
	public PointCloudSimple clouda;
	public PointCloudSimple cloudb;

	private PointCloudSimple[] _clouds;
	private int _thisFrame;
	private int _currentCloud;
	List<Point3DRGB> _bufferPoints;
	List<Point3DRGB> _readyPoints;

	private Thread _receiveThread;
	private UdpClient _udpClient;

	private bool _shouldRun;

	void Start () {
		_shouldRun = true;
		_bufferPoints = new List<Point3DRGB> ();
		_receiveThread = new Thread (
			new ThreadStart (_receiveData));
		_receiveThread.IsBackground = true;
		_receiveThread.Start ();

		_clouds = new PointCloudSimple[2];
		_clouds [0] = clouda;
		_clouds [1] = cloudb;
		_currentCloud = 0;
	}

	void Update () {
		if (_readyPoints != null) {
			_clouds [_currentCloud].hideFromView ();
			_currentCloud = (_currentCloud + 1) % 2;
			_clouds [_currentCloud].setPoints (_readyPoints);
			_clouds [_currentCloud].setToView ();
			_readyPoints = null;
		}
	}

	void _receiveData ()
	{
		_udpClient = new UdpClient (port);
		IPEndPoint endpoint = new IPEndPoint (IPAddress.Any, 0);
		_bufferPoints = new List<Point3DRGB>();
		while (_shouldRun) 
		{
			try 
			{
				byte [] data = _udpClient.Receive(ref endpoint);
				_parsePointCloud(data);
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message + System.Environment.NewLine + e.StackTrace);
			}
		}
	}

	void _parsePointCloud (byte[] byteData)
	{
		float [] floatData = new float[byteData.Length / 4];
		Buffer.BlockCopy (byteData, 0, floatData, 0, byteData.Length);

		int frameIndex = (int)floatData [0];
		if (frameIndex != _thisFrame) {
			frameFinished();
			_thisFrame = frameIndex;
			_bufferPoints = new List<Point3DRGB>();
		}
		Debug.Log (frameIndex);

		for (int i = 1; i < floatData.Length;)
		{
			float x = floatData[i++];
			float y = floatData[i++];
			float z = floatData[i++];
			float r = floatData[i++]/255;
			float g = floatData[i++]/255;
			float b = floatData[i++]/255;
			Point3DRGB p = new Point3DRGB(new Vector3(x,y,z),new Color(r,g,b));
			_bufferPoints.Add(p);
		}
	}


	void frameFinished(){
		_readyPoints = _bufferPoints;
		_bufferPoints = null;

	}

	void OnApplicationQuit()
	{
		_shouldRun = false;
		try
		{
			_receiveThread.Abort();
			_udpClient.Close();
			Debug.Log("[UDP] socket closed");
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message + System.Environment.NewLine + e.StackTrace);
		}
	}
	
	void OnQuit()
	{
		OnApplicationQuit();
	}
}
