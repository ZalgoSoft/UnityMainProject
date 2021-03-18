using UnityEditor;
using UnityEngine;
namespace Zalgo
{
    public static class ZMaterialSaverEditor
    {
        [MenuItem("CONTEXT/MeshRenderer/Save Material...")]
        public static void SaveMaterialInPlace(MenuCommand menuCommand)
        {
            MeshRenderer meshrenderer = menuCommand.context as MeshRenderer;
            Material myMaterial = meshrenderer.sharedMaterial;
            SaveMaterial(myMaterial, myMaterial.name, false);
        }
        [MenuItem("CONTEXT/MeshRenderer/Save Material As New Instance...")]
        public static void SaveMaterialNewInstanceItem(MenuCommand menuCommand)
        {
            MeshRenderer meshrenderer = menuCommand.context as MeshRenderer;
            Material myMaterial = meshrenderer.sharedMaterial;
            SaveMaterial(myMaterial, myMaterial.name, true);
        }
        public static void SaveMaterial(Material myMaterial, string name, bool makeNewInstance)
        {
            string path = EditorUtility.SaveFilePanel("Save Separate Material Asset", "Assets/", name, "asset");
            if (string.IsNullOrEmpty(path)) return;
            path = FileUtil.GetProjectRelativePath(path);
            Material materialToSave = (makeNewInstance) ? Object.Instantiate(myMaterial) as Material : myMaterial;
            AssetDatabase.CreateAsset(materialToSave, path);
            AssetDatabase.SaveAssets();
        }
    }
}