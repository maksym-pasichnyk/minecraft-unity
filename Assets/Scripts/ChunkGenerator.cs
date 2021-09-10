using UnityEngine;

public class ChunkGenerator
{
    // public int TerrainHeight = 30;
    // public int BaseTerrainHeight = 60;
    //
    // public void GenerateNoise(Chunk chunk)
    // {
    //     int chunkPositionX = (chunk.Position.x << 4);
    //     int chunkPositionZ = (chunk.Position.z << 4);
    //
    //     for (int x = 0; x < 16; x++)
    //     {
    //         for (int z = 0; z < 16; z++)
    //         {
    //             int xPosition = chunkPositionX + x;
    //             int zPosition = chunkPositionZ + z;
    //
    //             float noise = GetNoiseHeight(xPosition, zPosition);
    //             int height = (int) (noise * TerrainHeight + BaseTerrainHeight);
    //             
    //             // Debug.Log($"Height = {height}");
    //
    //             for (int y = 0; y < height - 1; y++)
    //             {
    //                 chunk.SetBlockState(x, y, z, Blocks.Dirt.GetDefaultState());
    //             }
    //             
    //             chunk.SetBlockState(x, height - 1, z, Blocks.Grass.GetDefaultState());
    //         }
    //     }
    // }
    //
    // public float GetNoiseHeight(int xPosition, int zPosition)
    // {
    //     float fx = xPosition / 16f;
    //     float fz = zPosition / 16f;
    //
    //     float noise = Mathf.PerlinNoise(fx, fz);
    //     return noise;
    // }
}
