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
    public bool splashDamage = false;
    public float splashDamageRadius = 1f;
    GameObject projectileLight;
    Light lightComponent;
    DamageController damageController;
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
        if (other.gameObject.TryGetComponent<DamageController>(out damageController))
        {
            if (splashDamage)
            {
                float ratio;
                Rigidbody rb;
                Collider[] hitColliders =
                    Physics.OverlapSphere(
                        transform.position,
                        splashDamageRadius,
                        1 << LayerMask.NameToLayer("Damageble")
                        ); ;

                foreach (var hitCollider in hitColliders)
                {
                    ratio = 1f - Mathf.InverseLerp(0, splashDamageRadius,
                              Vector3.Distance(transform.position, hitCollider.transform.position));
                    if (hitCollider.TryGetComponent<Renderer>(out Renderer rrenderer)
                           && hitCollider.gameObject.layer == LayerMask.NameToLayer("Damageble"))
                    {
                        //rrenderer.sharedMaterial.color = new Color(
                        //  rrenderer.sharedMaterial.color.r - 0.05f,
                        //rrenderer.sharedMaterial.color.g + 0.5f *
                        //  ratio,
                        //rrenderer.sharedMaterial.color.b - 0.05f);
                    }
                    rb = hitCollider.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.AddExplosionForce(damage * ratio * 0.1f, transform.position, splashDamageRadius, 1f, ForceMode.Force);
                    if (hitCollider.TryGetComponent<DamageController>(out damageController))
                    {
                        ratio = 1f - Mathf.InverseLerp(0, splashDamageRadius,
                            Vector3.Distance(transform.position, hitCollider.transform.position));
                        damageController.takeDamage(
                            damage * ratio,
                            transform.forward,
                             hitCollider.transform.position);
                    }
                }
            }
            else
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                damageController.takeDamage(
                damage,
               transform.forward,// other.ClosestPoint(transform.position),
                other.ClosestPoint(transform.position));
            }
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }
}