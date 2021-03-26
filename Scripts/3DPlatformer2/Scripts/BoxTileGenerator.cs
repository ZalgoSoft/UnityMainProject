using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTileGenerator : MonoBehaviour
{
    public GameObject tBlock;
     GameObject tParent;
    GameObject tInstance;
    public int xSize = 10;
    public int ySize = 4;
    public int zSize = 4;
    public float spaceRatio = 1.2f;
    public void Generate()
    {
        //if (tParent == null)
        //{
            tParent = new GameObject("container");
            tParent.transform.position = Vector3.zero;
        //}

        for (float x = 0; x < xSize; ++x)
            for (float y = 0; y < ySize; ++y)
                for (float z = 0; z < xSize; ++z)
                {
                    tInstance = Instantiate(tBlock, new Vector3(x * spaceRatio, y * spaceRatio + 0.5f, z * spaceRatio), Quaternion.identity);
                    tInstance.transform.parent = tParent.transform;
                }
    }
}
[CustomEditor(typeof(BoxTileGenerator))]
class BoxTileGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var gen = (BoxTileGenerator)target;
        if (DrawDefaultInspector())
        {
        }
        if (GUILayout.Button("Generate"))
        {
            gen.Generate();
        }
    }
}