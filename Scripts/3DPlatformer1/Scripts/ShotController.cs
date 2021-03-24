using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ShotController : MonoBehaviour
{

    //public Material explosionTexture;
    //public LayerMask LayerToDamage;
    public float bulletSpeed = 20f;
    public float damage = 2f;
    public float TimeToLive = 2f;
    public bool splashDamage = false;
    public float splashDamageRadius = 1f;
    float timeLived = 0f;
    [SerializeField]
    static public Material particleMaterial;
    DamageController damageController;
    GameObject ParticleSystemHitExplosionGO;
    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out Rigidbody rigidBody))
            rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        if (!TryGetComponent<Collider>(out Collider collider))
            collider = gameObject.AddComponent<Collider>();
        collider.isTrigger = true;
        transform.parent = GameObject.Find("GameObjectBin").transform;
        if (particleMaterial == null)
            particleMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        //if (splashDamage)
        //  collider.contactOffset = splashDamageRadius;
    }
    void creategoExplosionPS()
    {
        #region Particle System
        ParticleSystemHitExplosionGO = new GameObject("ParticleSystemHitExplosion");

        ParticleSystem ps = ParticleSystemHitExplosionGO.AddComponent<ParticleSystem>();
        ps.Stop();

        ParticleSystemHitExplosionGO.GetComponent<ParticleSystemRenderer>().sharedMaterial = particleMaterial;
        ParticleSystemHitExplosionGO.GetComponent<ParticleSystemRenderer>().enabled = true;
        //psr.enabled = true;        

        ParticleSystem.MainModule mm = ps.main;
        mm.startSize = 0.3f;
        mm.simulationSpace = ParticleSystemSimulationSpace.World;
        mm.duration = 0.05f;
        mm.startSpeed = 5f;
        mm.startLifetime = 1f;
        mm.loop = false;

        ParticleSystem.CollisionModule cm = ps.collision;
        cm.enabled = true;
        cm.mode = ParticleSystemCollisionMode.Collision3D;
        cm.type = ParticleSystemCollisionType.World;
        mm.startColor = new ParticleSystem.MinMaxGradient(Color.white);//Color.white;

        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = true;
        em.rateOverDistance = 10;
        em.rateOverTime = 10;
        em.burstCount = 1;
        em.SetBurst(0, new ParticleSystem.Burst(0f, 10));

        ParticleSystem.ShapeModule sh = ps.shape;
        sh.enabled = true;
        sh.shapeType = ParticleSystemShapeType.Hemisphere;

        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.grey, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
            );

        ParticleSystem.ColorOverLifetimeModule col = ps.colorOverLifetime;
        col.enabled = true;
        col.color = grad;
        //col.enabled = true;
        //ParticleSystem.MinMaxGradient minMaxGradient = new ParticleSystem.MinMaxGradient(grad); ;
        //minMaxGradient.mode = ParticleSystemGradientMode.RandomColor;            
        ps.Play();
        #endregion
        #region Light explosionFlash
        Light explosionFlash = ParticleSystemHitExplosionGO.AddComponent<Light>();
        explosionFlash.type = LightType.Point;
        explosionFlash.intensity = 10f;
        Destroy(explosionFlash, 0.1f);
        #endregion
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, splashDamageRadius);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<DamageController>(out damageController))
        {
            creategoExplosionPS();

            ParticleSystemHitExplosionGO.transform.parent = GameObject.Find("GameObjectBin").transform;

            if (splashDamage)
            {
                ParticleSystemHitExplosionGO.transform.position = transform.position;
                Rigidbody rb;
                Collider[] hitColliders =
                    Physics.OverlapSphere(
                        transform.position,
                        splashDamageRadius,
                        1 << LayerMask.NameToLayer("Damageble")
                        ); ;

                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.TryGetComponent<Renderer>(out Renderer rrenderer)
                           && hitCollider.gameObject.layer == LayerMask.NameToLayer("Damageble"))
                    {
                        rrenderer.sharedMaterial.color = new Color(
                            rrenderer.sharedMaterial.color.r,
                            rrenderer.sharedMaterial.color.g + 0.05f,
                            rrenderer.sharedMaterial.color.b);
                    }
                    rb = hitCollider.GetComponent<Rigidbody>();
                    if (rb != null)
                        rb.AddExplosionForce(damage, transform.position, splashDamageRadius, 1f, ForceMode.Impulse);
                    if (hitCollider.TryGetComponent<DamageController>(out damageController))
                        damageController.takeDamage(
                            damage,
                            -hitCollider.transform.position,
                            hitCollider.transform.rotation);
                }
            }
            else
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                ParticleSystemHitExplosionGO.transform.position = hit.point;
                //ParticleSystemHitExplosionGO.transform.rotation.SetEulerAngles(hit.normal);
                damageController.takeDamage(
                    damage,
                    -hit.normal,
                    hit.transform.rotation);
            }
            //other.attachedRigidbody.AddForce(other.poi  ClosestPoint(bc.transform.position), ForceMode.Impulse);
            //goExplosionPS.transform.position = other.ClosestPoint(transform.position);//(bc.transform.position);
            //goExplosionPS.transform.rotation = other.transform.rotation;
            Destroy(ParticleSystemHitExplosionGO, 1f);
            Destroy(gameObject, 0.01f);
        }
    }
    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        if ((timeLived += Time.deltaTime) > TimeToLive)
            Destroy(gameObject);
    }
}