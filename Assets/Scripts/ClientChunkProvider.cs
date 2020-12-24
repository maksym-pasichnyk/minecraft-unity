using System;
using UnityEngine;

public class ClientChunkProvider
{
    public class ChunkArray
    {
        private readonly Chunk[,] _chunks;
        private readonly int _viewDistance;
        private readonly int _sideLength;
        private volatile int _centerX;
        private volatile int _centerZ;
        private int _loaded;

        public ChunkArray(int viewDistance)
        {
            _viewDistance = viewDistance;
            _sideLength = viewDistance * 2 + 1;
            _chunks = new Chunk[_sideLength, _sideLength];
        }

        public void SetCenter(int centerX, int centerZ)
        {
            _centerX = centerX;
            _centerZ = centerZ;
        }
        
        public bool Contains(int x, int z) {
            return Math.Abs(x - _centerX) <= _viewDistance && Math.Abs(z - _centerZ) <= _viewDistance;
        }

        public Chunk Get(int chunkX, int chunkZ)
        {
            int x = chunkX + _centerX;
            int z = chunkZ + _centerZ;

            return _chunks[CraftMath.FloorMod(x, _sideLength), CraftMath.FloorMod(z, _sideLength)];
        }
        
        public Chunk Set(int chunkX, int chunkZ, Chunk chunk)
        {
            int x = chunkX + _centerX;
            int z = chunkZ + _centerZ;
            _chunks[CraftMath.FloorMod(x, _sideLength), CraftMath.FloorMod(z, _sideLength)] = chunk;
            return chunk;
        }
    }

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
    public void HandleChunkData(Chunk chunk)
    {
        LoadChunk(chunk.Position.x, chunk.Position.z, chunk);

        for (int y = 0; y < 16; y++)
        {
            _world.MarkSurroundingsForRerender(chunk.Position.x, y, chunk.Position.z);
        }
    }
}
