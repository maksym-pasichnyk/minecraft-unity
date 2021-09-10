using System;
using UnityEngine;

public class Textures
{
    public static readonly TextureAtlasSprite Dirt = GetTextureItem("dirt.png");
    public static readonly TextureAtlasSprite GrassTop = GetTextureItem("grass_top.png");
    public static readonly TextureAtlasSprite GrassSide = GetTextureItem("grass_side.png");

    private static TextureAtlasSprite GetTextureItem(string name)
    {
        return Client.Instance.TerrainTexture.GetTextureItem(name);
    }
}

public enum BlockRenderMode
{
    Invisible,
    Default,
    Entity
}

public class Block
{
    private static readonly Color32 DefaultColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    private static readonly TextureAtlasSprite DefaultTexture = TextureAtlas.MissingTextureSprite;
    private static readonly BlockRenderMode DefaultRenderMode = BlockRenderMode.Default;
    
    private string _name;
    private Func<BlockState, int, Color32> _color = (state, side) => DefaultColor;
    private Func<BlockState, int, TextureAtlasSprite> _texture = (state, side) => DefaultTexture;
    private Func<BlockState, BlockRenderMode> _renderMode = state => DefaultRenderMode;

    private BlockState _baseState;
    private BlockState _defaultState;
    
    public Block(string name)
    {
        _name = name;
        _baseState = new BlockState(this);
        _defaultState = _baseState;
    }
    
    public string GetName() => _name;
    
    public BlockState GetDefaultState() => _defaultState;

    public BlockRenderMode GetRenderMode(BlockState state) => _renderMode(state);

    public Color32 GetTextureColor(BlockState state, int side) => _color(state, side);
    public TextureAtlasSprite GetTexture(BlockState state, int side) => _texture(state, side);
    
    public Block SetTextureColor(Func<BlockState, int, Color32> color)
    {
        _color = color ?? ((side, variant) => DefaultColor);
        return this;
    }
    
    public Block SetTexture(Func<BlockState, int, TextureAtlasSprite> func)
    {
        _texture = func ?? ((side, variant) => DefaultTexture);
        return this;
    }

    public Block SetDefaultState(Func<BlockState, BlockState> func)
    {
        _defaultState = func?.Invoke(_baseState) ?? _baseState;
        return this;
    }

    public Block SetRenderMode(Func<BlockState, BlockRenderMode> func)
    {
        _renderMode = func ?? (state => DefaultRenderMode);
        return this;
    }
}