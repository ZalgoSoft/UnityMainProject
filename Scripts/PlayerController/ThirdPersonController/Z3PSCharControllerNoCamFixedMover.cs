using UnityEngine;
// как пользоваться: 
// 1. добавить куб или другой объект, убедиться, что координаты по нулям
// 2. поставить на него скрипт
// 3. Создать новый Layer(Слой) Ground
// 4. Добавить объект который будет землей и дать ему Layer(Слой) Ground
// 5. Выставить камеру, она передвигаться не будет
// годится для платформенного 3D

[RequireComponent(typeof(CharacterController))]
public class Z3PSCharControllerNoCamFixedMover : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float Gravity = -9.81f;
    public float DashDistance = 5f;
    public LayerMask Ground = 0;
    public Vector3 Drag = new Vector3(1, 1, 1);
    private CharacterController mcc;
    private Vector3 myVelocity;
    void Start()
    {
        mcc = GetComponent<CharacterController>();
        mcc.enableOverlapRecovery = false;
    }
    void FixedUpdate()
    {
        if (mcc.isGrounded && myVelocity.y < 0)
            myVelocity.y = 0;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (move != Vector3.zero)
            mcc.transform.forward = move;
        if (Input.GetButtonDown("Jump") && mcc.isGrounded)
            myVelocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);
        if (Input.GetButtonDown("Fire1"))//dash
        {
            myVelocity += Vector3.Scale(
                mcc.transform.forward,
                DashDistance * new Vector3(
                (Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime),
                (Mathf.Log(1f / (Time.deltaTime * Drag.y + 1)) / -Time.deltaTime),
                (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime))
                );
        }
        myVelocity.y += Gravity * Time.deltaTime;
        myVelocity.x /= 1 + Drag.x * Time.deltaTime;
        myVelocity.y /= 1 + Drag.y * Time.deltaTime;
        myVelocity.z /= 1 + Drag.z * Time.deltaTime;
        myVelocity += move * Speed * Time.deltaTime;
        mcc.Move(myVelocity * Time.deltaTime);
    }
}