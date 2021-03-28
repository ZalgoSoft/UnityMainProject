using UnityEditor;
using UnityEngine;
public class BoxTileGenerator : MonoBehaviour
{
    public GameObject tBlock;
    GameObject tParent;
    GameObject tInstance;
    public int xSize = 1;
    public int ySize = 1;
    public int zSize = 1;
    public float spaceRatio = 1.2f;
    #region obsolete
    public void reGenerate()
    {
        tParent = GameObject.Find("container");
        foreach (Rigidbody item in tParent.GetComponentsInChildren<Rigidbody>())
        {
            item.ResetInertiaTensor();
            item.ResetCenterOfMass();
            item.position = Vector3.zero;
        }
        Debug.Log(tParent.GetComponentsInChildren<Rigidbody>().Length);
    }
    #endregion
    public void Generate()
    {
        tParent = new GameObject("container");
        tParent.transform.parent = transform;
        for (float x = 0; x < xSize; ++x)
            for (float y = 0; y < ySize; ++y)
                for (float z = 0; z < zSize; ++z)
                {
                    tInstance = (GameObject)PrefabUtility.InstantiatePrefab(tBlock, tParent.transform);
                    tInstance.transform.localPosition += new Vector3(x * spaceRatio, y * spaceRatio + 0.5f, z * spaceRatio);
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
}