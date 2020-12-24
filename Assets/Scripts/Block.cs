using UnityEngine;

public abstract class Tile
{
    public static readonly AirTile Air = new AirTile();
    public static readonly DirtTile Dirt = new DirtTile();
    public static readonly GrassTile Grass = new GrassTile();

    public abstract TextureAtlasSprite GetTextureSide(int side, int variant);
    public abstract Color32 GetColorSide(int side, int variant);

    protected static TextureAtlasSprite GetTextureItem(string name)
    {
        return Client.Instance.TerrainTexture.GetTextureItem(name);
    }
}

public class AirTile : Tile
{
    public override TextureAtlasSprite GetTextureSide(int side, int variant)
    {
        return TextureAtlas.MissingTextureSprite;
    }

    public override Color32 GetColorSide(int side, int variant)
    {
        return new Color32(255, 255, 255, 255);
    }
}

public class DirtTile : Tile
{
    private readonly TextureAtlasSprite _dirt;
    
    public DirtTile()
    {
        _dirt = GetTextureItem("dirt.png");
    }
    
    public override TextureAtlasSprite GetTextureSide(int side, int variant)
    {
        return _dirt;
    }

    public override Color32 GetColorSide(int side, int variant)
    {
        return new Color32(255, 255, 255, 255);
    }
}

public class GrassTile : Tile
{
    private readonly TextureAtlasSprite _dirt;
    private readonly TextureAtlasSprite _grassTop;
    private readonly TextureAtlasSprite _grassSide;
    
    public GrassTile()
    {
        _dirt = GetTextureItem("dirt.png");
        _grassTop = GetTextureItem("grass_top.png");
        _grassSide = GetTextureItem("grass_side.png");
    }
    
    public override TextureAtlasSprite GetTextureSide(int side, int variant)
    {
        switch (side)
        {
            case 4:
                return _grassTop;
            default:
                return _dirt;
        }
    }

    public override Color32 GetColorSide(int side, int variant)
    {
        switch (side)
        {
            case 4: 
                return new Color32(0, 255, 0, 255);
            default: 
                return Color.white;
        }
    }
}