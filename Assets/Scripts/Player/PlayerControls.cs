using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
  private Rigidbody rb;
  private PlayerStats playerStats;

  [Header("Camera Movement")]
  [SerializeField] float fov = 60f;
  [SerializeField] bool invertCamera = false;
  [SerializeField] float mouseSensitivity = 2f;
  [SerializeField] float maxLookAngle = 90f;
  [SerializeField] Camera playerCamera;

  // Camera Internal Variables
  private float yaw = 0.0f;
  private float pitch = 0.0f;
  public bool cameraMovementEnabled = true;
  [Space]

  [Header("Player Movement")]
  [SerializeField] float walkSpeed = 10f;
  [SerializeField] float maxVelocityChange = 10f;
  [Space]

  [Header("Player Sprint")]
  [SerializeField] bool unlimitedSprint = false;
  [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
  [SerializeField] float sprintSpeed = 15f;
  [SerializeField] float sprintRegenDelay = 2f;
  [SerializeField] float sprintFOV = 80f;
  [SerializeField] float sprintFOVStepTime = 10f;
  // Movement Internal Variables
  private bool isWalking = false;
  private bool isSprinting = false;
  private float regenDelay;
  private float staminaDrain;
  [Space]

  [Header("Jumping")]
  [SerializeField] KeyCode jumpKey = KeyCode.Space;
  [SerializeField] float jumpPower = 10f;
  // Jump Internal variables
  private bool isGrounded = false;
  private bool isJumping = false;
  private bool isFalling = false;
  [Space]

  [Header("Crouching")]
  [SerializeField] bool holdToCrouch = true;
  [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
  [SerializeField] float crouchHeight = .75f;
  [SerializeField] float speedReduction = .5f;
  // Crouch Internal Variables
  private bool isCrouched = false;
  private Vector3 originalScale;

  // Public getters
  public bool IsWalking => isWalking;
  public bool IsSprinting => isSprinting;
  public bool IsCrouched => isCrouched;
  public bool IsGrounded => isGrounded;
  public bool IsJumping => isJumping;
  public bool IsFalling => isFalling;

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
    playerStats = GetComponentInChildren<PlayerStats>();
    staminaDrain = playerStats.StaminaDrain;

    playerCamera.fieldOfView = fov;
    originalScale = transform.localScale;

    if (!unlimitedSprint)
    {
      regenDelay = sprintRegenDelay;
    }
  }

  private void Update()
  {
    CheckGround();

    // Control camera movement
    if (cameraMovementEnabled)
    {
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
    }

    // Jumping
    if (Input.GetKeyDown(jumpKey) && isGrounded)
    {
      Jump();
    }

    // Crouching
    if (Input.GetKeyDown(crouchKey))
    {
      if (holdToCrouch)
      {
        isCrouched = false;
      }
      Crouch();
    }
    if (Input.GetKeyUp(crouchKey) && holdToCrouch)
    {
      isCrouched = true;
      Crouch();
    }

    // Sprinting
    if (isSprinting)
    {
      regenDelay = sprintRegenDelay;
      playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);
      // Sprinting drains water
      playerStats.UpdateWater(-playerStats.WaterDrain * Time.deltaTime);

      if (!unlimitedSprint)
      {
        playerStats.UpdateStamina(-staminaDrain * Time.deltaTime);
        if (playerStats.stamina <= 0)
        {
          isSprinting = false;
        }
      }
    }
    else
    {
      // Sprint regen
      if (regenDelay <= 0)
      {
        playerStats.UpdateStamina(staminaDrain * Time.deltaTime);
      }
      else
      {
        regenDelay -= 1 * Time.deltaTime;
      }
    }
  }

  private void FixedUpdate()
  {
    // Player Movement
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    Vector3 targetVelocity = new Vector3(horizontalInput, 0, verticalInput);
    bool isMovingBackwards = targetVelocity.z < 0;
    isWalking = targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded;

    // Sprinting
    if (Input.GetKey(sprintKey) && playerStats.stamina > 0f && !isMovingBackwards)
    {
      targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

      // Apply a force that attempts to reach our target velocity
      Vector3 velocity = rb.velocity;
      Vector3 velocityChange = (targetVelocity - velocity);
      velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
      velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
      velocityChange.y = 0;

      // Makes sure fov change only happens during movement
      if (velocityChange.x != 0 || velocityChange.z != 0)
      {
        isSprinting = true;

        if (isCrouched)
        {
          Crouch();
        }
      }

      rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    else
    {
      isSprinting = false;
      targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

      Vector3 velocity = rb.velocity;
      Vector3 velocityChange = (targetVelocity - velocity);
      velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
      velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
      velocityChange.y = 0;

      rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
  }

  private void CheckGround()
  {
    Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
    Vector3 direction = transform.TransformDirection(Vector3.down);
    float distance = 1f;

    if (Physics.Raycast(origin, direction, out RaycastHit hit))
    {
      isGrounded = hit.distance <= distance;
      isFalling = hit.distance >= distance * 2 && rb.velocity.y < 0;
    }

    if (isJumping && isFalling)
    {
      isJumping = false;
    }
  }

  private void Jump()
  {
    if (isGrounded)
    {
      rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
      isJumping = true;
    }

    // When crouched and using toggle system, will uncrouch for a jump
    if (isCrouched && !holdToCrouch)
    {
      Crouch();
    }
  }

  private void Crouch()
  {
    // Stands player up to full height resets walkSpeed
    if (isCrouched)
    {
      transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
      walkSpeed /= speedReduction;

      isCrouched = false;
    }
    // Crouches player down to set height, reduces walkSpeed
    else
    {
      transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
      walkSpeed *= speedReduction;

      isCrouched = true;
    }
  }

  public void SetCameraMovement(bool enabled)
  {
    cameraMovementEnabled = enabled;
  }
}




