using System;

public class ServerWorld
{
    private readonly ServerChunkProvider _provider;

    public ServerWorld()
    {
        _provider = new ServerChunkProvider(this);
    }
    
    public Chunk GetChunk(int x, int z)
    {
        return GetChunk(x, z, ChunkStatus.FULL, true);
    }
    //
    // public Chunk GetChunk(int chunkX, int chunkZ, ChunkStatus status)
    // {
    //     return GetChunk(chunkX, chunkZ, status, true);
    // }

    public Chunk GetChunk(int x, int z, ChunkStatus status, bool nonnull)
    {
        var chunk = _provider.GetChunk(x, z, status, nonnull);
        if (chunk == null && nonnull)
        {
            throw new Exception("Should always be able to create a chunk!");
        }

        return chunk;
    }
        
    // public Chunk GetChunk(ChunkPos position, ChunkStatus status)
    // {
    //     return _provider.GetChunkHolder(position).GetChunk(_manager, status).Result;
    // }
}
