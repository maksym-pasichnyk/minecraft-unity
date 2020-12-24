using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkManager
{
    private Dictionary<long, ChunkHolder> _holders = new Dictionary<long, ChunkHolder>();

    private ServerWorld _serverWorld;
    private ChunkGenerator _generator;

    public ChunkManager(ServerWorld serverWorld, ChunkGenerator generator)
    {
        _serverWorld = serverWorld;
        _generator = generator;
    }
    
    public async Task<Chunk> GetChunk(ChunkHolder holder, ChunkStatus status)
    {
        var chunkPos = holder.Position;
        
        if (status == ChunkStatus.EMPTY)
        {
            return await LoadChunk(chunkPos);
        }

        await holder.GetChunk(this, status.Parent);
        
        return await Generate(holder, status);
    }

    public async Task<Chunk> MakeWorldChunk(ChunkHolder holder)
    {
        var chunk = await holder.GetCompletableFuture(ChunkStatus.FULL.Parent);

        if (chunk != null)
        {
            Client.Instance.World.ChunkProvider.HandleChunkData(chunk);
        }

        return chunk;
    }

    public Task<Chunk> LoadChunk(ChunkPos chunkPos)
    {
        return Task.Run(() =>
        {
            // Debug.Log("LoadChunk");
            return new Chunk(chunkPos);
        });
    }

    public async Task<Chunk> Generate(ChunkHolder holder, ChunkStatus status)
    {
        var chunks = await GetChunks(holder.Position, status.Range, () => status.Parent);
        
        if (chunks == null)
        {
            // Debug.Log($"ChunkStatus: {status.Name}");
            return null;// Task.FromResult<Chunk>(null);
        }

        return await status.DoGenerationWork(_serverWorld, _generator, _ => MakeWorldChunk(holder), chunks);
    }

    public Task<Chunk> GetTickingChunk(ChunkPos position)
    {
        var task = GetChunks(position, 1, () => ChunkStatus.FULL).ThenApplyAsync(chunks =>
        {
            if (chunks == null) return null;

            var chunk = chunks[chunks.Length / 2];
            chunk.PostProcess();
            return chunk;

        });
        task.ThenAcceptAsync(chunk =>
        {
            if (chunk == null) return;
            SendChunkData(chunk);
        });
        return task;
    }

    public Task<Chunk> GetEntityTickingChunk(ChunkPos position)
    {
        return GetChunks(position, 2, () => ChunkStatus.FULL).ThenApplyAsync(chunks => chunks?[chunks.Length / 2]);
    }

    public Task<Chunk[]> GetChunks(ChunkPos position, int radius, Func<ChunkStatus> func)
    {
        List<Task<Chunk>> tasks = new List<Task<Chunk>>();
        int xPosition = position.x;
        int zPosition = position.z;
        for (int x = -radius; x <= radius; ++x)
        {
            for (int z = -radius; z <= radius; ++z)
            {
                var holder = GetChunkHolder(x + xPosition, z + zPosition);
                if (holder == null)
                {
                    return Task.FromResult<Chunk[]>(null);
                }
                tasks.Add(holder.GetChunk(this, func()));
            }
        }

        return Task.WhenAll(tasks.ToArray());
    }

    public ChunkHolder GetChunkHolder(int x, int z)
    {
        _holders.TryGetValue(ChunkPos.AsLong(x, z), out var holder);
        return holder;
    }

    public ChunkHolder CreateChunkHolder(int x, int z)
    {
        var position = new ChunkPos(x, z);
        var positionHash = position.AsLong();
    
        var holder = new ChunkHolder(position);
        _holders.Add(positionHash, holder);
        return holder;
    }

    private void SendChunkData(Chunk chunk)
    {
    }
}
