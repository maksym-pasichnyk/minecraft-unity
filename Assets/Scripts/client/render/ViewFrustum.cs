using System.Collections.Generic;
using UnityEngine;

public class ViewFrustum
{
    private readonly int _countChunksX;
    private readonly int _countChunksY;
    private readonly int _countChunksZ;
    
    private ChunkRender[] _chunks;

    public ViewFrustum(int viewDistance)
    {
        _countChunksX = viewDistance * 2 + 1;
        _countChunksY = 16;
        _countChunksZ = viewDistance * 2 + 1;

        _chunks = new ChunkRender[_countChunksX * _countChunksY * _countChunksZ];
        for (int x = 0; x < _countChunksX; x++)
        {
            for (int y = 0; y < _countChunksY; y++)
            {
                for (int z = 0; z < _countChunksZ; z++)
                {
                    _chunks[GetIndex(x, y, z)] = new ChunkRender();
                }
            }
        }

        long[] longs = new long[_countChunksX * _countChunksY * _countChunksZ];

        for (int x = 0; x < _countChunksX; x++)
        {
            for (int y = 0; y < _countChunksY; y++)
            {
                for (int z = 0; z < _countChunksZ; z++)
                {
                    longs[GetIndex(x, y, z)] = GetIndex(x, y, z);
                }
            }
        }
        
        for (int x = 100; x < 100 + _countChunksX; x++)
        {
            for (int y = 0; y < _countChunksY; y++)
            {
                for (int z = -100; z < -100 + _countChunksZ; z++)
                {
                    if (longs[GetIndex(x, y, z)] != GetIndex(x, y, z))
                    {
                        Debug.LogError("Panic!");
                    }
                }
            }
        }
    }

    public void MarkForRender(int x, int y, int z, bool immediate) {
        _chunks[GetIndex(x, y, z)].SetNeedsUpdate(immediate);
    }

    public ChunkRender GetRenderChunk(int x, int y, int z)
    {
        return _chunks[GetIndex(x, y, z)];
    }
    
    private int GetIndex(int x, int y, int z)
    {
        return CraftMath.FloorMod(x, _countChunksX) * _countChunksY * _countChunksZ
             + CraftMath.FloorMod(y, _countChunksY) * _countChunksZ
             + CraftMath.FloorMod(z, _countChunksZ);
    }
}
