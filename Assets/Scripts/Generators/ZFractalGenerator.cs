using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Zalgo
{
    public class ZFractalGenerator : MonoBehaviour
    {
        public float childScale = 0.5f;
        public int maxDepth = 5;
        public Mesh mesh;
        public Material material;
        private int depth;
        private Material[] materials;
        private Mesh[] meshes;
        private void Initialize(ZFractalGenerator parent, Vector3 direcion)
        {
            mesh = parent.mesh;
            materials = parent.materials;
            material = parent.material;
            maxDepth = parent.maxDepth;
            depth = parent.depth + 1;
            childScale = parent.childScale;
            transform.parent = parent.transform;
            transform.localScale = Vector3.one * childScale;
            transform.localPosition = direcion * (0.5f + 0.5f * childScale);
        }
        private void InitializeMaterials()
        {
            materials = new Material[maxDepth + 1];
            for (int i = 0; i <= maxDepth; i++)
            {
                materials[i] = new Material(material);
                materials[i].color =
                    Color.Lerp(Color.blue, Color.yellow, (float)i / maxDepth);
            }
            meshes = new Mesh[maxDepth + 1];
            for (int i = 0; i <= maxDepth; i++)
            {
                meshes[i] = new Mesh();
                meshes[i] = mesh;
            }
        }
        void Start()
        {
            if (materials == null)
            {
                InitializeMaterials();
            }
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
            gameObject.AddComponent<MeshRenderer>().material = materials[depth];
            if (depth < maxDepth)
            {
                new GameObject("Fractal Child").
                    AddComponent<ZFractalGenerator>().Initialize(this, Vector3.up);
                new GameObject("Fractal Child").
                    AddComponent<ZFractalGenerator>().Initialize(this, Vector3.right);
                new GameObject("Fractal Child").
                    AddComponent<ZFractalGenerator>().Initialize(this, Vector3.left);
                new GameObject("Fractal Child").
                    AddComponent<ZFractalGenerator>().Initialize(this, Vector3.down);
                new GameObject("Fractal Child").
                    AddComponent<ZFractalGenerator>().Initialize(this, Vector3.forward);
                new GameObject("Fractal Child").
                    AddComponent<ZFractalGenerator>().Initialize(this, Vector3.back);
            }
        }
    }
    [CustomEditor(typeof(ZFractalGenerator))]
    class ZFractalGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector())
            { }
        }
    }
}