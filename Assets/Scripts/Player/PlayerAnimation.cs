using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
  private Animator animator;
  private PlayerControls playerControls;
  private float acceleration = 1f;
  private float decceleration = 2f;
  private int isJumpingHash;
  private int isFallingHash;

  void Start()
  {
    animator = GetComponent<Animator>();
    playerControls = GetComponentInParent<PlayerControls>();
    isJumpingHash = Animator.StringToHash("isJumping");
    isFallingHash = Animator.StringToHash("isFalling");
  }

  void FixedUpdate()
  {

    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    bool horizontalMovement = Mathf.Abs(horizontalInput) > .05f;
    bool verticalMovement = Mathf.Abs(verticalInput) > .05f;

    bool isSprinting = playerControls.IsSprinting;
    float maxVelocity = isSprinting ? 1f : .5f;

    // Movement
    float velocityX = animator.GetFloat("velocityX");
    float velocityY = animator.GetFloat("velocityY");
    float targetVelocityX = 0f;
    float targetVelocityY = 0f;

    if (horizontalMovement)
    {
      targetVelocityX = horizontalInput > 0 ? maxVelocity : -maxVelocity;
    }

    if (verticalMovement)
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
      if (targetVelocityY == 0f)
      {
        velocityY += Time.deltaTime * decceleration * (velocityY > 0 ? -1 : 1);
      }
      else
      {
        velocityY += Time.deltaTime * acceleration * (targetVelocityY > velocityY ? 1 : -1);
      }
    }

    // If we are stopping, cut to 0 velocity when under .05f to prevent stuttering
    velocityX = targetVelocityX == 0f && Mathf.Abs(velocityX) < .05f ? 0f : velocityX;
    velocityY = targetVelocityY == 0f && Mathf.Abs(velocityY) < .05f ? 0f : velocityY;

    animator.SetFloat("velocityX", velocityX);
    animator.SetFloat("velocityY", velocityY);


    // Jumping
    if (animator.GetBool(isJumpingHash) != playerControls.IsJumping)
    {
      animator.SetBool(isJumpingHash, playerControls.IsJumping);
    }
    if (animator.GetBool(isFallingHash) != playerControls.IsFalling)
    {
      animator.SetBool(isFallingHash, playerControls.IsFalling);
    }
  }
}