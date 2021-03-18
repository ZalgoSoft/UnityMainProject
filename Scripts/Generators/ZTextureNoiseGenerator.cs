using UnityEditor;
using UnityEngine;
namespace Zalgo
{
    [RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
    public class ZTextureNoiseGenerator : MonoBehaviour
    {
        public int pixHeight = 64;
        public int pixWidth = 64;
        public float ShiftX;
        public float ShiftY;
        public float Scale = 8.0f;
        private Texture2D noiseTexture;
        private Color[] color;
        private Renderer myRenderer;
        private MeshFilter myMeshFilter;
        public bool AutoUpdate = true;
        void Start()
        {
            CheckTargetComponent();
            DrawNoise();
        }
        public void DrawNoise()
        {
            float y = 0.0F;
            while (y < noiseTexture.height)
            {
                float x = 0.0F;
                while (x < noiseTexture.width)
                {
                    float xCoord = ShiftX + x / noiseTexture.width * Scale;
                    float yCoord = ShiftY + y / noiseTexture.height * Scale;
                    float sample = Mathf.PerlinNoise(xCoord, yCoord);
                    color[(int)y * noiseTexture.width + (int)x] = new Color(sample, sample, sample);
                    x++;
                }
                y++;
            }
            noiseTexture.SetPixels(color);
            noiseTexture.Apply();
        }
        private static Mesh BuildQuad(float width, float height)
        {
            Mesh mesh = new Mesh();
            Vector3[] newVertices = new Vector3[4];
            float halfHeight = height * 0.5f;
            float halfWidth = width * 0.5f;
            newVertices[0] = new Vector3(-halfWidth, -halfHeight, 0);
            newVertices[1] = new Vector3(-halfWidth, halfHeight, 0);
            newVertices[2] = new Vector3(halfWidth, -halfHeight, 0);
            newVertices[3] = new Vector3(halfWidth, halfHeight, 0);
            Vector2[] newUVs = new Vector2[newVertices.Length];
            newUVs[0] = new Vector2(0, 0);
            newUVs[1] = new Vector2(0, 1);
            newUVs[2] = new Vector2(1, 0);
            newUVs[3] = new Vector2(1, 1);
            int[] newTriangles = new int[] { 0, 1, 2, 3, 2, 1 };
            Vector3[] newNormals = new Vector3[newVertices.Length];
            for (int i = 0; i < newNormals.Length; i++)
            {
                newNormals[i] = Vector3.forward;
            }
            mesh.vertices = newVertices;
            mesh.uv = newUVs;
            mesh.triangles = newTriangles;
            mesh.normals = newNormals;
            mesh.Optimize();
            return mesh;
        }
        public void CheckTargetComponent()
        {
            if (GetComponent<MeshRenderer>() == null)
                gameObject.AddComponent<MeshRenderer>();
            myRenderer = GetComponent<MeshRenderer>();
            if (GetComponent<MeshFilter>() == null)
                gameObject.AddComponent<MeshFilter>();
            myMeshFilter = GetComponent<MeshFilter>();
            if (myMeshFilter.sharedMesh == null ||
                myMeshFilter.sharedMesh.vertices[0].x != -pixWidth * 0.5f ||
                myMeshFilter.sharedMesh.vertices[0].y != -pixHeight * 0.5f
                )
            {
                Mesh mesh = BuildQuad(pixWidth, pixHeight);
                mesh.name = "Generated Quad";
                myMeshFilter.sharedMesh = mesh;
            }
            if (myRenderer.sharedMaterial == null)
                myRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
            if (color == null || color.Length == 0 || color.Length != pixWidth * pixHeight)
                color = new Color[pixWidth * pixHeight];
            noiseTexture = new Texture2D(pixWidth, pixHeight);
            myRenderer.sharedMaterial.mainTexture = noiseTexture;
        }
    }
    [CustomEditor(typeof(ZTextureNoiseGenerator))]
    class ZTextureNoiseGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector())
            { }
            if (GUILayout.Button("Generate") || ((ZTextureNoiseGenerator)target).AutoUpdate)
            {
                ((ZTextureNoiseGenerator)target).CheckTargetComponent();
                ((ZTextureNoiseGenerator)target).DrawNoise();
            }
        }
    }
}