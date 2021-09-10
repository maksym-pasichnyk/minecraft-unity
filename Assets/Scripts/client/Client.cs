using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Client : MonoBehaviour
{
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
    [SerializeField] private Camera _camera;
    [SerializeField] private ScreenManager _screenManager;
    [SerializeField] private Material _material;
    
    private static Client _instance;
    private AppPlatform _platform;
    private WorldRenderer _worldRenderer;
    private ClientWorld _world;
    private IntegratedServer _server;
    private ResourcePack _resourcePack;
    private TextureAtlas _atlas;
    private ResourceManager _resources;
    private bool _renderWorld = false;
    private readonly FreeCamera _freeCamera = new FreeCamera();

    private readonly int viewDistance = 16;

    public static Client Instance => _instance;
    public ClientWorld World => _world;
    public Camera Camera => _camera;
    public Material Material => _material;
    public AppPlatform Platform => _platform;

    public TextureAtlas TerrainTexture => _atlas;
    public ScreenManager ScreenManager => _screenManager;

    private void OnEnable()
    {
        _freeCamera.OnRotateCamera += OnRotateCamera;
        _freeCamera.OnTranslateCamera += OnTranslateCamera;
    }
    
    private void OnDisable()
    {
        _freeCamera.OnRotateCamera -= OnRotateCamera;
        _freeCamera.OnTranslateCamera -= OnTranslateCamera;
    }

    private void Start() {
        _camera.transform.position = new Vector3(0, 100, 0);
        
        _instance = this;
        _platform = new AppPlatform();
        
        _resources = new ResourceManager();
        _resourcePack = new ResourcePack(Path.Combine(AppPlatform.StreamingAssetsPath, "resource_packs/vanilla"));
        _resources.AddResourcePack(_resourcePack);
        
        _atlas = new TextureAtlas();
        _atlas.LoadFromResources(_resourcePack);

        _material.SetTexture(MainTex, _atlas.Texture);

        _screenManager.ShowMainMenu();
    }

    private void SetPlayerTracking(Vector2Int chunkPos, bool track)
    {
        for (int chunk_x = chunkPos.x - viewDistance; chunk_x <= chunkPos.x + viewDistance; chunk_x++) {
            for (int chunk_z = chunkPos.y - viewDistance; chunk_z <= chunkPos.y + viewDistance; chunk_z++) {
                SetChunkLoadedAtClient(chunk_x, chunk_z, !track, track);
            }
        }
    }
    
    private void OnRotateCamera(Vector2 delta)
    {
        var cameraTransform = _camera.transform;
        
        var eulerAngles = cameraTransform.eulerAngles;
        cameraTransform.eulerAngles = new Vector3(eulerAngles.x + delta.x, eulerAngles.y + delta.y, 0);
    }
    
    private static int GetChunkDistance(Vector2Int pos, int x, int z) {
        return Math.Max(Math.Abs(pos.x - x), Math.Abs(pos.y - z));
    }
    
    private static int GetChunkDistance(Vector2Int from, Vector2Int to) {
        return Math.Max(Math.Abs(from.x - to.x), Math.Abs(from.y - to.y));
    }

    private readonly Dictionary<long, Chunk> _chunks = new Dictionary<long, Chunk>();

    private void OnTranslateCamera(Vector3 velocity)
    {
        var cameraTransform = _camera.transform;
        
        var prevPosition = cameraTransform.position;
        var oldChunkPos = new Vector2Int((int) Mathf.Floor(prevPosition.x / 16.0f), (int) Mathf.Floor(prevPosition.z / 16.0f));

        cameraTransform.Translate(velocity);

        var position = cameraTransform.position;
        var newChunkPos = new Vector2Int((int) Mathf.Floor(position.x / 16.0f), (int) Mathf.Floor(position.z / 16.0f));

        if (oldChunkPos != newChunkPos)
        {
            _world.ChunkProvider.SetCenter(newChunkPos.x, newChunkPos.y);
            
            int xStart = Math.Min(newChunkPos.x, oldChunkPos.x) - viewDistance;
            int zStart = Math.Min(newChunkPos.y, oldChunkPos.y) - viewDistance;
            int xEnd = Math.Max(newChunkPos.x, oldChunkPos.x) + viewDistance;
            int zEnd = Math.Max(newChunkPos.y, oldChunkPos.y) + viewDistance;

            for (int x = xStart; x <= xEnd; x++) {
                for (int z = zStart; z <= zEnd; z++) {
                    bool wasLoaded = GetChunkDistance(oldChunkPos, x, z) <= viewDistance;
                    bool needLoad = GetChunkDistance(newChunkPos, x, z) <= viewDistance;
                    
                    SetChunkLoadedAtClient(x, z, wasLoaded, needLoad);
                }
            }
        }
    }

    private void SetChunkLoadedAtClient(int x, int z, bool wasLoaded, bool needLoad)
    {
        var positionHash = ChunkPos.AsLong(x, z);
                    
        if (wasLoaded && !needLoad) {
            // _world.ChunkProvider.HandleChunkData(chunk);
            // world->server->connection->sendPacket(SUnloadChunkPacket{chunk_x, chunk_z});
        } 
        else if (needLoad && !wasLoaded)
        {
            if (!_chunks.TryGetValue(positionHash, out var chunk))
            {
                chunk = new Chunk(new ChunkPos(x, z));
                _chunks.Add(positionHash, chunk);
                
                GenerateChunk(chunk);
            }
                        
            _world.ChunkProvider.HandleChunkData(chunk);
        }
    }

    private void GenerateChunk(Chunk chunk)
    {
        var grass = Blocks.GrassBlock.GetDefaultState();
        
        for (int x = 0; x < 16; ++x)
        {
            for (int z = 0; z < 16; ++z)
            {
                chunk.SetBlockState(x, 0, z, grass);
            } 
        }
    }
    
    private void RunGameLoop(bool renderWorld)
    {
        _freeCamera.Update(Time.deltaTime);
        
        if (renderWorld)
        {
            _worldRenderer.RenderWorld(_camera.transform.position);
        }
    }

    private void Update()
    {
        RunGameLoop(_renderWorld);
    }

    public void LoadWorld()
    {
        _screenManager.ShowGameMenu();
        
        _renderWorld = true;
        
        _worldRenderer = new WorldRenderer(this, viewDistance);
        _world = new ClientWorld(_worldRenderer, viewDistance);
        
        _worldRenderer.SetWorldAndLoadRenderers(_world);
        _server = MinecraftServer.CreateServer(thread => new IntegratedServer(thread));
        
        var cameraPosition = _camera.transform.position;
        var chunkPos = new Vector2Int((int) Mathf.Floor(cameraPosition.x / 16.0f), (int) Mathf.Floor(cameraPosition.z / 16.0f));

        SetPlayerTracking(chunkPos, true);
    }

    private void OnApplicationQuit()
    {
        _server?.StopServer();
    }
}
