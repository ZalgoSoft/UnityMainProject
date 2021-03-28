using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IProjectileControllerInterface
{
}
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ProjectileRifleController : MonoBehaviour, IProjectileControllerInterface
{
    public float TimeToLive = 2f;
    public float bulletSpeed = 20f;
    public float damage = 2f;
    public float spashForce = 1000f;
    public bool splashDamage = false;
    public float splashDamageRadius = 1f;
    GameObject projectileLight;
    Light lightComponent;
    DamageController otherDamageController;
    Rigidbody otherRigidBody;
    GameObject explosionSphere;
    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out Rigidbody rigidBody))
            rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        if (!TryGetComponent<Collider>(out Collider collider))
            collider = gameObject.AddComponent<Collider>();
        collider.isTrigger = true;
        explosionSphere = new GameObject("explcir", typeof(ExplosionSphere));
        projectileLight = new GameObject("bulletLight", typeof(Light));

        lightComponent = projectileLight.GetComponent<Light>();
        lightComponent.type = LightType.Point;
        lightComponent.intensity = 1f;
        lightComponent.enabled = true;
        projectileLight.transform.parent = gameObject.transform;
        projectileLight.transform.localPosition = Vector3.zero;

        explosionSphere.transform.parent = gameObject.transform;
        explosionSphere.transform.localPosition = Vector3.zero;

        Destroy(gameObject, TimeToLive);
    }
    void OnDestroy()
    {
        //explosionSphere.transform.parent = gameObject.transform.parent;
        //explosionSphere.GetComponent<ExplosionSphere>().startme();
        projectileLight.transform.parent = gameObject.transform.parent;
        lightComponent.intensity *= 5;
        //lightComponent.enabled = true;
        Destroy(projectileLight, 0.1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, splashDamageRadius);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            if (splashDamage)
            {
                Vector3 explosionPos = transform.position;
                Collider[] hitColliders =
                    Physics.OverlapSphere(
                        explosionPos,
                        splashDamageRadius,
                        1 << LayerMask.NameToLayer("Damageble"));
                foreach (Collider hitCollider in hitColliders)
                {
                    float ratio = 1f - Mathf.InverseLerp(0, splashDamageRadius,
                              Vector3.Distance(transform.position, hitCollider.transform.position));
                    if (hitCollider.attachedRigidbody)
                        hitCollider.attachedRigidbody.AddExplosionForce(
                            damage * spashForce,
                            explosionPos,
                            splashDamageRadius,
                            0f,
                            ForceMode.Force);
                    if (hitCollider.TryGetComponent(out otherDamageController))
                        otherDamageController.takeDamage(damage * ratio);
                }
            }
            else
                if (other.gameObject.TryGetComponent(out otherDamageController))
                otherDamageController.takeDamage(damage);

            #region
            /*

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                damageController.takeDamage(
                damage,
                                   //transform.forward,  
                                   other.ClosestPoint(transform.position),
                other.ClosestPoint(transform.position));
            }

            if (other.gameObject.TryGetComponent<Rigidbody>(out otherRigidBody))
            {
                float ratio;
                Collider[] hitColliders =
                        Physics.OverlapSphere(
                            transform.position,
                            splashDamageRadius,
                            1 << LayerMask.NameToLayer("Damageble")
                            );
                foreach (var hitCollider in hitColliders)
                {
                    ratio = 1f - Mathf.InverseLerp(0,
                                                    splashDamageRadius,
                                                    Vector3.Distance(transform.position, hitCollider.transform.position));
                    otherRigidBody = hitCollider.GetComponent<Rigidbody>();
                    otherRigidBody.AddForceAtPosition((transform.position - hitCollider.transform.position) * ratio,
                        hitCollider.ClosestPoint(hitCollider.ClosestPoint(transform.position))); //ratio * 10f, other.gameObject.transform.position, splashDamageRadius);// 
                }
            }
                       */
            #endregion

            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }
}