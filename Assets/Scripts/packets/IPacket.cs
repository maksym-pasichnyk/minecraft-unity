using System.IO;

public interface IPacket
{
    void Send(BinaryWriter writer);
    void Read(BinaryReader reader);
    void Action();
}