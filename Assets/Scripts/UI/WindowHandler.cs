using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandler : MonoBehaviour
{
  [Header("Input Settings")]
  [SerializeField] KeyCode inventoryKey = KeyCode.I;
  [SerializeField] KeyCode craftingKey = KeyCode.C;
  // Internal Variables
  private PlayerControls playerControls;
  private Transform inventoryScreen;
  private Transform craftingScreen;
  private bool inventoryOpen = false;
  private bool craftingOpen = false;

  private void Start()
  {
    playerControls = GetComponentInParent<PlayerControls>();
    inventoryScreen = transform.Find("Inventory");
    craftingScreen = transform.Find("Crafting");
  }

  private void Update()
  {
    bool isOpen = inventoryOpen || craftingOpen;
    bool cameraMovementEnabled = playerControls.cameraMovementEnabled;

    if (Input.GetKeyDown(inventoryKey))
    {
      inventoryOpen = !inventoryOpen;
      if (inventoryOpen && craftingOpen)
      {
        craftingOpen = false;
        craftingScreen.gameObject.SetActive(false);
      }
      inventoryScreen.gameObject.SetActive(inventoryOpen);
    }
    if (Input.GetKeyDown(craftingKey))
    {
      craftingOpen = !craftingOpen;
      if (craftingOpen && inventoryOpen)
      {
        inventoryOpen = false;
        inventoryScreen.gameObject.SetActive(false);
      }
      craftingScreen.gameObject.SetActive(craftingOpen);
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
