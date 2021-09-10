public class ClientWorld
{
    private WorldRenderer _worldRenderer;
    private ClientChunkProvider _provider;
    public ClientChunkProvider ChunkProvider => _provider;
    
    public ClientWorld(WorldRenderer worldRenderer, int viewDistance)
    {
        _worldRenderer = worldRenderer;
        _provider = new ClientChunkProvider(this, viewDistance);
    }

    public Chunk GetChunk(int x, int z)
    {
        return _provider.GetChunk(x, z);
    }

    public void MarkSurroundingsForRender(int sectionX, int sectionY, int sectionZ)
    {
        _worldRenderer.MarkSurroundingsForRender(sectionX, sectionY, sectionZ);
    }
}
