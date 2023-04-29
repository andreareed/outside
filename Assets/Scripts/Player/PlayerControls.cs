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
  [SerializeField] float maxLookAngle = 50f;
  [SerializeField] Camera playerCamera;

  // Camera Internal Variables
  private float yaw = 0.0f;
  private float pitch = 0.0f;
  public bool cameraMovementEnabled = true;
  [Space]

  [Header("Player Movement")]
  [SerializeField] float walkSpeed = 5f;
  [SerializeField] float maxVelocityChange = 10f;
  [Space]

  [Header("Player Sprint")]
  [SerializeField] bool unlimitedSprint = false;
  [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
  [SerializeField] float sprintSpeed = 7f;
  [SerializeField] float sprintRegenDelay = 2f;
  [SerializeField] float sprintFOV = 80f;
  [SerializeField] float sprintFOVStepTime = 10f;
  // Movement Internal Variables
  private bool isWalking = false;
  private bool isSprinting = false;
  private float sprintRemaining;
  private float regenDelay;
  private float staminaDrain;
  [Space]

  [Header("Jumping")]
  [SerializeField] KeyCode jumpKey = KeyCode.Space;
  [SerializeField] float jumpPower = 5f;
  // Jump Internal variables
  private bool isGrounded = false;
  [Space]

  [Header("Crouching")]
  [SerializeField] bool holdToCrouch = true;
  [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
  [SerializeField] float crouchHeight = .75f;
  [SerializeField] float speedReduction = .5f;
  // Crouch Internal Variables
  private bool isCrouched = false;
  private Vector3 originalScale;

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
    playerStats = GetComponentInChildren<PlayerStats>();
    staminaDrain = playerStats.GetStaminaDrain();


    playerCamera.fieldOfView = fov;
    originalScale = transform.localScale;

    if (!unlimitedSprint)
    {
      sprintRemaining = playerStats.stamina;
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
      playerStats.UpdateWater(-playerStats.GetWaterDrain() * Time.deltaTime);

      if (!unlimitedSprint)
      {
        sprintRemaining -= 1 * Time.deltaTime;
        playerStats.UpdateStamina(-staminaDrain * Time.deltaTime);
        if (sprintRemaining <= 0)
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
        sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, playerStats.stamina);
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
    isWalking = targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded;

    // Sprinting
    if (Input.GetKey(sprintKey) && sprintRemaining > 0f)
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




