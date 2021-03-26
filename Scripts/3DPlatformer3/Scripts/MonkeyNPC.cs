using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyNPC : MonoBehaviour
{
    public float detectRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Rigidbody rb;
        Collider[] hitColliders =
            Physics.OverlapSphere(
                transform.position,
                detectRadius,
                1 << LayerMask.NameToLayer("Player")
                ); ;
        if (hitColliders.Length != 0)
            foreach (var hitCollider in hitColliders)
            {
                gameObject.GetComponent<Rigidbody>().MoveRotation(
                    Quaternion.Euler(
                        (hitCollider.gameObject.transform.position - gameObject.transform.position)
                        ).normalized
              ); ;
            }
    }
}
