using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
  private Animator animator;
  private PlayerControls playerControls;
  private float acceleration = 1f;
  private float decceleration = 6f;
  private int isJumpingHash;
  private int isFallingHash;
  private int isLandingHash;
  private int isMovingBackwardsHash;
  private bool isStrafing;

  void Start()
  {
    animator = GetComponent<Animator>();
    playerControls = GetComponentInParent<PlayerControls>();
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

    bool isSprinting = playerControls.IsSprinting;
    float maxVelocity = isSprinting && !movingBackwards ? 1f : .5f;

    bool groundCheck = playerControls.IsGrounded;
    bool jumpInput = Input.GetButtonDown("Jump") && groundCheck;
    bool isJumping = animator.GetBool(isJumpingHash);
    bool isFalling = animator.GetBool(isFallingHash);
    bool isLanding = animator.GetBool(isLandingHash);

    // Movement
    float velocityX = animator.GetFloat("velocityX");
    float velocityY = animator.GetFloat("velocityY");
    float targetVelocityX = 0f;
    float targetVelocityY = 0f;

    if (horizontalInput != 0)
    {
      targetVelocityX = horizontalInput > 0 ? maxVelocity : -maxVelocity;
    }

    if (verticalInput != 0)
    {
      targetVelocityY = verticalInput > 0 ? maxVelocity : -maxVelocity;
    }

    if (velocityX != targetVelocityX)
    {
      if (targetVelocityX == 0)
      {
        velocityX += Time.deltaTime * decceleration * (velocityX > 0 ? -1 : 1);
      }
      else
      {
        velocityX += Time.deltaTime * acceleration * (targetVelocityX > velocityX ? 1 : -1);
      }
    }
    if (velocityY != targetVelocityY)
    {
      if (targetVelocityY == 0)
      {
        velocityY += Time.deltaTime * decceleration * (velocityY > 0 ? -1 : 1);
      }
      else
      {
        velocityY += Time.deltaTime * acceleration * (targetVelocityY > velocityY ? 1 : -1);
      }
    }

    animator.SetFloat("velocityX", velocityX);
    animator.SetFloat("velocityY", velocityY);

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
  }
}