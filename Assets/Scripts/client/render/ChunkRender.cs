using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public class ChunkRender
{
    public class CompiledChunk
    {
        public readonly BlockTessellator BlockTessellator = new BlockTessellator();
    }
    
    public enum ChunkTaskResult
    {
        SUCCESSFUL,
        CANCELLED
    }
    
    public abstract class ChunkRenderTask
    {
        protected readonly ChunkRenderDispatcher Dispatcher;
        protected readonly ChunkRender Chunk;
        protected readonly float DistanceSq;
        protected int Finished = 0;

        public ChunkRenderTask(ChunkRenderDispatcher dispatcher, ChunkRender chunk, float distanceSq)
        {
            Dispatcher = dispatcher;
            Chunk = chunk;
            DistanceSq = distanceSq;
        }

        public abstract Task<ChunkTaskResult> Execute();
        public abstract void Cancel();
    }

    public class RebuildTask : ChunkRenderTask
    {
        private ChunkRenderCache? _chunkRenderCache;

        public RebuildTask(ChunkRenderDispatcher dispatcher, ChunkRender chunk, float distanceSq, ChunkRenderCache? chunkRenderCache) : base(dispatcher, chunk, distanceSq)
        {
            _chunkRenderCache = chunkRenderCache;
        }

        public override Task<ChunkTaskResult> Execute()
        {
            if (_chunkRenderCache == null)
            {
                return Task.FromResult(ChunkTaskResult.CANCELLED);
            }
            
            if (Finished == 1)
            {
                return Task.FromResult(ChunkTaskResult.CANCELLED);
            }

            var compiledChunk = new CompiledChunk();

            Compile(ref compiledChunk);
            
            if (Finished == 1)
            {
                return Task.FromResult(ChunkTaskResult.CANCELLED);
            }
            
            var ret = new TaskCompletionSource<bool>();
            Dispatcher.UploadTasks.Enqueue(() =>
            {
                var tessellator = compiledChunk.BlockTessellator.Tessellator;

                var mesh = Chunk.Mesh;
                mesh.vertices = tessellator.Vertices.ToArray();
                mesh.colors32 = tessellator.Colors.ToArray();
                mesh.uv = tessellator.Coords.ToArray();

                mesh.subMeshCount = 1;
                mesh.SetTriangles(tessellator.Indices, 0);
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
                // mesh.RecalculateTangents();

                ret.SetResult(true);
            });
            ret.Task.Wait();

            // if (Finished == 1)
            // {
            //     return Task.FromResult(ChunkTaskResult.CANCELLED);
            // }
            
            Chunk._compiledChunk = compiledChunk;
            return Task.FromResult(ChunkTaskResult.SUCCESSFUL);
        }

        private void Compile(ref CompiledChunk compiledChunk)
        {
            Profiler.BeginSample("Compile");
            if (_chunkRenderCache.HasValue)
            {
                var startPos = _chunkRenderCache.Value.Position;
                var endPos = startPos + new Vector3Int(16, 16, 16);

                var blockTessellator = compiledChunk.BlockTessellator;
                
                for (int x = startPos.x; x < endPos.x; x++)
                {
                    for (int z = startPos.z; z < endPos.z; z++)
                    {
                        for (int y = startPos.y; y < endPos.y; y++)
                        {
                            var pos = new BlockPos(x, y, z);
                            var state = _chunkRenderCache.Value.GetBlockState(pos);

                            var mode = state.GetRenderMode();
                            
                            if (mode == BlockRenderMode.Default)
                            {
                                blockTessellator.RenderModelSmooth(_chunkRenderCache, pos, state);
                            } 
                            else if (mode == BlockRenderMode.Entity)
                            {
                                // todo: tile entity
                                blockTessellator.RenderModelSmooth(_chunkRenderCache, pos, state);
                            }
                        }
                    }
                }
            }
            Profiler.EndSample();
        }

        public override void Cancel()
        {
            _chunkRenderCache = null;
            
            if (Interlocked.CompareExchange(ref Finished, 1, 0) == 1)
            {
                Chunk.SetNeedsUpdate(false);
            }
        }
    }

    public class SortTransparencyTask : ChunkRenderTask
    {
        public SortTransparencyTask(ChunkRenderDispatcher dispatcher, ChunkRender chunk, float distanceSq) : base(dispatcher, chunk, distanceSq) {}

        public override Task<ChunkTaskResult> Execute()
        {
            return Task.FromResult(ChunkTaskResult.CANCELLED);
        }

        public override void Cancel()
        {
            Finished = 1;
        }
    }

    private Mesh _mesh = new Mesh();
    
    // private Vector3Int _position;
    // private AABB _bounds;
    // private Bounds _bounds = new Bounds
    // {
        // size = new Vector3(16, 16, 16)
    // };
    private bool _needsImmediateUpdate = false;
    private bool _needsUpdate = true;
    private CompiledChunk _compiledChunk = null;
    private RebuildTask _lastRebuildTask = null;
    private SortTransparencyTask _lastResortTransparencyTask = null;
    // private Matrix4x4 _matrix;
    
    public bool NeedsUpdate => _needsUpdate;
    // public Vector3Int Position => _position;
    // public Bounds Bounds => _bounds;
    public CompiledChunk Compiled => _compiledChunk;
    public Mesh Mesh => _mesh;

    // public Matrix4x4 Matrix => _matrix;

    // public void SetPosition(int x, int y, int z)
    // {
    //     if (_position.x == x && _position.y == y && _position.z == z) return;
    //
    //     // _matrix = Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.identity, Vector3.one);
    //     _matrix = Matrix4x4.Translate(new Vector3(x, y, z));
    //     
    //     StopCompileTask();
    //         
    //     _position.x = x;
    //     _position.y = y;
    //     _position.z = z;
    //     
    //     // _bounds = new AABB(x, y, z, 16, 16, 16);
    //     _bounds.center = new Vector3(x + 8, y + 8, z + 8);
    // }

    private void StopCompileTask()
    {
        StopTasks();
        _compiledChunk = null;
        _needsUpdate = true;
    }

    private void StopTasks()
    {
        if (_lastRebuildTask != null)
        {
            _lastRebuildTask?.Cancel();
            _lastRebuildTask = null;
        }

        if (_lastResortTransparencyTask != null)
        {
            _lastResortTransparencyTask.Cancel();
            _lastResortTransparencyTask = null;
        }
    }
    
    public void SetNeedsUpdate(bool immediate)
    {
        _needsImmediateUpdate = immediate || (_needsUpdate && _needsImmediateUpdate);
        _needsUpdate = true;
    }

    private RebuildTask MakeCompileTaskChunk(ChunkRenderDispatcher dispatcher, Vector3Int position)
    {
        _lastRebuildTask?.Cancel();
        var cache = ChunkRenderCache.GenerateCache(dispatcher.World, position, position - Vector3Int.one, position + new Vector3Int(16, 16, 16), 1);
        _lastRebuildTask = new RebuildTask(dispatcher, this, GetDistanceSq(), cache);
        return _lastRebuildTask;
    }

    private float GetDistanceSq()
    {
        // var dx = _bounds.center.x;// - activerenderinfo.getProjectedView().x;
        // var dy = _bounds.center.y;// - activerenderinfo.getProjectedView().y;
        // var dz = _bounds.center.z;// - activerenderinfo.getProjectedView().z;
        return 0;// dx * dx + dy * dy + dz * dz;
    }

    // public bool ResortTransparency()
    // {
    //     if (_lastResortTransparencyTask != null)
    //     {
    //         _lastResortTransparencyTask.Cancel();
    //     }
    //     // _lastResortTransparencyTask = new SortTransparencyTask(this, GetDistanceSq());
    //     // return true;
    //     return false;
    // }

    public void RebuildChunk(ChunkRenderDispatcher dispatcher, int x, int y, int z, bool immediate)
    {
        var task = MakeCompileTaskChunk(dispatcher, new Vector3Int(x << 4, y << 4, z << 4));
        if (immediate)
        {
            task.Execute();
        }
        else
        {
            Task.Run(() => task.Execute());
        }
    }

    public void ClearNeedsUpdate()
    {
        _needsUpdate = false;
    }
}
