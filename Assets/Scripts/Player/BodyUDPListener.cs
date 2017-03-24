using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

public class BodyUDPListener : MonoBehaviour 
{
	public static string NoneMessage = "0";

	public int port;
	private IPEndPoint ip;

    private UdpClient udpClient = null;
    private List<string> stringsToParse;

    void Start()
    { 
		UDPRestart();
	}

	void Update()
	{
		while (stringsToParse.Count > 0) 
		{
			string stringToParse = stringsToParse[stringsToParse.Count - 1];
			stringsToParse.Clear();

			List<Body> bodies = new List<Body>();

			if (stringToParse != null && stringToParse.Length != 1) 
			{
				int n = 0;

				foreach (string b in stringToParse.Split(MessageSeparators.L1)) 
				{
					if (n++ == 0) continue;
					if (b != NoneMessage) bodies.Add(new Body(b));
				}
			}
            //mudar para ficar generico
            if(gameObject.GetComponent<TrackerClientSimpleRobot>()!= null)
            {
                gameObject.GetComponent<TrackerClientSimpleRobot>().SetNewFrame(bodies.ToArray());
            }
		}
	}

	public void UDPRestart()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }

        stringsToParse = new List<string>();
		ip = new IPEndPoint(IPAddress.Any, port);

		udpClient = new UdpClient(ip);
        udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);

		UnityEngine.Debug.Log("[BodyUDPListener]: Receiving body data in port: " + port);
    }

    public void ReceiveCallback(IAsyncResult ar)
    {
        Byte[] receiveBytes = udpClient.EndReceive(ar, ref ip);
		stringsToParse.Add(Encoding.ASCII.GetString(receiveBytes));
		udpClient.BeginReceive(new AsyncCallback(this.ReceiveCallback), null);
    }

	void OnApplicationQuit()
    {
		if (udpClient != null) 
		{
			udpClient.Close();
		}
    }

    void OnQuit()
    {
        OnApplicationQuit();
    }
}