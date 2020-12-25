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

    public static Client Instance => _instance;
    public ClientWorld World => _world;
    public Camera Camera => _camera;
    public Material Material => _material;
    public AppPlatform Platform => _platform;

    public TextureAtlas TerrainTexture => _atlas;
    public ScreenManager ScreenManager => _screenManager;

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
    
    private void RunGameLoop(bool renderWorld)
    {
        if (renderWorld)
        {
            _worldRenderer.RenderWorld();
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
        
        _worldRenderer = new WorldRenderer(this);
        _world = new ClientWorld(_worldRenderer);
        
        _worldRenderer.SetWorldAndLoadRenderers(_world);
        _server = MinecraftServer.CreateServer(thread => new IntegratedServer(thread));
    }

    private void OnApplicationQuit()
    {
        _server?.StopServer();
    }
}
