using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyNPC : MonoBehaviour
{
    public float detectRadius = 999f;
    Rigidbody rr;
    // Start is called before the first frame update
    void Start()
    {
        rr = GetComponent<Rigidbody>();
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
                );
        if (hitColliders.Length > 0)
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    rr.MovePosition(Vector3.MoveTowards(transform.position, rb.position, Time.deltaTime * 3f));
            }
    }
}
