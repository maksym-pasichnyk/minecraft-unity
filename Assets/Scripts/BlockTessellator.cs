using System;
using UnityEngine;

public enum Direction
{
    South,
    East,
    North,
    West,
    Up,
    Down
}

public class BlockTessellator
{
    private Tessellator _tessellator;
    private AABB _bounds;
    private float _minU;
    private float _minV;
    private float _maxU;
    private float _maxV;

    private int _rotSouth = 0;
    private int _rotEast = 0;
    private int _rotNorth = 0;
    private int _rotWest = 0;
    private int _rotTop = 0;
    private int _rotBottom = 0;

    public Tessellator Tessellator => _tessellator;
    
    public BlockTessellator()
    {
        _tessellator = new Tessellator();
    }

    public void RenderModelSmooth(in IBlockDisplayReader blocks, in Tile tile, in Vector3Int blockPos)
    {
        _bounds.Set(0, 0, 0, 1, 1, 1);
        
        if (blocks.GetTile(blockPos.x, blockPos.y, blockPos.z - 1) == Tile.Air)
        {
            AddQuad();
            BuildSouth(tile.GetTextureSide(0, 0), tile.GetColorSide(0, 0), blockPos);
        }
        if (blocks.GetTile(blockPos.x + 1, blockPos.y, blockPos.z) == Tile.Air)
        {
            AddQuad();
            BuildEast(tile.GetTextureSide(1, 0), tile.GetColorSide(1, 0), blockPos);
        }
        if (blocks.GetTile(blockPos.x, blockPos.y, blockPos.z + 1) == Tile.Air)
        {
            AddQuad();
            BuildNorth(tile.GetTextureSide(2, 0), tile.GetColorSide(2, 0), blockPos);
        }
        if (blocks.GetTile(blockPos.x - 1, blockPos.y, blockPos.z) == Tile.Air)
        {
            AddQuad();
            BuildWest(tile.GetTextureSide(3, 0), tile.GetColorSide(3, 0), blockPos);
        }
        if (blocks.GetTile(blockPos.x, blockPos.y + 1, blockPos.z) == Tile.Air)
        {
            AddQuad();
            BuildUp(tile.GetTextureSide(4, 0), tile.GetColorSide(4, 0), blockPos);
        }
        
        if (blocks.GetTile(blockPos.x, blockPos.y - 1, blockPos.z) == Tile.Air)
        {
            AddQuad();
            BuildDown(tile.GetTextureSide(5, 0), tile.GetColorSide(5, 0), blockPos);
        }
    }

    public void AddQuad() => _tessellator.Quad();
    public void AddInvQuad() => _tessellator.InvQuad();
    
    public void BuildSouth(in TextureAtlasSprite texture, in Color32 color, in Vector3Int blockPos)
    {
        _minU = 0;
        _minV = 0;
        _maxU = 1;
        _maxV = 1;
        
        var p1 = new Vector3(_bounds.minX, _bounds.minY, _bounds.minZ);
        var p2 = new Vector3(_bounds.minX, _bounds.maxY, _bounds.minZ);
        var p3 = new Vector3(_bounds.maxX, _bounds.maxY, _bounds.minZ);
        var p4 = new Vector3(_bounds.maxX, _bounds.minY, _bounds.minZ);
        
        BuildFace(texture, color, blockPos, p1, p2, p3, p4, _rotSouth, () => GetLightIntensivity(Direction.South));
    }
    
    public void BuildEast(in TextureAtlasSprite texture, in Color32 color, in Vector3Int blockPos)
    {
        _minU = 0;
        _minV = 0;
        _maxU = 1;
        _maxV = 1;
        
        var p1 = new Vector3(_bounds.maxX, _bounds.minY, _bounds.minZ);
        var p2 = new Vector3(_bounds.maxX, _bounds.maxY, _bounds.minZ);
        var p3 = new Vector3(_bounds.maxX, _bounds.maxY, _bounds.maxZ);
        var p4 = new Vector3(_bounds.maxX, _bounds.minY, _bounds.maxZ);
        
        BuildFace(texture, color, blockPos, p1, p2, p3, p4, _rotEast, () => GetLightIntensivity(Direction.East));
    }

    public void BuildNorth(in TextureAtlasSprite texture, in Color32 color, in Vector3Int blockPos)
    {
        _minU = 0;
        _minV = 0;
        _maxU = 1;
        _maxV = 1;

        var p1 = new Vector3(_bounds.maxX, _bounds.minY, _bounds.maxZ);
        var p2 = new Vector3(_bounds.maxX, _bounds.maxY, _bounds.maxZ);
        var p3 = new Vector3(_bounds.minX, _bounds.maxY, _bounds.maxZ);
        var p4 = new Vector3(_bounds.minX, _bounds.minY, _bounds.maxZ);
        
        BuildFace(texture, color, blockPos, p1, p2, p3, p4, _rotNorth, () => GetLightIntensivity(Direction.North));
    }
    
    public void BuildWest(in TextureAtlasSprite texture, in Color32 color, in Vector3Int blockPos)
    {
        _minU = 0;
        _minV = 0;
        _maxU = 1;
        _maxV = 1;
        
        var p1 = new Vector3(_bounds.minX, _bounds.minY, _bounds.maxZ);
        var p2 = new Vector3(_bounds.minX, _bounds.maxY, _bounds.maxZ);
        var p3 = new Vector3(_bounds.minX, _bounds.maxY, _bounds.minZ);
        var p4 = new Vector3(_bounds.minX, _bounds.minY, _bounds.minZ);
        
        BuildFace(texture, color, blockPos, p1, p2, p3, p4, _rotWest, () => GetLightIntensivity(Direction.West));
    }

    public void BuildUp(in TextureAtlasSprite texture, in Color32 color, in Vector3Int blockPos)
    {
        _minU = 0;
        _minV = 0;
        _maxU = 1;
        _maxV = 1;
        
        var p1 = new Vector3(_bounds.minX, _bounds.maxY, _bounds.minZ);
        var p2 = new Vector3(_bounds.minX, _bounds.maxY, _bounds.maxZ);
        var p3 = new Vector3(_bounds.maxX, _bounds.maxY, _bounds.maxZ);
        var p4 = new Vector3(_bounds.maxX, _bounds.maxY, _bounds.minZ);
        
        BuildFace(texture, color, blockPos, p1, p2, p3, p4, _rotTop, () => GetLightIntensivity(Direction.Up));
    }
    
    public void BuildDown(in TextureAtlasSprite texture, in Color32 color, in Vector3Int blockPos)
    {
        _minU = 0;
        _minV = 0;
        _maxU = 1;
        _maxV = 1;
        
        var p1 = new Vector3(_bounds.minX, _bounds.minY, _bounds.maxZ);
        var p2 = new Vector3(_bounds.minX, _bounds.minY, _bounds.minZ);
        var p3 = new Vector3(_bounds.maxX, _bounds.minY, _bounds.minZ);
        var p4 = new Vector3(_bounds.maxX, _bounds.minY, _bounds.maxZ);
        
        BuildFace(texture, color, blockPos, p1, p2, p3, p4, _rotBottom, () => GetLightIntensivity(Direction.Down));
    }
    
    public void BuildFace(in TextureAtlasSprite texture, in Color32 color, in Vector3Int blockPos, in Vector3 p1, in Vector3 p2, in Vector3 p3, in Vector3 p4, int texRot, in Func<(byte, byte, byte, byte)> getLightIntencivity)
    {
        var x = blockPos.x;
        var y = blockPos.y;
        var z = blockPos.z;

        var uv00 = new Vector2(_minU, _minV);
        var uv01 = new Vector2(_minU, _maxV);
        var uv11 = new Vector2(_maxU, _maxV);
        var uv10 = new Vector2(_maxU, _minV);
        
        for (var i = 0; i < texRot; i++)
        {
            uv00 = new Vector2(uv00.y, 1 - uv00.x);
            uv01 = new Vector2(uv01.y, 1 - uv01.x);
            uv11 = new Vector2(uv11.y, 1 - uv11.x);
            uv10 = new Vector2(uv10.y, 1 - uv10.x);
        }
        
        _tessellator.Color(color);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv00.x), texture.GetInterpolatedV(uv00.y));
        _tessellator.Vertex(x + p1.x, y + p1.y, z + p1.z);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv01.x), texture.GetInterpolatedV(uv01.y));
        _tessellator.Vertex(x + p2.x, y + p2.y, z + p2.z);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv11.x), texture.GetInterpolatedV(uv11.y));
        _tessellator.Vertex(x + p3.x, y + p3.y, z + p3.z);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv10.x), texture.GetInterpolatedV(uv10.y));
        _tessellator.Vertex(x + p4.x, y + p4.y, z + p4.z);
    }

    public (byte, byte, byte, byte) GetLightIntensivity(Direction direction)
    {
        return (255, 255, 255, 255);
    }
}
