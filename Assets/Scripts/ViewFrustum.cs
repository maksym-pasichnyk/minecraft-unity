public class ViewFrustum
{
    public readonly int CountChunksX;
    public readonly int CountChunksY;
    public readonly int CountChunksZ;
    
    private ChunkRender[,,] _renderChunks;
    
    public ViewFrustum(int renderDistance)
    {
        CountChunksX = renderDistance * 2 + 1;
        CountChunksY = 16;
        CountChunksZ = renderDistance * 2 + 1;
    
        _renderChunks = new ChunkRender[CountChunksX, CountChunksY, CountChunksZ];
        for (int x = 0; x < CountChunksX; x++)
        {
            for (int z = 0; z < CountChunksX; z++)
            {
                for (int y = 0; y < CountChunksY; y++)
                {
                    _renderChunks[x, y, z] = new ChunkRender();
                    _renderChunks[x, y, z].SetPosition(x * 16, y * 16, z * 16);
                }
            }
        }
    }
    
    public void MarkForRerender(int sectionX, int sectionY, int sectionZ, bool rerenderOnMainThread) {
        int x = CraftMath.FloorMod(sectionX, CountChunksX);
        int y = CraftMath.FloorMod(sectionY, CountChunksY);
        int z = CraftMath.FloorMod(sectionZ, CountChunksZ);
        
        _renderChunks[x, y, z].SetNeedsUpdate(rerenderOnMainThread);
    }

    public ChunkRender GetRenderChunk(int x, int y, int z)
    {
        return _renderChunks[x, y, z];
    }
}
