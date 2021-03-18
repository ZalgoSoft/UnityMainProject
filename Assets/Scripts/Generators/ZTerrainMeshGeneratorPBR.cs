using UnityEditor;
using UnityEngine;
namespace Zalgo
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ZTerrainMeshGeneratorPBR : MonoBehaviour
    {
        Vector3[] vertices;
        Color[] colors;
        Mesh mesh;
        int[] triangles;
        Vector2[] uvs;
        public Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        public int xSize = 64, ySize = 64, zSize = 64;
        public int textureWidth = 512;
        public int textureHeight = 512;
        public bool FirstWaveHarmonics = true;
        public float noise01Frequency = 2f;
        public float noise01Amp = 13f;
        public float Noise01ShiftX = 7;
        public float Noise01ShiftY = 11;
        public bool SecondWaveHarmonics = true;
        public float noise02Frequency = 11f;
        public float noise02Amp = 2f;
        public float Noise02ShiftX = 13;
        public float Noise02ShiftY = 17;
        public bool ThirdWaveHarmonics = true;
        public float noise03Frequency = 37f;
        public float noise03Amp = 0.5f;
        public float Noise03ShiftX = 41;
        public float Noise03ShiftY = 43;

        public bool AutoUpdate = true;
        bool isDirty = false;
        private void Reset()
        {
            gradient = new Gradient();
            colorKey = new GradientColorKey[6];
            colorKey[0].color = new Color(0f, 0.94f, 0.81f);
            colorKey[0].time = 0.0f;
            colorKey[1].color = new Color(0f, 0f, 1f);
            colorKey[1].time = 0.2f;
            colorKey[2].color = new Color(0f, 0.56f, 0.62f);
            colorKey[2].time = 0.33f;
            colorKey[3].color = new Color(0f, 0.94f, 0.11f);
            colorKey[3].time = 0.6f;
            colorKey[4].color = new Color(0.35f, 0.35f, 0.35f);
            colorKey[4].time = 0.75f;
            colorKey[5].color = new Color(1f, 1f, 1f);
            colorKey[5].time = 1.0f;
            alphaKey = new GradientAlphaKey[6];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;
            alphaKey[2].alpha = 1.0f;
            alphaKey[2].time = 1.0f;
            alphaKey[3].alpha = 1.0f;
            alphaKey[3].time = 1.0f;
            alphaKey[4].alpha = 1.0f;
            alphaKey[4].time = 1.0f;
            alphaKey[5].alpha = 1.0f;
            alphaKey[5].time = 1.0f;
            gradient.mode = GradientMode.Blend;
            gradient.SetKeys(colorKey, alphaKey);
            CreateTerrain();
        }
        float makeNoise(int x, int y)
        {
            float a = 0;
            float xCoord;
            float yCoord;
            if (FirstWaveHarmonics)
            {
                xCoord = Noise01ShiftX + (float)x / xSize * noise01Frequency;
                yCoord = Noise01ShiftY + (float)y / zSize * noise01Frequency;
                a = Mathf.PerlinNoise(xCoord, yCoord) * noise01Amp;
            }
            if (SecondWaveHarmonics)
            {
                xCoord = Noise02ShiftX + (float)x / xSize * noise02Frequency;
                yCoord = Noise02ShiftY + (float)y / zSize * noise02Frequency;
                a = a + Mathf.PerlinNoise(xCoord, yCoord) * noise02Amp;
            }
            if (ThirdWaveHarmonics)
            {
                xCoord = Noise03ShiftX + (float)x / xSize * noise03Frequency;
                yCoord = Noise03ShiftY + (float)y / zSize * noise03Frequency;
                a = a + Mathf.PerlinNoise(xCoord, yCoord) * noise03Amp;
            }
            return a;
        }
        public void CreateTerrain()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Grid";
            vertices = new Vector3[(xSize + 1) * (zSize + 1)];
            float min = float.PositiveInfinity;
            float max = float.NegativeInfinity;
            Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
            Vector2[] uv = new Vector2[vertices.Length];
            Vector4[] tangents = new Vector4[vertices.Length];
            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    float y = makeNoise(x, z);
                    vertices[i] = new Vector3(x, y, z);
                    if (vertices[i].y < min) min = vertices[i].y;
                    if (vertices[i].y > max) max = vertices[i].y;
                    tangents[i] = tangent;
                }
            }
            colors = new Color[(xSize + 1) * (zSize + 1)];
            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    float height = Mathf.InverseLerp(min, max, vertices[i].y);
                    colors[i] = gradient.Evaluate(height);
                }
            }
            mesh.vertices = vertices;
            mesh.colors = colors;
            mesh.RecalculateNormals();
            //mesh.tangents = tangents;
            triangles = new int[xSize * zSize * 6];
            for (int ti = 0, vi = 0, y = 0; y < zSize; y++, vi++)
            {
                for (int x = 0; x < xSize; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;
                }
            }
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            uvs = new Vector2[mesh.vertices.Length];
            for (int i = 0, y = 0; y <= zSize; y++)
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    uvs[i] = new Vector2((float)x / xSize, (float)y / zSize);
                }
            }
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            isDirty = true;
        }
        void OnDrawGizmos()
        {
            return;
            if (mesh.vertices == null || mesh == null)
            {
                return;
            }
            if (isDirty)
            {
                Gizmos.color = Color.grey;
                for (int i = 0; i < mesh.vertices.Length; i++)
                {
                    Gizmos.DrawSphere(transform.TransformPoint(mesh.vertices[i]), 0.05f);
                }
                isDirty = false;
            }
        }
    }
    [CustomEditor(typeof(ZTerrainMeshGeneratorPBR))]
    class ZCombineChildrenMeshPBREditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector())
            {
                if (((ZTerrainMeshGeneratorPBR)target).AutoUpdate)
                    ((ZTerrainMeshGeneratorPBR)target).CreateTerrain();
            }
            if (GUILayout.Button("Flat"))
            {
                // ((ZTerrainGenerator20)target).CreateFlat();
                //((ZTerrainGenerator20)target).UpdateMesh();
                //                ((ZTerrainGenerator20)target).StartCoroutine(
                //    ((ZTerrainGenerator20)target).CreateFlat());
            }
            if (GUILayout.Button("Terrain"))
            {
                ((ZTerrainMeshGeneratorPBR)target).CreateTerrain();
                //((ZTerrainGenerator20)target).UpdateMesh();  
                //   ((ZTerrainGenerator20)target).StartCoroutine(
                //       ((ZTerrainGenerator20)target).CreateTerrain());

            }
        }
    }
}