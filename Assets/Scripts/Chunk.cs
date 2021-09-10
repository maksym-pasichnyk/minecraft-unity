using System;
using UnityEngine;

public class ChunkSection
{
    public readonly BlockState[,,] States = new BlockState[16, 16, 16];
    private int _blockRefCount = 0;

    public int BlockRefCount => _blockRefCount;
    
    public bool IsEmpty() => _blockRefCount == 0;


    public void SetTile(int x, int y, int z, BlockState state)
    {
        try
        {
            if (States[x, y, z] == BlockStates.Air)
            {
                _blockRefCount--;
            }

            States[x, y, z] = state;

            if (States[x, y, z] != BlockStates.Air)
            {
                _blockRefCount++;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public BlockState GetBlockState(int x, int y, int z)
    {
        return States[x, y, z] ?? BlockStates.Air;
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

    public void SetBlockState(int x, int y, int z, BlockState state)
    {
        try
        {
            var section = Sections[y >> 4] ?? (Sections[y >> 4] = new ChunkSection());

            section.States[x & 15, y & 15, z & 15] = state;
        }
        catch (Exception e)
        {
            Debug.LogError(e);            
        }
    }

    public BlockState GetBlockState(int x, int y, int z)
    {
        if (y < 0 || y > 255)
        {
            return BlockStates.Air;
        }

        var section = Sections[y >> 4];
        return section != null
            ? section.GetBlockState(x & 15, y & 15, z & 15)
            : BlockStates.Air;
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
