using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClickPutPrefab : MonoBehaviour
{
    public GameObject prefab;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Vector3 putPosition = hit.point + hit.normal * 0.5f;
                putPosition = new Vector3(
                    Mathf.Round(putPosition.x - 0.5f) + 0.5f,
                    Mathf.Round(putPosition.y - 0.5f) + 0.5f,
                    Mathf.Round(putPosition.z - 0.5f) + 0.5f);
                GameObject clone = Instantiate(prefab, putPosition, Quaternion.identity);
                clone.name = "Block!";
                //Destroy(clone, 5);                
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.collider.gameObject.name.ToString() == "Block!")
                    Destroy(hit.transform.gameObject);
            }
            Debug.Log(hit.transform.gameObject.name.ToString());
        }
    }
    void OnDrawGizmos()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 putPosition = hit.point + hit.normal * 0.5f;
            putPosition = new Vector3(
                Mathf.Round(putPosition.x - 0.5f) + 0.5f,
                Mathf.Round(putPosition.y - 0.5f) + 0.5f,
                Mathf.Round(putPosition.z - 0.5f) + 0.5f);
            Debug.DrawRay(hit.point, hit.normal, Color.red, 0.1f);
            Debug.DrawLine(hit.point, putPosition, Color.blue, 0.1f);
            //text1.text = hit.point.x.ToString() + " " + hit.point.y.ToString() + " " + hit.point.z.ToString();
            //text2.text = rounded.x.ToString() + " " + rounded.y.ToString() + " " + rounded.z.ToString();
        }
    }

}