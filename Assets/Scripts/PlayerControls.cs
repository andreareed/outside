using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
  private Rigidbody rb;

  [Header("Camera Movement")]
  [SerializeField] float fov = 60f;
  [SerializeField] bool invertCamera = false;
  [SerializeField] float mouseSensitivity = 2f;
  [SerializeField] float maxLookAngle = 50f;
  [Space]
  [Header("Required References")]
  [SerializeField] Camera playerCamera;
  [Space]

  // Camera Internal Variables
  private float yaw = 0.0f;
  private float pitch = 0.0f;

  [Header("Player Movement")]
  [SerializeField] float walkSpeed = 5f;
  [SerializeField] float maxVelocityChange = 10f;
  [Space]


  // Movement Internal Variables
  private bool isWalking = false;
  private bool isSprinting = false;
  private float sprintRemaining;
  private bool isSprintCooldown = false;
  private float sprintCooldownReset;
  [Header("Jumping")]
  [SerializeField] KeyCode jumpKey = KeyCode.Space;
  [SerializeField] float jumpPower = 5f;

  // Jump Internal variables
  private bool isGrounded = false;

  private void Awake()
  {
    rb = GetComponent<Rigidbody>();

    playerCamera.fieldOfView = fov;
  }

  private void Update()
  {
    CheckGround();

    // Control camera movement
    yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

    if (!invertCamera)
    {
      pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
    }
    else
    {
      // Inverted Y
      pitch += mouseSensitivity * Input.GetAxis("Mouse Y");
    }

    // Clamp pitch between lookAngle
    pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

    transform.localEulerAngles = new Vector3(0, yaw, 0);
    playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

    if (Input.GetKeyDown(jumpKey) && isGrounded)
    {
      Jump();
    }
  }

  private void FixedUpdate()
  {
    // Player Movement
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    Vector3 targetVelocity = new Vector3(horizontalInput, 0, verticalInput);
    isWalking = targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded;

    targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

    // Apply a force that attempts to reach our target velocity
    Vector3 velocity = rb.velocity;
    Vector3 velocityChange = (targetVelocity - velocity);
    velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
    velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
    velocityChange.y = 0;

    rb.AddForce(velocityChange, ForceMode.VelocityChange);
    // }

  }

  private void CheckGround()
  {
    Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
    Vector3 direction = transform.TransformDirection(Vector3.down);
    float distance = .75f;

    if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
    {
      Debug.DrawRay(origin, direction * distance, Color.red);
      isGrounded = true;
    }
    else
    {
      isGrounded = false;
    }
  }

  private void Jump()
  {
    if (isGrounded)
    {
      rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
      isGrounded = false;
    }
  }
}




