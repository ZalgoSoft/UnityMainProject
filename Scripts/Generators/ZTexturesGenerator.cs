using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Zalgo
{
    [RequireComponent(typeof(Terrain))]
    public class ZTexturesGenerator : MonoBehaviour
    {
        public List<TerrainLayerAsset> TerranLayersList = new List<TerrainLayerAsset>();
        public void Generate()
        {
            if (TerranLayersList == null)
            {
                throw new NullReferenceException("Textures list not setted");
            }
            TerrainData terrainData = Terrain.activeTerrain.terrainData;

            terrainData.terrainLayers = TerranLayersList.Select((item) => item.layer).ToArray();
            if (terrainData.alphamapResolution != terrainData.size.x)
            {
                Debug.LogError("terrainData.alphamapResolution must fit terrain size");
            }
            float[,,] textureMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
            float terrainMaxHeight = terrainData.size.y;
            float x = 0.0f;
            while (x < terrainData.alphamapHeight)
            {
                float y = 0.0f;
                while (y < terrainData.alphamapWidth)
                {
                    float height = terrainData.GetHeight((int)x, (int)y);
                    float heightScaled = height / terrainMaxHeight;
                    float xS = x / terrainData.heightmapResolution;
                    float yS = y / terrainData.heightmapResolution;
                    float steepness = terrainData.GetSteepness(xS, yS);
                    float angleScaled = steepness / 90.0f;
                    for (int i = 0; i < terrainData.alphamapLayers; i++)
                    {
                        switch (TerranLayersList[i].Type)
                        {
                            case (0):
                                if (i != 0)
                                {
                                    textureMap[(int)y, (int)x, i] = TerranLayersList[i].HeightCurve.Evaluate(heightScaled);
                                    for (int hi = 0; hi < i; hi++)
                                    {
                                        textureMap[(int)y, (int)x, hi] *= (textureMap[(int)y, (int)x, i] - 1) / -1;
                                    }
                                }
                                else
                                {
                                    textureMap[(int)y, (int)x, i] = TerranLayersList[i].HeightCurve.Evaluate(heightScaled);
                                }
                                break;
                            case (1):
                                textureMap[(int)y, (int)x, i] = TerranLayersList[i].AngleCurve.Evaluate(angleScaled);
                                for (int ai = 0; ai < i; ai++)
                                {
                                    textureMap[(int)y, (int)x, ai] *= (textureMap[(int)y, (int)x, i] - 1) / -1;
                                }
                                break;
                            default:
                                break;
                        }
                        if (textureMap[(int)y, (int)x, i] > 1.0f) { textureMap[(int)y, (int)x, i] = 1.0f; }
                    }
                    y++;
                }
                x++;
            }
            terrainData.SetAlphamaps(0, 0, textureMap);
        }
        public void Clear()
        {
            TerranLayersList = new List<TerrainLayerAsset>();
            Generate();
        }
        void Reset()
        {
            Clear();
        }
    }
    public class TerrainLayerAsset
    {
        //public Texture2D Texture { get; set; }
        //public Vector2 Tilesize = new Vector2(1, 1);
        public TerrainLayer layer { get; set; }
        public Color Color { get; set; }
        public int Type { get; set; }
        public AnimationCurve HeightCurve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);
        public AnimationCurve AngleCurve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);
    }
    [CustomEditor(typeof(ZTexturesGenerator))]
    class ZTexturesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var generator = (ZTexturesGenerator)target;
            foreach (var t in generator.TerranLayersList)
            {
                EditorGUILayout.BeginHorizontal();
                t.layer = EditorGUILayout.ObjectField("TerrainLayer", t.layer, typeof(TerrainLayer), false) as TerrainLayer;
                t.Type = EditorGUILayout.Popup(t.Type, new string[] { "Height", "Angle" });
                EditorGUILayout.EndHorizontal();
                switch (t.Type)
                {
                    case (0):
                        t.HeightCurve = EditorGUILayout.CurveField("Height Curve", t.HeightCurve);
                        break;
                    case (1):
                        t.AngleCurve = EditorGUILayout.CurveField("Angle Curve", t.AngleCurve);
                        break;
                    default:
                        break;
                }
                EditorGUILayout.LabelField("");
            }
            if (generator.TerranLayersList.Count > 0)
            {
                if (GUILayout.Button("Delete last"))
                {
                    generator.TerranLayersList.RemoveAt(generator.TerranLayersList.Count - 1);
                }
            }
            if (GUILayout.Button("Add"))
            {
                generator.TerranLayersList.Add(new TerrainLayerAsset());
            }
            if (generator.TerranLayersList.Count > 0)
            {
                if (GUILayout.Button("Generate"))
                {
                    generator.Generate();
                }
            }
            if (GUILayout.Button("Clear"))
            {
                generator.Clear();
            }
        }
    }
}