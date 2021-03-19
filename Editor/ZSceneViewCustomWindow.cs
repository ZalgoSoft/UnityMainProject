using UnityEngine;
using UnityEditor;
public class ZSceneViewCustomWindow : EditorWindow
{
    GameObject go;
    Camera c;
    SceneView.CameraSettings settings;
    bool toggle = false;
    Vector4 camvalue;
    [MenuItem("Window/SceneView Customize")]
    static void Init()
    {
        ZSceneViewCustomWindow window = (ZSceneViewCustomWindow)EditorWindow.GetWindow(typeof(ZSceneViewCustomWindow));
        window.titleContent = new GUIContent("SceneView Custom");
        window.Show();
    }
    void OnGUI()
    {
        Transform cvtransform = SceneView.lastActiveSceneView.camera.transform;
        settings = SceneView.lastActiveSceneView.cameraSettings;
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();
        SceneView.lastActiveSceneView.pivot = EditorGUILayout.Vector3Field("Pivot position", SceneView.lastActiveSceneView.pivot);
        cvtransform.position = EditorGUILayout.Vector3Field("Camera position", cvtransform.position);
        float f = EditorGUILayout.FloatField("Camera distance", SceneView.lastActiveSceneView.cameraDistance);
        cvtransform.rotation = V4toQ(EditorGUILayout.Vector4Field("Rotation", QToV4(cvtransform.rotation)));
        cvtransform.eulerAngles = EditorGUILayout.Vector4Field("Euler angles", cvtransform.eulerAngles);
        if (EditorGUI.EndChangeCheck())
            SceneView.lastActiveSceneView.Repaint();
        toggle = EditorGUILayout.Foldout(toggle, "Additional SceneView settings", true, EditorStyles.foldout);
        if (toggle)
        {
            EditorGUI.BeginChangeCheck();
            settings.accelerationEnabled = EditorGUILayout.Toggle("Acceleration enabled", settings.accelerationEnabled);
            settings.easingEnabled = EditorGUILayout.Toggle("Easing enabled", settings.easingEnabled);
            settings.easingDuration = EditorGUILayout.FloatField("Easing duration", settings.easingDuration);
            settings.speedMin = EditorGUILayout.FloatField("Speed min", settings.speedMin);
            settings.speedMax = EditorGUILayout.FloatField("Speed max", settings.speedMax);
            settings.speed = EditorGUILayout.FloatField("Speed", settings.speed);
            settings.speedNormalized = EditorGUILayout.FloatField("Speed normalized", settings.speedNormalized);
            settings.fieldOfView = EditorGUILayout.FloatField("Field of view", settings.fieldOfView);
            settings.nearClip = EditorGUILayout.FloatField("Near Clip", settings.nearClip);
            settings.farClip = EditorGUILayout.FloatField("Far clip", settings.farClip);
            settings.dynamicClip = EditorGUILayout.Toggle("Dynamic clipping", settings.dynamicClip);
            settings.occlusionCulling = EditorGUILayout.Toggle("Occlusion culling", settings.occlusionCulling);
            if (EditorGUI.EndChangeCheck())
            {
                SceneView.lastActiveSceneView.cameraSettings = settings;
                SceneView.lastActiveSceneView.Repaint();
            }
            if (GUILayout.Button("Reset to defaults"))
            {
                SceneView.lastActiveSceneView.ResetCameraSettings();
                SceneView.lastActiveSceneView.camera.Reset();
                SceneView.lastActiveSceneView.pivot = Vector3.zero;
                SceneView.lastActiveSceneView.Repaint();
            }
            EditorGUILayout.EndVertical();
        }
    }
    void Update()
    {
        Repaint();
    }
    static Vector4 QToV4(Quaternion rot)
    {
        return new Vector4(rot.x, rot.y, rot.z, rot.w);
    }
    static Quaternion V4toQ(Vector4 rot)
    {
        return new Quaternion(rot.x, rot.y, rot.z, rot.w);
    }

}