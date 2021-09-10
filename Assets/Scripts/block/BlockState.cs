using UnityEngine;

public class BlockState
{
    private Block _block;
    // private List<(IBlockStateProperty, object)> _properties = new List<(IBlockStateProperty, object)>();

    public BlockState(Block block)
    {
        _block = block;
    }

    public BlockState With<T>(BlockStateProperty<T> property, T value)
    {
        return this;
        // return Registries.States.GetValues().First(state => state._properties == _properties && state._properties[property].Equals(value));
    }

    public Block GetBlock() => _block;

    public bool IsOpaqueSide(Direction direction) => this != BlockStates.Air;
    
    
    public BlockRenderMode GetRenderMode() => _block.GetRenderMode(this);

    public Color32 GetTextureColor(int side) => _block.GetTextureColor(this, side);
    public TextureAtlasSprite GetTextureVariant(int side) => _block.GetTexture(this, side);
    
}