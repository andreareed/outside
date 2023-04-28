using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
  [Header("Input Settings")]
  [SerializeField] KeyCode inventoryKey = KeyCode.I;
  // Internal Variables
  private PlayerControls playerControls;
  private Transform inventoryScreen;
  private bool inventoryOpen = false;

  private void Start()
  {
    playerControls = GetComponentInParent<PlayerControls>();
    inventoryScreen = transform.Find("Inventory");
  }

  private void Update()
  {
    bool isOpen = inventoryOpen;
    bool cameraMovementEnabled = playerControls.cameraMovementEnabled;

    if (Input.GetKeyDown(inventoryKey))
    {
      inventoryOpen = !inventoryOpen;
      inventoryScreen.gameObject.SetActive(inventoryOpen);
    }

    if (isOpen)
    {
      Cursor.lockState = CursorLockMode.None;
      playerControls.SetCameraMovement(false);
    }
    else
    {
      Cursor.lockState = CursorLockMode.Locked;
      playerControls.SetCameraMovement(true);

    }
  }
}
