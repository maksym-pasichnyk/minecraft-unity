using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    private Vector2Int _lastPlayerPosition = new Vector2Int(1000, 1000);
    
    public WorldRenderer(Client client)
    {
        _client = client;
    }

    public void SetWorldAndLoadRenderers(ClientWorld world)
    {
        _world = world;
        
        _camera = _client.Camera;
        _cameraTransform = _camera.transform;
        _frustum = new ViewFrustum(renderDistance: 3);
        _chunkRenderDispatcher = new ChunkRenderDispatcher(world);
    }

    public void MarkSurroundingsForRerender(int sectionX, int sectionY, int sectionZ)
    {
        for (int z = sectionZ - 1; z <= sectionZ + 1; ++z)
        {
            for (int x = sectionX - 1; x <= sectionX + 1; ++x)
            {
                for (int y = sectionY - 1; y <= sectionY + 1; ++y)
                {
                    MarkForRerender(x, y, z);
                }
            }
        }
    }

    public void MarkForRerender(int sectionX, int sectionY, int sectionZ, bool rerenderOnMainThread = false) {
        _frustum.MarkForRerender(sectionX, sectionY, sectionZ, rerenderOnMainThread);
    }
    
    public void UpdateCameraAndRender()
    {
        SetupTerrain();
        
        UpdateChunks();
    }

    public void SetupTerrain()
    {
        
    }
    
    public void UpdateChunks()
    {
        _chunkRenderDispatcher.RunChunkUploads();
        
        GeometryUtility.CalculateFrustumPlanes(_camera, _planes);
        
        for (int x = 0; x < _frustum.CountChunksX; x++)
        {
            for (int z = 0; z < _frustum.CountChunksX; z++)
            {
                for (int y = 0; y < _frustum.CountChunksY; y++)
                {
                    var renderChunk = _frustum.GetRenderChunk(x, y, z);

                    if (!GeometryUtility.TestPlanesAABB(_planes, renderChunk.Bounds))
                    {
                        continue;
                    }
                    
                    if (renderChunk.NeedsUpdate) {
                        renderChunk.RebuildChunk(_chunkRenderDispatcher);
                        renderChunk.ClearNeedsUpdate();
                    }
                    
                    _renderInfos.Add(renderChunk);
                }
            }
        }
    }

    public void RenderWorld()
    {
        UpdateCameraAndRender();

        foreach (ChunkRender chunkRender in _renderInfos)
        {
            Graphics.DrawMesh(chunkRender.Mesh, Matrix4x4.identity, _client.Material, 0);
        }
        
        _renderInfos.Clear();
    }
}
