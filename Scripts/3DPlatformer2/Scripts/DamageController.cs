using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class DamageController : MonoBehaviour
{
    public float Health = 15f;
    private Animator animator;
    private Material material;
    public GameObject boxdestroy;
    void Start()
    {
        animator = GetComponent<Animator>();
        material = GetComponent<Renderer>().material;
    }
    public void takeDamage(float takenDamage, Vector3 force, Vector3 position)
    {

        if (TryGetComponent<Rigidbody>(out Rigidbody optionalRigidbody))
            optionalRigidbody.AddForceAtPosition(force, position, ForceMode.Force);
        Health -= takenDamage;
        material.color = new Color(material.color.r + 0.05f, material.color.g, material.color.b);
        if (Health <= 0f)
            Die();
        else
            animator.SetTrigger("isHit");
    }
    void Die()
    {
        GameObject g = Instantiate(boxdestroy, transform.position, transform.rotation);
        g.transform.parent = gameObject.transform.parent;
        //Destroy(g, 1f);
        Destroy(gameObject);
    }
}