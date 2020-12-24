using System.IO;
using UnityEngine;

public class AppPlatform
{
    public static string StreamingAssetsPath => Application.streamingAssetsPath;
    
    public byte[] ReadFile(string path)
    {
        return File.ReadAllBytes(path);
    }
    
    public byte[] ReadAssetFile(string path)
    {
        return File.ReadAllBytes(Path.Combine(StreamingAssetsPath, path));
    }
    
    public Texture2D ReadAssetPNG(string path)
    {
        var texture = new Texture2D(1, 1, TextureFormat.RGBA32, 0, true);
        texture.LoadImage(ReadAssetFile(path));
        return texture;
    }

    public Texture2D ReadPNG(string path)
    {
        var texture = new Texture2D(1, 1, TextureFormat.RGBA32, 0, true);
        return texture.LoadImage(ReadFile(path)) ? texture : null;
    }
}
