using UnityEngine;
using UnityEngine.EventSystems;
//For 2D Object (Sprite Renderer/any 2D Collider)
public class ZSpriteDetector : MonoBehaviour, IPointerDownHandler
{
    void Start()
    {
        addPhysics2DRaycaster();
    }

    void addPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }
    //Implement Other Events from Method 1
}