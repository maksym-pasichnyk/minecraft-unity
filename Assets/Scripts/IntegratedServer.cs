using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class IntegratedServer : MinecraftServer
{
    private delegate void ServerTask();
    
    private Thread _thisThread;
    private ServerWorld _serverWorld;
    private Task _task = Task.CompletedTask;

    private ConcurrentQueue<ServerTask> _queue = new ConcurrentQueue<ServerTask>();
    
    private bool _running = true;

    private TcpListener _network;
    private TcpClient _client;

    // private TcpClient _tcp;
    
    public IntegratedServer(Thread thisThread)
    {
        _thisThread = thisThread;

        _network = new TcpListener(IPAddress.Any, 8080);
        _network.Start();
        
        _network.BeginAcceptTcpClient(NetworkCallback, null);

        // _tcp = new TcpClient();
    }

    private void NetworkCallback(IAsyncResult result)
    {
        _client = _network.EndAcceptTcpClient(result);
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
