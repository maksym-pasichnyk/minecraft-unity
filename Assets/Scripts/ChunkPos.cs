public struct ChunkPos
{
    public static readonly long SENTINEL = AsLong(1875016, 1875016);
    
    public int x;
    public int z;

    public ChunkPos(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static long AsLong(int x, int z)
    {
        return x & 4294967295L | ((z & 4294967295L) << 32);
    }
    
    public long AsLong() {
        return AsLong(x, z);
    }
}
