using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
public class ExplosionSphere : MonoBehaviour
{
    GameObject go;
    Renderer cRenderer;
    public void startme()
    {
        go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.parent = gameObject.transform;
        go.transform.localPosition = Vector3.zero;
        //go.transform.position = transform.position;
        //go.transform.localPosition = transform.position;
        //go.transform.parent = transform.parent;
        //go.transform.position = transform.position;
        Destroy(go.GetComponent<SphereCollider>());
        cRenderer = go.GetComponent<Renderer>();
        cRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        cRenderer.material.SetColor("_Color", Color.red);
        cRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        cRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        cRenderer.material.SetInt("_ZWrite", 0);
        //cRenderer.material.SetColor("_EmissionColor", Color.blue);
        cRenderer.material.DisableKeyword("_ALPHATEST_ON");
        cRenderer.material.DisableKeyword("_ALPHABLEND_ON");
        cRenderer.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        //cRenderer.material.EnableKeyword("_EMISSION");
        cRenderer.material.renderQueue = 3000;
        StartCoroutine("Fade");
    }
    public IEnumerator Fade()
    {
        cRenderer.transform.localScale = Vector3.one * 2;
        for (float ft = 1f; ft >= 0f; ft -= 0.2f)
        {
            Color color = cRenderer.material.color;
            Vector3 size = cRenderer.transform.localScale;
            color.a = ft / 2;
            size *= 1.2f;
            cRenderer.material.color = color;
            cRenderer.transform.localScale = size;
            yield return null;
        }
        Destroy(gameObject);
    }
}