using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ZCombineChildrenMesh : MonoBehaviour
{
    public void doCombine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        HashSet<CombineInstance> ci = new HashSet<CombineInstance>();
        foreach (MeshFilter item in meshFilters)
            if (item.sharedMesh)
                ci.Add(new CombineInstance
                {
                    mesh = item.sharedMesh,
                    transform = item.transform.localToWorldMatrix
                });
        Mesh mm = new Mesh { name = "CombinedMeshTemp" };
        mm.CombineMeshes(ci.ToArray());
        transform.GetComponent<MeshFilter>().sharedMesh = mm;

        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
        HashSet<Material> mats = new HashSet<Material>();
        foreach (MeshRenderer item1 in mrs)
            foreach (Material item2 in item1.sharedMaterials)
                if (item2)
                    mats.Add(item2);
        mr.sharedMaterials = mats.ToArray();
    }
    [CustomEditor(typeof(ZCombineChildrenMesh))]
    class ZCombineChildrenMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (DrawDefaultInspector())
            { }
            if (GUILayout.Button("Combine meshes and materials"))
            {
                ((ZCombineChildrenMesh)target).doCombine();
            }
        }
    }
}