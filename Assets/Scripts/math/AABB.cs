using UnityEngine;

public struct AABB
{
    public float minX;
    public float minY;
    public float minZ;
    public float maxX;
    public float maxY;
    public float maxZ;

    public AABB(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        minX = Mathf.Min(x1, x2);
        minY = Mathf.Min(y1, y2);
        minZ = Mathf.Min(z1, z2);
        maxX = Mathf.Max(x1, x2);
        maxY = Mathf.Max(y1, y2);
        maxZ = Mathf.Max(z1, z2);
    }

    public void Set(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        minX = Mathf.Min(x1, x2);
        minY = Mathf.Min(y1, y2);
        minZ = Mathf.Min(z1, z2);
        maxX = Mathf.Max(x1, x2);
        maxY = Mathf.Max(y1, y2);
        maxZ = Mathf.Max(z1, z2);
    }
}
