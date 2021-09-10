using UnityEngine;

public static class Blocks
{
    private static Block Create(string name) => new Block(name);
    
    public static readonly Block Air = Create("air")
        .SetRenderMode(state => BlockRenderMode.Invisible);

    public static readonly Block Dirt = Create("dirt")
        .SetTexture((side, variant) => Textures.Dirt);

    public static readonly Block GrassBlock = Create("grass_block")
        .SetTexture((state, side) => side switch
        {
            4 => Textures.GrassTop,
            _ => Textures.Dirt
        })
        .SetTextureColor((state, side) => side switch
        {
            4 => new Color32(0, 255, 0, 255),
            _ => Color.white
        })
        .SetDefaultState(baseState => baseState.With(BlockStateProperties.Snowy, false));
}