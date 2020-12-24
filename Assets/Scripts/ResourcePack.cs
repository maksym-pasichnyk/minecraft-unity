using System.IO;
using UnityEngine;

public class ResourcePack : IResourcePack
{
    private string _path;

    public ResourcePack(string path)
    {
        _path = path;
    }

    public byte[] LoadResource(string path)
    {
        return Client.Instance.Platform.ReadAssetFile(Path.Combine(_path, path));
    }

    public Texture2D LoadTexture(string path)
    {
        return Client.Instance.Platform.ReadPNG(Path.Combine(_path, path));
    }

    public string[] LoadResources(string path)
    {
        return Directory.GetFiles(Path.Combine(_path, path));
    }
    
    public string[] LoadResources(string path, string format)
    {
        return Directory.GetFiles(Path.Combine(_path, path), format);
    }
}
