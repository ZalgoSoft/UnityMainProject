using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
[AttributeUsage(AttributeTargets.All)]
public class ZUnityReflectorAttribute : Attribute
{
    private string myName;
    public ZUnityReflectorAttribute(string name) { myName = name; }
    public string Name { get { return myName; } }
}
public class ZUintyReflectorWindow : EditorWindow
{
    string path = String.Empty;
    string text = String.Empty;
    string output = String.Empty;
    bool isTextUpdated = false;
    Vector2 scroll;
    [MenuItem("Window/Unity Assembly Reflector")]
    static void Init()
    {
        ZUintyReflectorWindow window = (ZUintyReflectorWindow)EditorWindow.GetWindow(typeof(ZUintyReflectorWindow));
        window.titleContent = new GUIContent("Reflector");
        EditorStyles.textArea.wordWrap = true;
        window.Show();
    }
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Open..."))
        {
            path = EditorUtility.OpenFilePanel("Open assembly file", "", "dll");
            text = path;
        }
        if (GUILayout.Button("Show") && path != String.Empty)
        {
            text = "Listing contents of assembly " + path + "\n";
            Assembly a = Assembly.LoadFile(path);
            Type[] types = a.GetTypes();
            foreach (Type t in types)
            {
                text += t.MemberType + ": " + t + "\n"; ;
                MemberInfo[] mbrInfoArray = t.GetMembers();
                foreach (MemberInfo mbrInfo in mbrInfoArray)
                {
                    object[] attrs = mbrInfo.GetCustomAttributes(false);
                    if (attrs.Length > 0)
                    {
                        foreach (object o in attrs)
                            text += o + "\n"; ;
                    }
                    text += mbrInfo.MemberType + ": " + mbrInfo + "\n"; ;
                }
            }
            text += types.Length + " types found\n";
            isTextUpdated = true;
        }
        EditorGUILayout.EndHorizontal();
        scroll = EditorGUILayout.BeginScrollView(scroll);
        output = EditorGUILayout.TextArea(text, EditorStyles.textArea, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
        isTextUpdated = false;
        EditorGUILayout.EndVertical();
    }
}