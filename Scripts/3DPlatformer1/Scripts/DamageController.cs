using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class DamageController : MonoBehaviour
{
    public float Health = 15f;
    private Animator animator;
    private Material material;
    void Start()
    {
        animator = GetComponent<Animator>();
        material = GetComponent<Renderer>().material;
    }
    public void takeDamage(float takenDamage, Vector3 force, Quaternion direction)
    {

        if (TryGetComponent<Rigidbody>(out Rigidbody optionalRigidbody))
            optionalRigidbody.AddForceAtPosition(force, direction.eulerAngles, ForceMode.VelocityChange);
        Health -= takenDamage;
        material.color = new Color(material.color.r + 0.05f, material.color.g, material.color.b);
        animator.SetTrigger("isHit");
        if (Health <= 0f)
            Die();
    }
    void Die()
    {
        Destroy(gameObject, 0.2f);
    }
}