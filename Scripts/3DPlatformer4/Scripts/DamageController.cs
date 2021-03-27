using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class DamageController : MonoBehaviour
{
    public float maxHealth = 15f;
    [SerializeField]
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
    public void takeDamage(float takenDamage)
    {

        //if (TryGetComponent<Rigidbody>(out Rigidbody optionalRigidbody))
        //   optionalRigidbody.AddForceAtPosition(force * 10f, position, ForceMode.Force);
        //optionalRigidbody.AddForce( position, ForceMode.Force);
        Health -= takenDamage;
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