using System;
using UnityEngine;

public enum Direction
{
    South,
    East,
    North,
    West,
    Up,
    Down
}

public class BlockTessellator
{
    private static readonly Vector3[][] Positions = {
        new[]
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),
        },
        new []
        {
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1),
        },
        new[]
        {
            new Vector3(1, 0, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 0, 1),
        },
        new[]
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 1),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),
        },
        new[]
        {
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 0),
        },
        new[]
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
        }
    };

    private static readonly int[][] Textures = {
        new [] {0, 0, 1, 1},
        new [] {0, 0, 1, 1},
        new [] {0, 0, 1, 1},
        new [] {0, 0, 1, 1},
        new [] {0, 0, 1, 1},
        new [] {0, 0, 1, 1},
    };
    
    private readonly Tessellator _tessellator = new Tessellator();
    // private AABB _bounds;
    // private float _minU;
    // private float _minV;
    // private float _maxU;
    // private float _maxV;
    // private int _rotSouth = 0;
    // private int _rotEast = 0;
    // private int _rotNorth = 0;
    // private int _rotWest = 0;
    // private int _rotTop = 0;
    // private int _rotBottom = 0;

    public Tessellator Tessellator => _tessellator;
    

    private static bool ShouldRenderFace(in IBlockDisplayReader blocks, BlockState state, BlockPos blockPos, Direction direction)
    {
        return !blocks.GetBlockState(blockPos.Move(direction)).IsOpaqueSide(direction);
    }
    
    public void RenderModelSmooth(in IBlockDisplayReader blocks, BlockPos pos, BlockState state)
    {
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            if (!ShouldRenderFace(blocks, state, pos, direction))
            {
                continue;
            }
            
            AddQuad();
            BuildFace(pos, state, direction);
        }
    }

    private void BuildFace(BlockPos pos, BlockState state, Direction direction)
    {
        var minU = Textures[(int) direction][0];
        var minV = Textures[(int) direction][1];
        var maxU = Textures[(int) direction][2];
        var maxV = Textures[(int) direction][3];

        var uv00 = new Vector2(minU, minV);
        var uv01 = new Vector2(minU, maxV);
        var uv11 = new Vector2(maxU, maxV);
        var uv10 = new Vector2(maxU, minV);
        
        // for (var i = 0; i < texRot; i++)
        // {
        //     uv00 = new Vector2(uv00.y, 1 - uv00.x);
        //     uv01 = new Vector2(uv01.y, 1 - uv01.x);
        //     uv11 = new Vector2(uv11.y, 1 - uv11.x);
        //     uv10 = new Vector2(uv10.y, 1 - uv10.x);
        // }

        var p1 = Positions[(int) direction][0];
        var p2 = Positions[(int) direction][1];
        var p3 = Positions[(int) direction][2];
        var p4 = Positions[(int) direction][3];
        
        _tessellator.Color(state.GetTextureColor((int) direction));
            
        var texture = state.GetTextureVariant((int) direction);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv00.x), texture.GetInterpolatedV(uv00.y));
        _tessellator.Vertex(pos.x + p1.x, pos.y + p1.y, pos.z + p1.z);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv01.x), texture.GetInterpolatedV(uv01.y));
        _tessellator.Vertex(pos.x + p2.x, pos.y + p2.y, pos.z + p2.z);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv11.x), texture.GetInterpolatedV(uv11.y));
        _tessellator.Vertex(pos.x + p3.x, pos.y + p3.y, pos.z + p3.z);
        
        _tessellator.Tex(texture.GetInterpolatedU(uv10.x), texture.GetInterpolatedV(uv10.y));
        _tessellator.Vertex(pos.x + p4.x, pos.y + p4.y, pos.z + p4.z);
    }

    private void AddQuad() => _tessellator.Quad();
    private void AddInvQuad() => _tessellator.InvQuad();
}
