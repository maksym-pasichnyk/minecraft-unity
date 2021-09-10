using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer
{
    private Client _client;
    private ClientWorld _world;
    private Camera _camera;
    private Transform _cameraTransform;
    private ViewFrustum _frustum;
    private ChunkRenderDispatcher _chunkRenderDispatcher;
    private Plane[] _planes = new Plane[6];
    private Queue<ChunkRender> _chunksToUpdate = new Queue<ChunkRender>();
    private List<ChunkRender> _renderInfos = new List<ChunkRender>(69696); // 66x66 chunks
    private int _viewDistance;

    private Vector2Int _lastPlayerPosition = new Vector2Int(1000, 1000);

    public WorldRenderer(Client client, int viewDistance)
    {
        _client = client;
        _viewDistance = viewDistance;
    }

    public void SetWorldAndLoadRenderers(ClientWorld world)
    {
        _world = world;
        
        _camera = _client.Camera;
        _cameraTransform = _camera.transform;
        _frustum = new ViewFrustum(_viewDistance);
        _chunkRenderDispatcher = new ChunkRenderDispatcher(world);
    }

    public void MarkSurroundingsForRender(int sectionX, int sectionY, int sectionZ)
    {
        for (int z = sectionZ - 1; z <= sectionZ + 1; ++z)
        {
            for (int x = sectionX - 1; x <= sectionX + 1; ++x)
            {
                for (int y = sectionY - 1; y <= sectionY + 1; ++y)
                {
                    MarkForRender(x, y, z);
                }
            }
        }
    }

    private void MarkForRender(int sectionX, int sectionY, int sectionZ, bool immediate = false) {
        _frustum.MarkForRender(sectionX, sectionY, sectionZ, immediate);
    }

    private void UpdateCameraAndRender(Vector3 position)
    {
        SetupTerrain();
        
        UpdateChunks(position);
    }

    private void SetupTerrain()
    {
        
    }
    
    private void UpdateChunks(Vector3 position)
    {
        _chunkRenderDispatcher.RunChunkUploads();
        
        GeometryUtility.CalculateFrustumPlanes(_camera, _planes);
        
        _lastPlayerPosition = new Vector2Int((int) Mathf.Floor(position.x / 16.0f), (int) Mathf.Floor(position.z / 16.0f));   
        for (int x = _lastPlayerPosition.x - _viewDistance; x <= _lastPlayerPosition.x + _viewDistance; ++x)
        {
            for (int z = _lastPlayerPosition.y - _viewDistance; z <= _lastPlayerPosition.y + _viewDistance; ++z)
            {
                for (int y = 0; y < 16; ++y)
                {
                    var min = new Vector3(x << 4, y << 4, z << 4);
                    var max = min = new Vector3(16, 16, 16);

                    var bounds = new Bounds()
                    {
                        min = min,
                        max = max
                    };
                
                    if (!GeometryUtility.TestPlanesAABB(_planes, bounds))
                    {
                        continue;
                    }
                
                    var renderChunk = _frustum.GetRenderChunk(x, y, z);
                    if (renderChunk.NeedsUpdate) {
                        renderChunk.ClearNeedsUpdate();
                    
                        int dx = _lastPlayerPosition.x - x;
                        int dz = _lastPlayerPosition.y - z;
                        bool immediate = false;//(dx * dx + dz * dz) <= 1;
                    
                        renderChunk.RebuildChunk(_chunkRenderDispatcher, x, y, z, immediate);
                    }
                
                    _renderInfos.Add(renderChunk);
                }
            }
        }
    }

    public void RenderWorld(Vector3 position)
    {
        UpdateCameraAndRender(position);

        foreach (ChunkRender chunkRender in _renderInfos)
        {
            Graphics.DrawMesh(chunkRender.Mesh, Matrix4x4.identity, _client.Material, 0);
        }
        
        _renderInfos.Clear();
    }
}
