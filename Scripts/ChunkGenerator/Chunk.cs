using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    Mesh cMesh;
    //public Material mmaterial;
    protected MeshCollider meshCollider;
    protected MeshRenderer meshRenderer;
    protected MeshFilter meshFilter;
    public byte[,,] map;
    public static int n = 0;
    public static int scale { get { return World.activeWorld.noiseScale; } }
    public static int chunkWidth { get { return World.activeWorld.chunkWidth; } }
    public static int chunkHeight { get { return World.activeWorld.chunkHeight; } }
    public static List<Chunk> chunks = new List<Chunk>();
    void Start()
    {
        chunks.Add(this);
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        map = new byte[chunkWidth, chunkHeight, chunkWidth];
        Random.InitState(World.activeWorld.randomSeed);
        Vector3 offset = new Vector3(Random.value * 10000, Random.value * 10000, Random.value * 10000);
        for (int x = 0; x < chunkWidth; x++)
        {
            float perlinX = Mathf.Abs((float)(x + transform.position.x + offset.x) / scale);
            for (int y = 0; y < chunkHeight; y++)
            {
                float perlinY = Mathf.Abs((float)(y + transform.position.y + offset.y) / scale);
                for (int z = 0; z < chunkWidth; z++)
                {
                    float perlinZ = Mathf.Abs((float)(z + transform.position.z + offset.z) / scale);
                    float perlin = Noise.Generate(perlinX, perlinY, perlinZ);
                    perlin += (10f - (float)y) / 10;
                    if (perlin > 0.05f)
                        map[x, y, z] = 1;
                }
            }
        }
        StartCoroutine(GenerateMesh());
    }
    public virtual IEnumerator GenerateMesh()
    { //IEnumerator
        cMesh = new Mesh();
        cMesh.name = "GeneratedMesh" + n++;
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> UVs = new List<Vector2>();
        List<int> tris = new List<int>();
        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                for (int z = 0; z < chunkWidth; z++)
                {
                    if (map[x, y, z] == 0)  //0 - бока , 1 - топ, 2 - дно куба.
                        continue;
                    byte block = map[x, y, z];
                    // lewa sciana
                    if (Visible(x - 1, y, z))
                        GenerateFace(block, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, UVs, tris, 0);
                    // prawa sciana
                    if (Visible(x + 1, y, z))
                        GenerateFace(block, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, UVs, tris, 0);
                    // tyl
                    if (Visible(x, y, z - 1))
                        GenerateFace(block, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, UVs, tris, 0);
                    // przod
                    if (Visible(x, y, z + 1))
                        GenerateFace(block, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, UVs, tris, 0);
                    // podloga
                    if (Visible(x, y - 1, z))
                        GenerateFace(block, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, UVs, tris, 2);
                    // sufit
                    if (Visible(x, y + 1, z))
                        GenerateFace(block, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, UVs, tris, 1);
                }
            }
        }
        //cMesh.vertices = verts.ToArray();
        //cMesh.uv = UVs.ToArray();
        //cMesh.triangles = tris.ToArray();
        cMesh.SetVertices(verts);
        cMesh.SetUVs(0, UVs);
        cMesh.SetTriangles(tris, 0);
        cMesh.RecalculateBounds();
        cMesh.RecalculateNormals();
        cMesh.RecalculateTangents();
        cMesh.OptimizeIndexBuffers();
        cMesh.OptimizeReorderVertexBuffer();
        cMesh.Optimize();
        meshFilter.sharedMesh = cMesh;
        meshCollider.sharedMesh = cMesh;
        yield return new WaitForEndOfFrame();
    }
    public virtual void GenerateFace(byte block, Vector3 corner, Vector3 up, Vector3 right, bool rev, List<Vector3> verts, List<Vector2> uvs, List<int> tris, int face)
    {
        Vector2 uv_width = Vector2.zero;
        Vector2 uv_corner = Vector2.zero;
        int index = verts.Count;
        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);
        if (face == 0) // side
            uv_corner = new Vector2(0.0f, 0.0f);
        else if (face == 1) //top
            uv_corner = new Vector2(0.666f, 0f);
        else if (face == 2) //bottom
            uv_corner = new Vector2(0.333f, 0.333f);
        uv_width = new Vector2(0.333f, 0.333f);
        //uv_corner.x += (float)(block - 1) / 4;
        uvs.Add(uv_corner);
        uvs.Add(new Vector2(uv_corner.x, uv_corner.y + uv_width.y));
        uvs.Add(new Vector2(uv_corner.x + uv_width.x, uv_corner.y + uv_width.y));
        uvs.Add(new Vector2(uv_corner.x + uv_width.x, uv_corner.y));
        if (rev)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        }
        else
        {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }
    }
    public virtual bool Visible(int x, int y, int z)
    {
        byte block = GetBlock(x, y, z);
        switch (block)
        {
            default:
            case 0:
                return true;
            case 1:
                return false;
        }
    }
    public virtual byte GetBlock(int x, int y, int z)
    {
        if (
            (x >= chunkWidth) ||
            (y >= chunkHeight) ||
            (z >= chunkWidth) ||
            (x < 0) || (y < 0) || (z < 0)
            )
            return 0;
        return map[x, y, z];
    }
    public static Chunk GetChunk(Vector3 position)
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            Vector3 chunkPos = chunks[i].transform.position;
            if ((position.x < chunkPos.x) || (position.z < chunkPos.z) ||
                (position.x >= chunkPos.x + chunkWidth) || (position.z >= chunkPos.z + chunkWidth))
                continue;
            return chunks[i];
        }
        return null;
    }
}