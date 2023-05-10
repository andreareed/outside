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

  void Start()
  {
    animator = GetComponent<Animator>();
    playerControls = GetComponentInParent<PlayerControls>();
    velocityHash = Animator.StringToHash("velocity");
    isJumpingHash = Animator.StringToHash("isJumping");
    isFallingHash = Animator.StringToHash("isFalling");
    isLandingHash = Animator.StringToHash("isLanding");
  }

  void Update()
  {
    bool movementInput = Input.GetAxis("Vertical") != 0
      || Input.GetAxis("Horizontal") != 0;
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
    animator.SetFloat(velocityHash, velocity);
  }
}