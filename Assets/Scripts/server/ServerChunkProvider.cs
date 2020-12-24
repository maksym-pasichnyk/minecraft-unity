using System;
using System.Threading;
using System.Threading.Tasks;

public class ServerChunkProvider
{
    private Thread _mainThread;
    private ServerWorld _world;
    private ChunkManager _manager;

    private long[] _recentPositions = new long[4];
    private ChunkStatus[] _recentStatuses = new ChunkStatus[4];
    private Chunk[] _recentChunks = new Chunk[4];

    public ServerChunkProvider(ServerWorld world)
    {
        _mainThread = Thread.CurrentThread;
        _world = world;
        _manager = new ChunkManager(_world, new ChunkGenerator());

        InvalidateCaches();
    }

    private void InvalidateCaches()
    {
        for (int i = 0; i < 4; i++)
        {
            _recentPositions[i] = ChunkPos.SENTINEL;
            _recentStatuses[i] = null;
            _recentChunks[i] = null;
        }
    }

    private void UpdateCache(long positionHash, Chunk chunk, ChunkStatus status)
    {
        for (int i = 3; i > 0; i--)
        {
            _recentPositions[i] = _recentPositions[i - 1];
            _recentStatuses[i] = _recentStatuses[i - 1];
            _recentChunks[i] = _recentChunks[i - 1];
        }

        _recentPositions[0] = positionHash;
        _recentStatuses[0] = status;
        _recentChunks[0] = chunk;
    }
    
    public Chunk GetChunk(int x, int z, ChunkStatus status, bool nonnull)
    {
        if (Thread.CurrentThread != _mainThread)
        {
            throw new Exception("Wrong Thread");
        }
        
        var positionHash = ChunkPos.AsLong(x, z);
        for (int i = 0; i < 4; i++)
        {
            if (_recentPositions[i] != positionHash || _recentStatuses[i] != status) continue;
            
            var recentChunk = _recentChunks[i];
            if (recentChunk != null || !nonnull) {
                return recentChunk;
            }
        }

        var task = ProvideChunk(x, z, status, nonnull);
        var chunk = task.GetAwaiter().GetResult();
        if (chunk == null && nonnull)
        {
            throw new Exception("Chunk not there when requested");
        }

        UpdateCache(positionHash, chunk, status);
        return chunk;
    }
    
    public ChunkHolder GetChunkHolder(int x, int z)
    {
        return _manager.GetChunkHolder(x, z);
    }

    public Task<Chunk> ProvideChunk(int x, int z, ChunkStatus status, bool nonnull)
    {
        var holder = GetChunkHolder(x, z);
        if (holder == null && nonnull)
        {
            holder = _manager.CreateChunkHolder(x, z);
        }

        return holder != null
            ? holder.GetChunk(_manager, status)
            : ChunkHolder.MissingChunkFuture;
    }
}
