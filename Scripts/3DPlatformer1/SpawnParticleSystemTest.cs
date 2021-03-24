using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SpawnParticleSystemTest : MonoBehaviour
{
    // Start is called before the first frame update
    ParticleSystem ps;
    GameObject go;
    ParticleSystemRenderer psr;
    Texture2D t;
    void Start()
    {
        var material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        material.SetTexture("_MainTex", AssetDatabase.GetBuiltinExtraResource<Texture2D>("Default-Particle.psd"));
        go = new GameObject("myPS");
        ps = go.AddComponent<ParticleSystem>();
        psr = go.GetComponent<ParticleSystemRenderer>();
        psr.sharedMaterial = material;
        //go.GetComponent<ParticleSystemRenderer>().sharedMaterial = material;
        //go = new GameObject("myPS");//, typeof(ParticleSystem));
        //psr.material.shader = Shader.Find("HDRenderPipeline/Lit"); ;
        //t = new Texture2D(256, 256);
        //t.SetPixels(0, 0, 256, 256, new Color[256 * 256]);
        //psr.material.color = Color.red;
        //psr.sharedMaterial = new Material(Shader.Find("Particle"));
        //psr.sharedMaterial.mainTexture = t;// SetTexture("_MainTex", t);
        //psr.sharedMaterial.SetTexture("_MainTex", t);
        //ps = new ParticleSystem();
        //go.GetComponent<ParticleSystem>() = ps;
        //psr = go.GetComponent<ParticleSystemRenderer>();
        //psr.material = new Material(Shader.Find("Standard"));
        //psr.material.name = "_MainTex";
        //psr.material.color = Color.white;

        //psr.material.SetTexture("_MainTex", tex)// mainTexture
        //psr.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
