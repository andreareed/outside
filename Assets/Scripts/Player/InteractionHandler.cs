using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
  [SerializeField] float interactionRange = 2f;
  [SerializeField] KeyCode interactionKey = KeyCode.E;

  private void Update()
  {
    if (Input.GetKeyDown(interactionKey))
    {
      Interact();
    }
  }
  private void Interact()
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange))
    {
      Interactable item = hit.transform.GetComponent<Interactable>();
      if (item != null)
      {
        GetComponentInParent<InventoryManager>().AddItem(item);
        Destroy(hit.transform.gameObject);
      }

    }

  }
}
