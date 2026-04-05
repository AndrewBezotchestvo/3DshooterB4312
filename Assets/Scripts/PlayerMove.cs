using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float JumpHeight = 50;
    [SerializeField] private float horizontal_sens = 0.5f;

    private CharacterController CC;
    private float velosity;
    private float moveX;
    private float moveZ;
    private float rotation;

    void Start()
    {
        CC = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal") * speed;
        moveZ = Input.GetAxis("Vertical") * speed;

        if (Input.GetKeyDown(KeyCode.Space) && CC.isGrounded)
        {
            velosity = JumpHeight;
        }
        rotation += Input.GetAxis("Mouse X") * horizontal_sens;
        transform.localEulerAngles = new Vector3(0, rotation, 0);
    }
    void FixedUpdate()
    {

        if (CC.isGrounded == true && velosity <= 0f)
        {
            velosity = -1f;
        }
        else
        {
            velosity -= gravity * Time.fixedDeltaTime;
        }


        Vector3 movement = new Vector3(moveX, velosity, moveZ);
        //movement=Vector3.ClampMagnitude(movement, speed);
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        CC.Move(movement);
    }

}
