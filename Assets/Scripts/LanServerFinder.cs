using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class LanServerFinder
{
    private readonly UdpClient _socket = new UdpClient
    {
        EnableBroadcast = true,
        ExclusiveAddressUse = false,
        Client =
        {
            Blocking = false, 
            ReceiveBufferSize = 1024
        }
    };
    
    private readonly Thread _thisThread;
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
    private bool _running = true;

    private readonly LanServerList _lanServerList;

    public LanServerFinder(LanServerList lanServerList)
    {
        _lanServerList = lanServerList;
        
        _socket.Client.Bind(new IPEndPoint(IPAddress.Any, 4445));
        _socket.JoinMulticastGroup(IPAddress.Parse("224.0.2.60"));

        _thisThread = new Thread(Start);
        _thisThread.Start();
        
        Debug.Log("Start LanServerFinder");
    }

    private void Start()
    {
        IPEndPoint sender = null;
        
        while (_running)
        {
            if (_socket.Available != 0)
            {
                var bytes = _socket.Receive(ref sender);
                var response = Encoding.ASCII.GetString(bytes);
                _lanServerList.AddServer(response, sender.Address);
            }
            _autoResetEvent.WaitOne(TimeSpan.FromSeconds(1));
        }
        _socket.Close();
    }

    public void Stop()
    {
        _running = false;
        _autoResetEvent.Set();
        _thisThread.Join();
        
        Debug.Log("Stop LanServerFinder");
    }
}