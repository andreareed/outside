using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
  private Animator animator;
  private PlayerControls playerControls;
  private float velocity = 0f;
  private float acceleration = 1f;
  private float decceleration = 6f;
  private int velocityHash;
  private int isJumpingHash;
  private int isFallingHash;
  private int isLandingHash;
  private int isMovingBackwardsHash;
  private bool isStrafing;

  void Start()
  {
    animator = GetComponent<Animator>();
    playerControls = GetComponentInParent<PlayerControls>();
    velocityHash = Animator.StringToHash("velocity");
    isJumpingHash = Animator.StringToHash("isJumping");
    isFallingHash = Animator.StringToHash("isFalling");
    isLandingHash = Animator.StringToHash("isLanding");
    isMovingBackwardsHash = Animator.StringToHash("isMovingBackwards");
  }

  void Update()
  {

    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");

    bool isStrafing = horizontalInput != 0;
    bool movingBackwards = verticalInput < 0;

    bool movementInput = horizontalInput != 0
      || verticalInput != 0;
    // bool isBackwards = verticalInput < 0
    Debug.Log(horizontalInput + " " + verticalInput);
    bool isSprinting = playerControls.IsSprinting;
    float maxVelocity = isSprinting ? 1f : .2f;

    bool groundCheck = playerControls.IsGrounded;
    bool jumpInput = Input.GetButtonDown("Jump") && groundCheck;
    bool isJumping = animator.GetBool(isJumpingHash);
    bool isFalling = animator.GetBool(isFallingHash);
    bool isLanding = animator.GetBool(isLandingHash);

    // Jumping
    if (jumpInput && !isJumping && !isFalling)
    {
      animator.SetBool("isJumping", true);
      animator.SetBool("isFalling", true);
    }
    else if (isJumping)
    {
      animator.SetBool("isJumping", false);
      animator.SetBool("isFalling", true);
    }
    else if (isFalling && groundCheck)
    {
      animator.SetBool("isFalling", false);
      animator.SetBool("isLanding", true);
    }

    // Movement
    if (movementInput && velocity < 1f && velocity <= maxVelocity)
    {
      velocity += Time.deltaTime * acceleration;
    }
    else if (velocity > maxVelocity)
    {
      velocity -= Time.deltaTime * 1f;
    }
    else if (!movementInput && velocity > 0f)
    {
      velocity -= Time.deltaTime * decceleration;
    }

    if (animator.GetFloat(velocityHash) != velocity)
    {
      animator.SetFloat(velocityHash, movingBackwards ? 0 : Mathf.Clamp(velocity, 0, 1));
    }

    if (animator.GetBool(isMovingBackwardsHash) != movingBackwards)
    {
      animator.SetBool(isMovingBackwardsHash, movingBackwards);
    }

    animator.SetFloat(velocityHash, velocity);
  }



}