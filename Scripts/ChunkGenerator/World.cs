using UnityEngine;
public class World : MonoBehaviour
{
    public int chunkWidth = 64;
    public int chunkHeight = 16;
    public int randomSeed = 0;
    public float viewRange = 64;
    public int noiseScale = 32;
    public static World activeWorld;
    public Material chunkMaterial;
    static Chunk chunkPrefab;//= GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Chunk>();
    void Awake()
    {
        activeWorld = this;
        if (randomSeed == 0)
            randomSeed = Random.Range(0, int.MaxValue);
    }
    private void Start()
    {
        chunkPrefab = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<Chunk>();
        chunkPrefab.GetComponent<MeshRenderer>().sharedMaterial = chunkMaterial;
        //chunkPrefab.scale = chunkScale;
        Destroy(chunkPrefab.GetComponent<BoxCollider>());
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Chunk.chunks.ForEach(x => Gizmos.DrawWireMesh(x.GetComponent<MeshFilter>().sharedMesh, 0, x.transform.position));
    }
    void Update()
    {
        for (float x = transform.position.x - viewRange; x < transform.position.x + viewRange; x += chunkWidth)
        {
            for (float z = transform.position.z - viewRange; z < transform.position.z + viewRange; z += chunkWidth)
            {
                Vector3 pos = new Vector3(x, 0, z);
                pos.x = Mathf.Floor(pos.x / (float)chunkWidth) * chunkWidth;
                pos.z = Mathf.Floor(pos.z / (float)chunkWidth) * chunkWidth;
                Chunk chunk = Chunk.GetChunk(pos);
                if (chunk != null) continue;
                chunk = (Chunk)Instantiate(chunkPrefab, pos, Quaternion.identity);
            }
        }
    }
}