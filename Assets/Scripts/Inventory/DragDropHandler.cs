using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragDropHandler : MonoBehaviour
{
  [SerializeField] Image dragDropIcon;
  private Slot slotDraggedFrom;
  private Slot slotDraggedTo;
  private bool isDragging;

  // Public getters and setters
  public bool IsDragging { get => isDragging; set => isDragging = value; }
  public Slot SlotDraggedFrom { get => slotDraggedFrom; set => slotDraggedFrom = value; }
  public Slot SlotDraggedTo { get => slotDraggedTo; set => slotDraggedTo = value; }


  private void Update()
  {
    if (isDragging && slotDraggedFrom != null)
    {
      dragDropIcon.gameObject.SetActive(true);
      dragDropIcon.sprite = slotDraggedFrom.Icon.sprite;
      dragDropIcon.transform.position = Input.mousePosition;
    }
    else
    {
      dragDropIcon.gameObject.SetActive(false);
    }
  }
}
