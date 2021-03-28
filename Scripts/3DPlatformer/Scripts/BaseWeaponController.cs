using UnityEngine;
using UnityEditor;
interface IWeaponControllerInterface
{
    void Shoot();
}
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class BaseWeaponController : MonoBehaviour, IWeaponControllerInterface
{
    float TimeLastShoot = 0f;
    public float ShootDelayTime = 0.1f;
    public GameObject projectile;
    void Start()
    {
        if (projectile == null)
            EditorUtility.DisplayDialog("Error", "Select Projectile type", "Ok");
    }
    void Update()
    {
        TimeLastShoot += Time.deltaTime;
    }
    public void Shoot()
    {
        if (TimeLastShoot >= ShootDelayTime)
        {
            TimeLastShoot = 0f;
            GameObject projectileGO = Instantiate(projectile, transform.position, transform.rotation);
            projectileGO.transform.parent = GameObject.Find("GameObjectBin").transform;
        }
    }
}