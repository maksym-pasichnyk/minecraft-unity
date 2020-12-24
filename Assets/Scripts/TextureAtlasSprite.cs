using UnityEngine;

public struct TextureAtlasSprite
{
    private readonly string _name;
    private readonly Rect _rect;

    public string Name => _name;

    public TextureAtlasSprite(string name, Rect rect)
    {
        _name = name;
        _rect = rect;
    }

    public float GetInterpolatedU(float value)
    {
        return (1 - value) * _rect.xMin + value * _rect.xMax;
    }
    
    public float GetInterpolatedV(float value)
    {
        return (1 - value) * _rect.yMin + value * _rect.yMax;
    }
}