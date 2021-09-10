using System;
using System.Collections.Concurrent;

public class ChunkRenderDispatcher
{
    public delegate void UploadTask();
    
    public readonly ConcurrentQueue<UploadTask> UploadTasks = new ConcurrentQueue<UploadTask>();

    private ClientWorld _world;
    public ClientWorld World => _world;

    public ChunkRenderDispatcher(ClientWorld world)
    {
        _world = world;
    }
    
    public void RunChunkUploads()
    {
        if (UploadTasks.TryDequeue(out var action))
        {
            action();
        }
    }
}
