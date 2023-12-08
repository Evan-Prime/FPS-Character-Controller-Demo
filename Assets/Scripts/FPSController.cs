using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement Settting")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float jumpVelocity = 5.0f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Look Settting")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float upDownLimit = 65f;

    
    private bool isGrounded;
    private float verticalRotation;
    private Camera playerCamera;
    private Vector3 currentMovement = Vector3.zero;
    private Vector3 velocity;
    private CharacterController characterController;
    


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentSpeed = walkSpeed;
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        HandleMovement();
        HandleLook();
    }
    void HandleMovement()
    {
        Vector3 playerMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        playerMovement = transform.rotation * playerMovement;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        currentMovement.x = playerMovement.x * currentSpeed;
        currentMovement.z = playerMovement.z * currentSpeed;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        

        // Check for jumps.
        if (isGrounded && Input.GetAxis("Jump") == 1)
        {
            velocity.y = Mathf.Sqrt(jumpVelocity * -2f * gravity);
        }


        characterController.Move(currentMovement * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);


    }

    void HandleLook()
    {
        float mouseXRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        this.transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownLimit, upDownLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
