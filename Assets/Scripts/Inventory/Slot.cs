using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler, IDragHandler
{
  [Header("Slot Data")]
  private ItemSO item;
  private int stackSize;
  [Space]
  [Header("Required References")]
  [SerializeField] Image icon;
  [SerializeField] Text stackText;
  private bool isEmpty;
  private DragDropHandler dragDropHandler;
  private InventoryManager inventoryManager;

  // Public setters & getters
  public bool IsEmpty => isEmpty;
  public Image Icon => icon;
  public ItemSO Item { get => item; set => item = value; }
  public int StackSize { get => stackSize; set => stackSize = value; }

  private void Start()
  {
    dragDropHandler = GetComponentInParent<DragDropHandler>();
    inventoryManager = GetComponentInParent<InventoryManager>();
    UpdateSlot();
  }

  public void UpdateSlot()
  {
    if (item == null)
    {
      isEmpty = true;
      icon.gameObject.SetActive(false);
      stackText.gameObject.SetActive(false);
    }
    else
    {
      isEmpty = false;
      icon.gameObject.SetActive(true);
      stackText.gameObject.SetActive(true);
      icon.sprite = item.Icon;
    }
  }

  public void AddItemToSlot(ItemSO data_, int stackSize_)
  {
    item = data_;
    stackSize = stackSize_;
    stackText.text = $"{stackSize}";
    UpdateSlot();
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (dragDropHandler.IsDragging)
    {
      dragDropHandler.SlotDraggedTo = this;
    }
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    if (!dragDropHandler.IsDragging)
    {
      if (eventData.button == PointerEventData.InputButton.Left && !isEmpty)
      {
        dragDropHandler.SlotDraggedFrom = this;
        dragDropHandler.IsDragging = true;
      }
    }
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    if (dragDropHandler.IsDragging)
    {
      if (dragDropHandler.SlotDraggedTo == null)
      {
        dragDropHandler.SlotDraggedFrom.Drop();
        dragDropHandler.IsDragging = false;
      }
      else if (dragDropHandler.SlotDraggedTo != null)
      {
        bool isStackable = dragDropHandler.SlotDraggedTo.Item == dragDropHandler.SlotDraggedFrom.Item
          && dragDropHandler.SlotDraggedTo.Item.IsStackable;

        // Combine stacks
        if (isStackable)
        {
          int amountToAdd = Mathf.Min(
            dragDropHandler.SlotDraggedTo.Item.MaxStack - dragDropHandler.SlotDraggedTo.StackSize,
            dragDropHandler.SlotDraggedFrom.StackSize
          );

          dragDropHandler.SlotDraggedTo.AddItemToSlot(
            dragDropHandler.SlotDraggedTo.Item,
            amountToAdd + dragDropHandler.SlotDraggedTo.StackSize
          );

          if (amountToAdd == dragDropHandler.SlotDraggedFrom.StackSize)
          {
            dragDropHandler.SlotDraggedFrom.ClearSlot();
          }
          else
          {
            dragDropHandler.SlotDraggedFrom.AddItemToSlot(
              dragDropHandler.SlotDraggedFrom.Item,
              dragDropHandler.SlotDraggedFrom.StackSize - amountToAdd
            );
          }
        }
        else
        {
          // Swap items
          inventoryManager.SwapSlots(dragDropHandler.SlotDraggedFrom, dragDropHandler.SlotDraggedTo);
        }
        dragDropHandler.IsDragging = false;

      }
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (dragDropHandler.IsDragging)
    {
      dragDropHandler.SlotDraggedTo = null;
    }
  }

  public void Drop()
  {
    if (item != null)
    {
      inventoryManager.DropItem(this);
      ClearSlot();
    }
  }

  public void ClearSlot()
  {
    item = null;
    stackSize = 0;
    UpdateSlot();
  }

  public void OnDrag(PointerEventData eventData)
  {
    // Unused, but required to prevent onPointerUp from being called when dragging
  }
}