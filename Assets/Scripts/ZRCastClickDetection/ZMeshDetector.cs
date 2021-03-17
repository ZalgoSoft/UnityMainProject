using UnityEngine;
using UnityEngine.EventSystems;
//For 3D Object (Mesh Renderer/any 3D Collider)
public class ZMeshDetector : MonoBehaviour, IPointerDownHandler
{
    void Start()
    {
        addPhysicsRaycaster();
    }
    void addPhysicsRaycaster()
    {
        PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }
    //Implement Other Events from Method 1
}