using System.Collections.Generic;
using UnityEngine;

public class Tessellator
{
    private readonly List<Vector3> _vertices = new List<Vector3>();
    private readonly List<Color32> _colors = new List<Color32>();
    private readonly List<Vector2> _coords = new List<Vector2>();
    private readonly List<int> _indices = new List<int>();
    
    private Color32 _color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    private Vector2 _uv = new Vector2(0, 0);

    public List<Vector3> Vertices => _vertices;
    public List<Color32> Colors => _colors;
    public List<Vector2> Coords => _coords;
    public List<int> Indices => _indices;

    public void Color(Color32 color) => _color = color;
    public void Color(byte r, byte g, byte b, byte a) => _color = new Color32(r, g, b, a);

    public void Tex(float u, float v) => _uv = new Vector2(u, v);
    
    public void Vertex(float x, float y, float z)
    {
        _vertices.Add(new Vector3(x, y, z));
        _colors.Add(_color);
        _coords.Add(_uv);
    }
    
    public void Vertex(float x, float y, float z, float u, float v)
    {
        _vertices.Add(new Vector3(x, y, z));
        _colors.Add(_color);
        _coords.Add(new Vector2(u, v));
    }

    public void Quad() => Quad(0, 1, 2, 0, 2, 3);
    public void InvQuad() => Quad(0, 2, 1, 0, 3, 2);
    
    public void Quad(int a, int b, int c)
    {
        var startVertexIndex = _vertices.Count;
        _indices.Add(startVertexIndex + a);
        _indices.Add(startVertexIndex + b);
        _indices.Add(startVertexIndex + c);
    }
    
    public void Quad(int a1, int b1, int c1, int a2, int b2, int c2)
    {
        var startVertexIndex = _vertices.Count;
        _indices.Add(startVertexIndex + a1);
        _indices.Add(startVertexIndex + b1);
        _indices.Add(startVertexIndex + c1);
        _indices.Add(startVertexIndex + a2);
        _indices.Add(startVertexIndex + b2);
        _indices.Add(startVertexIndex + c2);
    }
}
