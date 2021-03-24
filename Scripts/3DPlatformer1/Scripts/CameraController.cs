using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public GameObject followPlayer;
    public bool isApplyOptimizedCameraSettings = true;
    void Start()
    {
        if (followPlayer == null)
            followPlayer = GameObject.Find("Player");
        if (isApplyOptimizedCameraSettings)
            ApplyOptimizedCameraSettings();
    }
    void ApplyOptimizedCameraSettings()
    {
        Camera cam = GetComponent<Camera>();
        cam.transform.position = new Vector3(0f, 10f, 0f);
        cam.transform.rotation = Quaternion.Euler(45f, 30f, 5f);
        cam.orthographic = true;
        cam.nearClipPlane = -500f;
        cam.farClipPlane = 500f;
    }
    void Update()
    {
        transform.position += (followPlayer.transform.position - transform.position) * Time.deltaTime;
    }
}
