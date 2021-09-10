// using System.Threading.Tasks;
//
// public class ChunkHolder
// {
//     public static readonly Task<Chunk> MissingChunkFuture = Task.FromResult<Chunk>(null);
//     public enum ChunkLoadingError
//     {
//         UNLOADED
//     }
//     
//     public ChunkPos Position;
//     private readonly Task<Chunk>[] _tasks = new Task<Chunk>[13];
//     // private Task<Chunk> _resultTask = Task.FromResult<>(null);
//
//     public ChunkHolder(ChunkPos position)
//     {
//         Position = position;
//     }
//
//     public Task<Chunk> GetCompletableFuture(ChunkStatus status)
//     {
//         var task = _tasks[status.Ordinal];
//         return task ?? MissingChunkFuture;
//     }
//     
//     public Task<Chunk> GetChunk(ChunkManager manager, ChunkStatus status)
//     {
//         if (_tasks[status.Ordinal] != null)
//         {
//             return _tasks[status.Ordinal];
//         }
//
//         var task = manager.GetChunk(this, status);
//         _tasks[status.Ordinal] = task;
//         // Chain(task);
//         return task;
//     }
//
//     // private void Chain(Task<Chunk> task)
//     // {
//     //     _resultTask = _resultTask.ContinueWith((result) => task.Result);
//     // }
// }
