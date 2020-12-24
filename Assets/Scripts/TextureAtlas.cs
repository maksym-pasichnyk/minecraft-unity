using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TextureAtlas
{
    private static TextureAtlasSprite _missing;
    public static TextureAtlasSprite MissingTextureSprite => _missing;

    private Texture2D _texture;
    private Dictionary<string, TextureAtlasSprite> _sprites;
    
    public Texture2D Texture => _texture;
    
    public void LoadFromResources(ResourcePack resourcePack)
    {
        var resources = resourcePack.LoadResources("textures/blocks", "*.png");
        var textures = resources
            .Select(path => Client.Instance.Platform.ReadPNG(path))
            .Append(Texture2D.whiteTexture)
            .ToArray();

        _texture = new Texture2D(2048, 2048, TextureFormat.RGBA32, 0, true)
        {
            filterMode = FilterMode.Point
        };

        _sprites = resources
            .Select(Path.GetFileName)
            .Append("missing")
            .Zip(_texture.PackTextures(textures, 0), (name, rect) => new TextureAtlasSprite(name, rect))
            .ToDictionary(image => image.Name, image => image);

        _missing = _sprites["missing"];
    }

    public TextureAtlasSprite GetTextureItem(string name)
    {
        return _sprites.TryGetValue(name, out var texture) ? texture : _missing;
    }
}
