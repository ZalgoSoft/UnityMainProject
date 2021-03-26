using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    public Rigidbody projectile;
    public float speed = 4;
    private void OnDrawGizmos()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 putPosition = hit.point + hit.normal * 0.5f;
            Debug.DrawRay(hit.point, hit.normal, Color.red, 0.1f);
            Debug.DrawLine(hit.point, putPosition, Color.blue, 0.1f);
            //text1.text = hit.point.x.ToString() + " " + hit.point.y.ToString() + " " + hit.point.z.ToString();
            //text2.text = rounded.x.ToString() + " " + rounded.y.ToString() + " " + rounded.z.ToString();
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.DrawRay(hit.point, transform.forward, Color.green, 0.1f);
            Debug.DrawRay(hit.point, hit.collider.ClosestPoint(transform.position), Color.yellow, 0.1f);
            ;
        }

    }
    void Update()
    {

    }
}