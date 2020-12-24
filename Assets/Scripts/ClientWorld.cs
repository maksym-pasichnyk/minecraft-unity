public class ClientWorld
{
    private WorldRenderer _worldRenderer;
    private ClientChunkProvider _provider;
    public ClientChunkProvider ChunkProvider => _provider;
    
    public ClientWorld(WorldRenderer worldRenderer)
    {
        _worldRenderer = worldRenderer;
        _provider = new ClientChunkProvider(this, viewDistance: 3);
    }

    public Chunk GetChunk(int x, int z)
    {
        return _provider.GetChunk(x, z);
    }

    public void MarkSurroundingsForRerender(int sectionX, int sectionY, int sectionZ)
    {
        _worldRenderer.MarkSurroundingsForRerender(sectionX, sectionY, sectionZ);
    }
}
