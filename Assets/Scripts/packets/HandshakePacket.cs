using UnityEngine;
using System.IO;

public struct HandshakePacket : IPacket
{
    public string UsernameHost;

    public void Send(BinaryWriter writer)
    {
        writer.Write(0x02);
        writer.Write(UsernameHost);
    }

    public void Read(BinaryReader reader)
    {        
        var hash = reader.ReadString();
        Debug.Log("Got connection hash: " + hash);              
    }

    public void Action()
    {
        
    }
}