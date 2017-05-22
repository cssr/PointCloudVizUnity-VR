using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

using UnityEngine;

public enum HandType
{
    Left,
    Right,
    Unknown
}

public class HandheldMessage
{

    private HandType _hand;
    public HandType Hand { get { return _hand; } }

    private bool _click;
    public bool Click { get { return _click; } }

    private Quaternion _rotation;
    public Quaternion Rotation { get { return _rotation; } }

    public HandheldMessage()
    {
        _click = false;
        _rotation = Quaternion.identity;
    }

    public void Update(string udpMessage)
    {
        string[] statements = udpMessage.Split('/');
        Quaternion newRotation = Quaternion.identity;

        foreach (string s in statements)
        {
            string[] tokens = s.Split('=');

            if (tokens[0] == "hand")
            {
                if (tokens[1] == "Right") _hand = HandType.Right;
                else if (tokens[1] == "Left") _hand = HandType.Left;
                else _hand = HandType.Unknown;
            }
            else if (tokens[0] == "click") _click = bool.Parse(tokens[1]);
            else if (tokens[0] == "r.x") newRotation.x = float.Parse(tokens[1].Replace(',', '.'));
            else if (tokens[0] == "r.y") newRotation.y = float.Parse(tokens[1].Replace(',', '.'));
            else if (tokens[0] == "r.z") newRotation.z = float.Parse(tokens[1].Replace(',', '.'));
            else if (tokens[0] == "r.w") newRotation.w = float.Parse(tokens[1].Replace(',', '.'));
        }
        _rotation = newRotation;
    }
}

public class UDPHandheldListener
{
    private UdpClient _client;
    private int _port;

    public HandheldMessage Message;
    private string _decryptKey;

    private bool _receiving;
    public bool Receiving { get { return _receiving; } }

    public UDPHandheldListener(int port, string decryptKey)
    {
        _receiving = false;
        Message = new HandheldMessage();
        _decryptKey = decryptKey;
        _port = port;
        _client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
        Debug.Log("[UDPHandheldListener] Listening at port " + port);

        try
        {
            _client.BeginReceive(new AsyncCallback(recv), null);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void recv(IAsyncResult res)
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, _port);
        byte[] received = _client.EndReceive(res, ref ep);

        _receiving = true;
        Message.Update(DataEncryptor.Decrypt(Encoding.UTF8.GetString(received), _decryptKey));

        _client.BeginReceive(new AsyncCallback(recv), null);
    }
}