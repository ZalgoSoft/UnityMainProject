using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
namespace Zalgo
{
    public class ZCurvesGenerator : MonoBehaviour
    {
        [Range(0.00001f, 1f)]
        public float deltaSlopeRight = 0.00001f;
        [Range(0.00001f, 1f)]
        public float deltaSlopeLeft = 0.00001f;
        [Range(0f, 1f)]
        public float leftSide = 0f;
        [Range(0f, 1f)]
        public float rightSide = 0.5f;
        [HideInInspector]
        public AnimationCurve Curve;
        public bool AutoUpdate = true;
        public void Generate()
        {
            Curve = new AnimationCurve();
            Curve.preWrapMode = WrapMode.ClampForever;
            Curve.postWrapMode = WrapMode.ClampForever;
            Curve.AddKey(new Keyframe(0f, 0f));
            Curve.AddKey(new Keyframe(leftSide - deltaSlopeLeft, 0f));
            Curve.AddKey(new Keyframe(leftSide + deltaSlopeLeft, 1f));
            Curve.AddKey(new Keyframe(rightSide - deltaSlopeRight, 1f));
            Curve.AddKey(new Keyframe(rightSide + deltaSlopeRight, 0f));
            Curve.AddKey(new Keyframe(1f, 0f));
        }
        public void Reset()
        {
            deltaSlopeRight = 0.00001f;
            deltaSlopeLeft = 0.00001f;
            leftSide = 0f;
            rightSide = 0.5f;
            Generate();
        }
    }
    [CustomEditor(typeof(ZCurvesGenerator))]
    public class ZCurvesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ZCurvesGenerator CurvesGenerator = (ZCurvesGenerator)target;
            if (DrawDefaultInspector())
            {
                if (CurvesGenerator.AutoUpdate)
                {
                    CurvesGenerator.Generate();
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate", GUILayout.Width(135f)))
            {
                CurvesGenerator.Generate();
            }
            if (GUILayout.Button("Revert", GUILayout.Width(135f)))
            {
                CurvesGenerator.Reset();
            }
            GUILayout.EndHorizontal();
            CurvesGenerator.Curve =
                EditorGUILayout.CurveField("Height Curve", CurvesGenerator.Curve);
            //EditorGUILayout.CurveField(CurvesGenerator.Curve, new Rect(0f, 0f, 2f, 2f), GUILayout.Height(30f));

        }
    }
}