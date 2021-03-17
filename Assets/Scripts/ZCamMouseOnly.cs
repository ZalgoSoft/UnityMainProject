using UnityEngine;
[RequireComponent(typeof(Camera))]
public class ZCamMouseOnly : MonoBehaviour
{
    Vector3 anchorPoint;
    Quaternion anchorRot;
    public float speed = 0.1f;
    public float sensitivity = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void LateUpdate()
    {   //rotate on click
        if (Input.GetMouseButtonDown(1))
        {
            anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            anchorRot = transform.rotation;
        }
        if (Input.GetMouseButton(1))
        {
            Quaternion rot = anchorRot;
            Vector3 dif = anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            rot.eulerAngles += dif * sensitivity;
            transform.rotation = rot;
        }
        //rotate and move
        if (Input.GetMouseButtonDown(2))
        {
            anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            anchorRot = transform.rotation;
        }
        if (Input.GetMouseButton(2))
        {
            Quaternion rot = anchorRot;
            Vector3 dif = anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
            rot.eulerAngles += dif * sensitivity;
            transform.rotation = rot;
            transform.Translate(speed * Vector3.forward);
        }
        //scroll for up/down
        if (Input.mouseScrollDelta.y != 0)
        {
            Vector3 pos = transform.position;
            pos.y -= Input.mouseScrollDelta.y * speed;
            transform.position = pos;
        }
    }
}