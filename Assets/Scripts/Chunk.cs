using System;
using UnityEngine;

public class ChunkSection
{
    public readonly Tile[,,] Tiles = new Tile[16, 16, 16];
    private int _blockRefCount = 0;

    public int BlockRefCount => _blockRefCount;
    
    public bool IsEmpty() => _blockRefCount == 0;


    public void SetTile(int x, int y, int z, Tile tile)
    {
        try
        {
            if (Tiles[x, y, z] == Tile.Air)
            {
                _blockRefCount--;
            }

            Tiles[x, y, z] = tile;

            if (Tiles[x, y, z] != Tile.Air)
            {
                _blockRefCount++;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public Tile GetTile(int x, int y, int z)
    {
        return Tiles[x, y, z] ?? Tile.Air;
    }
    
    public static bool IsEmpty(ChunkSection section)
    {
        return section == null || section.IsEmpty();
    }
}

public class Chunk
{
    public readonly ChunkPos Position;
    public readonly ChunkSection[] Sections = new ChunkSection[16];

    public Chunk(ChunkPos position)
    {
        Position = position;
    }

    public void PostProcess()
    {
    }

    public void SetTile(int x, int y, int z, Tile tile)
    {
        try
        {
            var section = Sections[y >> 4] ?? (Sections[y >> 4] = new ChunkSection());

            section.Tiles[x & 15, y & 15, z & 15] = tile;
        }
        catch (Exception e)
        {
            Debug.LogError(e);            
        }
    }

    public Tile GetTile(int x, int y, int z)
    {
        if (y < 0 || y > 255)
        {
            return Tile.Air;
        }

        var section = Sections[y >> 4];
        return section != null
            ? section.GetTile(x & 15, y & 15, z & 15)
            : Tile.Air;
    }

    public bool IsEmptyBetween(int startY, int endY) {
        if (startY < 0) {
            startY = 0;
        }

        if (endY >= 256) {
            endY = 255;
        }

        for(var i = startY; i <= endY; i += 16) {
            if (!ChunkSection.IsEmpty(Sections[i >> 4])) {
                return false;
            }
        }

        return true;
    }
}
