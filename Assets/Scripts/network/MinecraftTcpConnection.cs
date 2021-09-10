using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Threading;

public class MinecraftTcpConnection
{
    private static readonly string serverIP = "localhost";
    private static readonly int serverPort = 25565;
    private TcpClient _tcp;
    private BinaryReader _reader;
    private BinaryWriter _writer;
    // private PacketMap packetMap = null;
    private bool stopReading = false;

    public MinecraftTcpConnection()
    {
        _tcp = new TcpClient();
        _tcp.Connect(serverIP, serverPort);
        // packetMap = new PacketMap();

        _reader = new BinaryReader(_tcp.GetStream());
        _writer = new BinaryWriter(_tcp.GetStream());

        Debug.Log("TCP Connection established to " + serverIP + ":" + serverPort);

        //new HandshakePacket(serverIP + ":" + serverPort).Send(_writer);

        // new PacketLogin().Send(_writer);
    }

    private void ReadPackets()
    {
        var stream = _tcp.GetStream();
        while (!stopReading && stream.CanRead && stream.DataAvailable)
        {
            var id = _reader.ReadByte();

            Debug.Log($"Packet: {id}");

            // var packet = packetMap.Get(id);
            // if (packet == null)
            // {
            //     stopReading = true;
            //     Debug.LogError("Stop reading packets! Packet not registered: 0x" + id.ToString("X"));
            // }                
            // else
            // {
            //     packet.Read(socketReader).Action(socketWriter);                
            // }
        }
    }

    public void Disconnect()
    {
        _tcp.Close();
    }
}
