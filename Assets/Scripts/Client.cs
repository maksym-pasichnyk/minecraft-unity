using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class LanServerFinder : IDisposable
{
    private readonly UdpClient _socket = new UdpClient();
    private readonly Thread _thread;
    private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource(); 

    public LanServerFinder()
    {
        _socket.EnableBroadcast = true;
        _socket.ExclusiveAddressUse = false;
        _socket.Client.ReceiveTimeout = 5000;
        _socket.Client.ReceiveBufferSize = 1024;
        _socket.Client.Bind(new IPEndPoint(IPAddress.Any, 4445));
        _socket.JoinMulticastGroup(IPAddress.Parse("224.0.2.60"));
        
        _thread = new Thread(() => Start(_tokenSource.Token));
        _thread.Start();
    }

    private void Start(CancellationToken token)
    {
        var sender = new IPEndPoint(IPAddress.Any, 0);
        
        while (!token.IsCancellationRequested)
        {
            var bytes = _socket.Receive(ref sender);
            Debug.Log("Receive from: " + sender.Address);
            Thread.Sleep(1000);
        }
        
        _socket.Close();
        _socket.Dispose();
    }

    public void Stop()
    {
        _tokenSource.Cancel();
        _thread.Join();
        
        _socket.Close();
    }

    public void Dispose()
    {
        Stop();
        
        _tokenSource.Dispose();
        _socket.Dispose();
    }
}

public class Client : MonoBehaviour
{
    // private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    [SerializeField]
    private Camera _camera;
    
    [SerializeField]
    private Material _material;
    
    private static Client _instance;
    private AppPlatform _platform;
    private WorldRenderer _worldRenderer;
    private ClientWorld _world;
    private IntegratedServer _server;
    private ResourcePack _resourcePack;
    private TextureAtlas _atlas;

    public static Client Instance => _instance;
    public ClientWorld World => _world;
    public Camera Camera => _camera;
    public Material Material => _material;
    public AppPlatform Platform => _platform;

    public TextureAtlas TerrainTexture => _atlas;
    
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private ResourceManager _resources;

    private LanServerFinder _lanServerFinder;

    private void Start() {
        _camera.transform.position = new Vector3(0, 100, 0);
        
        // create game components
        
        _instance = this;
        _platform = new AppPlatform();
        
        // load resource packs
        
        _resourcePack = new ResourcePack(Path.Combine(AppPlatform.StreamingAssetsPath, "resource_packs/vanilla"));

        _atlas = new TextureAtlas();
        _atlas.LoadFromResources(_resourcePack);

        _material.SetTexture(MainTex, _atlas.Texture);
        
        // create world
        
        _worldRenderer = new WorldRenderer(this);
        _world = new ClientWorld(_worldRenderer);
        
        _worldRenderer.SetWorldAndLoadRenderers(_world);
        _server = MinecraftServer.CreateServer(thread => new IntegratedServer(thread));

        _resources = new ResourceManager();
        _resources.AddResourcePack(_resourcePack);

        _lanServerFinder = new LanServerFinder();
    }
    
    private void RunGameLoop(bool renderWorld)
    {
        if (renderWorld)
        {
            _worldRenderer.RenderWorld();
        }
    }

    private void Update()
    {
        RunGameLoop(true);
    }

    private void OnApplicationQuit()
    {
        _lanServerFinder.Stop();
        _server.StopServer();
    }
}
