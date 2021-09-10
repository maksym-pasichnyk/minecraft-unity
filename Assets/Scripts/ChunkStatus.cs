using System;
using System.Threading.Tasks;
using UnityEngine;

public class ChunkStatus
{
    // public delegate Task<Chunk> LoadingWorker(ChunkStatus status, ServerWorld serverWorld, ChunkGenerator generator, Func<Chunk, Task<Chunk>> func, Chunk chunk);
    // public delegate Task<Chunk> GenerationWorker(ChunkStatus status, ServerWorld serverWorld, ChunkGenerator generator, Func<Chunk, Task<Chunk>> func, Chunk[] chunks, Chunk chunk);
    // public delegate void SelectiveWorker(ServerWorld serverWorld, ChunkGenerator generator, Chunk[] chunks, Chunk chunk);
    //
    // private static readonly LoadingWorker NoopLoadingWorker = (status, world, generator, func, chunk) => Task.FromResult(chunk);
    //
    // public static readonly ChunkStatus EMPTY = RegisterSelective("empty", null, -1, 
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("EMPTY");
    //     });
    // public static readonly ChunkStatus STRUCTURE_STARTS = Register("structure_starts", EMPTY, 0,
    //     (status, world, generator, func, chunks, chunk) =>
    //     {
    //         // Debug.LogError("STRUCTURE_STARTS");
    //         
    //         return Task.FromResult(chunk);
    //     });
    // public static readonly ChunkStatus STRUCTURE_REFERENCES = RegisterSelective("structure_references", STRUCTURE_STARTS, /*8*/0,
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("STRUCTURE_REFERENCES");
    //     });
    // public static readonly ChunkStatus BIOMES = RegisterSelective("biomes", STRUCTURE_REFERENCES, 0,
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("BIOMES");
    //     });
    // public static readonly ChunkStatus NOISE = RegisterSelective("noise", BIOMES, /*8*/0,
    //     (world, generator, chunks, chunk) =>
    //     {
    //         generator.GenerateNoise(chunk);
    //     });
    // public static readonly ChunkStatus SURFACE = RegisterSelective("surface", NOISE, 0, 
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("SURFACE");
    //     });
    // public static readonly ChunkStatus CARVERS = RegisterSelective("carvers", SURFACE, 0, 
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("CARVERS");
    //     });
    // public static readonly ChunkStatus LIQUID_CARVERS = RegisterSelective("liquid_carvers", CARVERS, 0,
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("LIQUID_CARVERS");
    //     });
    // public static readonly ChunkStatus FEATURES = Register("features", LIQUID_CARVERS, /*8*/0,
    //     (status, world, generator, func, chunks, chunk) =>
    //     {
    //         // Debug.LogError("FEATURES");
    //         return Task.FromResult(chunk);
    //     });
    // public static readonly ChunkStatus LIGHT = Register("light", FEATURES, /*1*/0, 
    //     (status, world, generator, func, chunks, chunk) =>
    //     {
    //         // Debug.LogError("LIGHT");
    //         return Task.FromResult(chunk);
    //     });
    // public static readonly ChunkStatus SPAWN = RegisterSelective("spawn", LIGHT, 0, 
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("SPAWN");
    //     });
    // public static readonly ChunkStatus HEIGHTMAPS = RegisterSelective("heightmaps", SPAWN, 0, 
    //     (world, generator, chunks, chunk) =>
    //     {
    //         // Debug.LogError("HEIGHTMAPS");
    //     });
    // public static readonly ChunkStatus FULL = Register("full", HEIGHTMAPS, 0, (status, world, generator, func, chunks, chunk) =>
    // {
    //     // Debug.LogError("FULL");
    //     return func(chunk);
    // });
    //
    // private string _name;
    // private ChunkStatus _parent;
    // private int _range;
    // private int _ordinal;
    // private GenerationWorker _generationWorker;
    // private LoadingWorker _loadingWorker;
    //
    // public string Name => _name;
    // public ChunkStatus Parent => _parent;
    // public int Range => _range;
    // public int Ordinal => _ordinal;
    //
    // public ChunkStatus(string name, ChunkStatus parent, int range, GenerationWorker generationWorker, LoadingWorker loadingWorker)
    // {
    //     _name = name;
    //     _parent = parent ?? this;
    //     _range = range;
    //     _ordinal = parent?.Ordinal + 1 ?? 0;
    //     _generationWorker = generationWorker;
    //     _loadingWorker = loadingWorker;
    // }
    //
    // public static ChunkStatus RegisterSelective(string name, ChunkStatus parent, int range, SelectiveWorker selective)
    // {
    //     return Register(name, parent, range, (status, world, generator, func, chunks, chunk) =>
    //     {
    //         selective(world, generator, chunks, chunk);
    //         return Task.FromResult(chunk);
    //     });
    // }
    //
    // public static ChunkStatus Register(string name, ChunkStatus parent, int range, GenerationWorker generationWorker)
    // {
    //     return Register(name, parent, range, generationWorker, NoopLoadingWorker);
    // }
    //
    // public static ChunkStatus Register(string name, ChunkStatus parent, int range, GenerationWorker generationWorker, LoadingWorker loadingWorker)
    // {
    //     return new ChunkStatus(name, parent, range, generationWorker, loadingWorker);
    // }
    //
    // public Task<Chunk> DoGenerationWork(ServerWorld serverWorld, ChunkGenerator generator, Func<Chunk, Task<Chunk>> func, Chunk[] chunks)
    // {
    //     return _generationWorker(this, serverWorld, generator, func, chunks, chunks[chunks.Length / 2]);
    // }
    //
    // public Task<Chunk> DoLoadingWork(ServerWorld serverWorld, ChunkGenerator generator, Func<Chunk, Task<Chunk>> func, Chunk chunk)
    // {
    //     return _loadingWorker(this, serverWorld, generator, func, chunk);
    // }
}
