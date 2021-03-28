using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(BaseWeaponController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector3 lookDirection;
    CharacterController controller;
    private bool groundedPlayer;
    private Vector3 playerVelocity = Vector3.zero;
    BaseWeaponController leftShot;
    BaseWeaponController rightShot;
    public BaseWeaponController[] weapons;
    public bool LookHorizOnly = true;
    public bool AutoMove = false;
    public float playerSpeed = 5f;
    public float jumpHeight = 3f;
    public float gravityValue = -9.98f;
    public GameObject crossHair;
    Ray ray;
    RaycastHit hit;
    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out Rigidbody rigidBody))
            rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
        if (!TryGetComponent<CharacterController>(out controller))
            controller = gameObject.AddComponent<CharacterController>();
        weapons = gameObject.GetComponents<BaseWeaponController>();
        leftShot = weapons[0];
        rightShot = weapons[1];
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y /= -7f;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            lookDirection = hit.point;
            if (LookHorizOnly)
                lookDirection.y = 0.5f;
            crossHair.transform.position = lookDirection + Vector3.up * 0.01f;
            transform.LookAt(lookDirection, Vector3.up);
        }
        if (AutoMove)
            controller.Move((lookDirection - transform.position).normalized * playerSpeed * Time.deltaTime);
        playerVelocity = new Vector3(
            Time.deltaTime * playerVelocity.x + Input.GetAxis("Horizontal"),
            Time.deltaTime * gravityValue + playerVelocity.y,
            Time.deltaTime * playerVelocity.z + Input.GetAxis("Vertical")
            );
        if (Input.GetButtonDown("Jump") && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
        controller.Move(playerVelocity * Time.deltaTime * playerSpeed);
        if (Input.GetButton("Fire1"))
        {
            leftShot.Shoot();
        }
        if (Input.GetButton("Fire2"))
        {
            rightShot.Shoot();
        }
    }
}