using System;
using UnityEngine;

public class ClientChunkProvider
{
    private ClientWorld _world;
    private ChunkArray _chunkArray;
    
    public ClientChunkProvider(ClientWorld world, int viewDistance)
    {
        _world = world;
        _chunkArray = new ChunkArray(AdjustViewDistance(viewDistance));
    }
    
    public void SetCenter(int centerX, int centerZ)
    {
        _chunkArray.SetCenter(centerX, centerZ);
    }

    public Chunk LoadChunk(int chunkX, int chunkZ, Chunk chunk /*todo: replace with NetworkPacket*/)
    {
        if (!_chunkArray.Contains(chunkX, chunkZ))
        {
            return null;
        }
        
        // Debug.Log("ReceiveChunk");
        
        return _chunkArray.Set(chunkX, chunkZ, chunk);
    }

    public Chunk GetChunk(int chunkX, int chunkZ)
    {
        if (!_chunkArray.Contains(chunkX, chunkZ))
        {
            return null;
        }
        
        return _chunkArray.Get(chunkX, chunkZ);
    }
    
    private static int AdjustViewDistance(int viewDistance) {
        return Math.Max(2, viewDistance) + 3;
    }

    // todo: NetworkHandler
    public void HandleChunkData(Chunk chunk) {
        LoadChunk(chunk.Position.x, chunk.Position.z, chunk);

        for (int y = 0; y < 16; y++)
        {
            _world.MarkSurroundingsForRender(chunk.Position.x, y, chunk.Position.z);
        }
    }
}
