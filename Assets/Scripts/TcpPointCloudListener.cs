using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using UnityEngine;

public class TcpPointCloudListener : MonoBehaviour {

	private TcpListener _serverSocket;
	private TcpClient _clientSocket;
	private Thread _connectionAcceptThread;
	private List<Thread> _clientThreads;

	private bool _shouldRun = true;

	private const int WINDOW_SIZE = 1002;
	public int TcpPort;

	private PointCloudSimple[] _clouds;
	private int _thisFrame;
	private int _currentCloud;
	List<Point3DRGB> _bufferPoints;
	List<Point3DRGB> _readyPoints;

	void Start () {
		_bufferPoints = new List<Point3DRGB> ();
		_readyPoints = new List<Point3DRGB> ();
		_clouds = new PointCloudSimple[2];
		GameObject a = Instantiate (Resources.Load ("PointCloud"), Vector3.zero, Quaternion.identity) as GameObject;
		GameObject b = Instantiate (Resources.Load ("PointCloud"), Vector3.zero, Quaternion.identity) as GameObject;
		_clouds [0] = a.GetComponent<PointCloudSimple> ();
		_clouds [1] = b.GetComponent<PointCloudSimple> ();
		_currentCloud = 0;
		_serverSocket = new TcpListener (System.Net.IPAddress.Any,TcpPort);
		_clientSocket = default(TcpClient);
		_clientThreads = new List<Thread> ();
		_serverSocket.Start ();
		_connectionAcceptThread = new Thread (
			new ThreadStart (AcceptConnections));
		_connectionAcceptThread.IsBackground = true;
		_connectionAcceptThread.Start ();

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

	void AcceptConnections ()
	{
		Debug.Log ("[TCP] waiting for clients:");
		while (_shouldRun) 
		{
			_clientSocket = _serverSocket.AcceptTcpClient ();
			Debug.Log("Accefsdfd");

			Thread t = new Thread(DealWithClient);
			//t.IsBackground = true;
			t.Start((object) _clientSocket);
			_clientThreads.Add(t);
		}
	}

	void DealWithClient (object client)
	{
		TcpClient tcpClient = (TcpClient) client;
		byte[] ack = new byte[1];
		Debug.Log ("[TCP] new client");
		byte[] rcvdata = new byte[(WINDOW_SIZE + 1)*sizeof(float)];
		NetworkStream stream = tcpClient.GetStream ();
		while (_shouldRun) 
		{
			int bytes = stream.Read(rcvdata,0,(WINDOW_SIZE + 1)*sizeof(float));
			_parsePointCloud(rcvdata,bytes);
			stream.Write(ack,0,1);
		}

	}

	void _parsePointCloud (byte[] byteData,int bytes)
	{
		float [] floatData = new float[bytes / 4];
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
			_connectionAcceptThread.Abort();
			_clientSocket.Close();
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
