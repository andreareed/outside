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

  // Camera Internal Variables
  private float yaw = 0.0f;
  private float pitch = 0.0f;

  private void Awake()
  {
    rb = GetComponent<Rigidbody>();

    playerCamera.fieldOfView = fov;
  }

  private void Update()
  {
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
  }
}

