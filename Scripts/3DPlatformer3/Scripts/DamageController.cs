using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class DamageController : MonoBehaviour
{
    public float maxHealth = 15f;
    float Health;
    private Animator animator;
    private Material material;
    public GameObject boxdestroy;
    HealthBar healthBarGO;
    void Start()
    {
        Health = maxHealth;
        animator = GetComponent<Animator>();
        material = GetComponent<Renderer>().material;
        TryGetComponent<HealthBar>(out healthBarGO);
    }
    public void takeDamage(float takenDamage, Vector3 force, Vector3 position)
    {

        if (TryGetComponent<Rigidbody>(out Rigidbody optionalRigidbody))
            optionalRigidbody.AddForceAtPosition(force * 0.1f, position, ForceMode.VelocityChange);
        Health -= takenDamage;

        //material.color = new Color(material.color.r + 0.05f, Mathf.Lerp(0f, maxHealth, Health) / maxHealth, material.color.b - 0.05f);
        if (Health <= 0f)
            Die();
        else
            animator.SetTrigger("isHit");

        healthBarGO.AddjustCurrentHealth(Health, maxHealth);
    }
    void Die()
    {
        GameObject g = Instantiate(boxdestroy, transform.position, transform.rotation);
        g.transform.parent = gameObject.transform.parent;
        Destroy(g, 1f);
        Destroy(gameObject);
    }
}