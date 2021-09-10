using System;

public struct BlockPos
{
    public int x;
    public int y;
    public int z;
    
    public BlockPos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public BlockPos Move(Direction direction)
    {
        return direction switch
        {
            Direction.South => South(),
            Direction.East => East(),
            Direction.North => North(),
            Direction.West => West(),
            Direction.Up => Up(),
            Direction.Down => Down(),
            _ => this
        };
    }

    public BlockPos Up()
    {
        return new BlockPos(x, y + 1, z);
    }
    
    public BlockPos Down()
    {
        return new BlockPos(x, y - 1, z);
    }
    
    public BlockPos South()
    {
        return new BlockPos(x, y, z - 1);
    }
    
    public BlockPos North()
    {
        return new BlockPos(x, y, z + 1);
    }
    
    public BlockPos East()
    {
        return new BlockPos(x + 1, y, z);
    }
    
    public BlockPos West()
    {
        return new BlockPos(x - 1, y, z);
    }
}