using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class D2MoveDirectionInputManager : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField]
    Vector3 lookDirection;
    [SerializeField]
    private Vector3 playerVelocity;
    [SerializeField]
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float jumpHeight = 3.0f;
    private float gravityValue = -9.81f;
    //public UnityEngine.UI.Text debug1;
    Ray ray;
    RaycastHit hit;
    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
    }
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        //  debug1.text = groundedPlayer.ToString();
        if (groundedPlayer && playerVelocity.y < 0)
            playerVelocity.y /= -7f;

        playerVelocity = new Vector3(
            Time.deltaTime * playerVelocity.x + Input.GetAxis("Horizontal"),
            Time.deltaTime * gravityValue + playerVelocity.y,
            Time.deltaTime * playerVelocity.z + Input.GetAxis("Vertical")
            );
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
        }
        controller.Move(playerVelocity * Time.deltaTime * playerSpeed);
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000f))
            lookDirection = hit.point;
        transform.LookAt(lookDirection, Vector3.up);
    }
}