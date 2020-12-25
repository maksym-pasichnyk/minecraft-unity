using System.Threading;
using UnityEngine;

public class IntegratedServer : MinecraftServer
{
    private readonly Thread _thisThread;
    private ServerWorld _serverWorld;
    private bool _running = true;

    public IntegratedServer(Thread thisThread)
    {
        _thisThread = thisThread;
    }

    private void CreateWorld()
    {
        _serverWorld = new ServerWorld();
        CreateSpawnPosition();
    }

    private void CreateSpawnPosition()
    {
        for (int x = -3; x <= 3; x++)
        {
            for (int z = -3; z <= 3; z++)
            {
                _serverWorld.GetChunk(x, z);
            }
        }
    }

    private void CreateChunks(int x, int y)
    {
        
    }

    public override void StartServer()
    {
        Debug.Log("Start integrated server");
        CreateWorld();
        
        while (_running)
        {
            Update();
        }
    }
    
    public override void StopServer()
    {
        _running = false;
        _thisThread.Join();
        
        Debug.Log("Stop integrated server");
    }
    
    private void Update()
    {
        // for (int x = 0; x < _frustum.CountChunksX; x++)
        // {
        //     for (int z = 0; z < _frustum.CountChunksX; z++)
        //     {
        //         for (int y = 0; y < _frustum.CountChunksY; y++)
        //         {
        //             var renderChunk = _frustum.GetRenderChunk(x, y, z);
        //
        //             // if (GeometryUtility.TestPlanesAABB(_planes, renderChunk.Bounds))
        //             {
        //                 if (renderChunk.NeedsUpdate) {
        //                     renderChunk.RebuildChunk(_chunkRenderDispatcher, _serverWorld);
        //                     renderChunk.ClearNeedsUpdate();
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}
