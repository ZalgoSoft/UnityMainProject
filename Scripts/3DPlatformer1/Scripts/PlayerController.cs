using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float horizontal;
    [SerializeField]
    float vertical;
    [SerializeField]
    Vector3 lookDirection;
    CharacterController controller;
    private bool groundedPlayer;
    private Vector3 playerVelocity = Vector3.zero;
    public GameObject leftShot;
    public GameObject rightShot;
    public bool AutoMove = false;
    public float playerSpeed = 5f;
    public float jumpHeight = 3f;
    public float gravityValue = -9.98f;
    public float leftShootDelayTime = 0.1f;
    float leftTimeLastShoot = 0f;
    public float rightShootDelayTime = 1f;
    float rightTimeLastShoot = 0f;
    public GameObject crossHair;
    Ray ray;
    RaycastHit hit;
    void Start()
    {
        if (!TryGetComponent<CharacterController>(out controller))
            controller = gameObject.AddComponent<CharacterController>();
    }
    void Update()
    {
        leftTimeLastShoot += Time.deltaTime;
        rightTimeLastShoot += Time.deltaTime;
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y /= -7f;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f))
        {
            lookDirection = hit.point;
            crossHair.transform.position = lookDirection;
            transform.LookAt(lookDirection, Vector3.up);
        }
        if (AutoMove)
            controller.Move(lookDirection * playerSpeed * Time.deltaTime);
        playerVelocity = new Vector3(
            Time.deltaTime * playerVelocity.x + Input.GetAxis("Horizontal"),
            Time.deltaTime * gravityValue + playerVelocity.y,
            Time.deltaTime * playerVelocity.z + Input.GetAxis("Vertical")
            );
        if (Input.GetButtonDown("Jump") && groundedPlayer)
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
        controller.Move(playerVelocity * Time.deltaTime * playerSpeed);
        if (Input.GetButton("Fire1") && leftTimeLastShoot > leftShootDelayTime)
        {
            shootLeft();
            leftTimeLastShoot = 0f;
        }
        if (Input.GetButton("Fire2") && rightTimeLastShoot > rightShootDelayTime)
        {
            shootRight();
            rightTimeLastShoot = 0f;
        }
    }
    void shootLeft()
    {
        GameObject ishot = Instantiate(leftShot, transform.position, transform.rotation);
    }
    void shootRight()
    {
        GameObject ishot = Instantiate(rightShot, transform.position, transform.rotation);
    }
}