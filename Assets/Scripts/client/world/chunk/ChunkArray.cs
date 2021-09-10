using System;
using System.Threading;

public class ChunkArray
{
    private readonly Chunk[] _chunks;
    private readonly int _viewDistance;
    private readonly int _sideLength;
    private volatile int _centerX;
    private volatile int _centerZ;
    private int _loaded;

    public ChunkArray(int viewDistance)
    {
        _viewDistance = viewDistance;
        _sideLength = viewDistance * 2 + 1;
        _chunks = new Chunk[_sideLength * _sideLength];
    }

    public void SetCenter(int x, int z)
    {
        _centerX = x;
        _centerZ = z;
    }
        
    public bool Contains(int x, int z) {
        return Math.Abs(x - _centerX) <= _viewDistance && Math.Abs(z - _centerZ) <= _viewDistance;
    }

    public Chunk Get(int x, int z)
    {
        var chunk = _chunks[GetIndex(x, z)];
        return IsValid(chunk, x, z) ? chunk : null;
    }
        
    public Chunk Set(int x, int z, Chunk chunk)
    {
        return Interlocked.Exchange(ref _chunks[GetIndex(x, z)], chunk);
    }

    private int GetIndex(int x, int z)
    {
        return CraftMath.FloorMod(z, _sideLength) * _sideLength + CraftMath.FloorMod(x, _sideLength);
    }

    private static bool IsValid(Chunk chunk, int x, int z)
    {
        return chunk != null && chunk.Position.x == x && chunk.Position.z == z;
    }
}