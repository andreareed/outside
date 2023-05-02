using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] int inventorySize = 42;
  [SerializeField] int slotCap = 100;
  [SerializeField] int hotbarSize = 8;

  [Header("Required References")]
  [SerializeField] GameObject slotTemplate;
  [SerializeField] Transform slotContainer;
  [SerializeField] Transform hotbarContainer;
  [SerializeField] GameObject dropModel;

  private Slot[] inventorySlots;
  private Slot[] hotbarSlots;


  private void Start()
  {
    GenerateHotBarSlots();
    GenerateInventorySlots();
  }

  private void GenerateInventorySlots()
  {
    List<Slot> inventorySlotList = new List<Slot>();

    for (int i = 0; i < inventorySize; i++)
    {
      Slot slot = Instantiate(slotTemplate.gameObject, slotContainer).GetComponent<Slot>();
      slot.UpdateSlot();
      inventorySlotList.Add(slot);
    }

    inventorySlots = inventorySlotList.ToArray();
  }

  private void GenerateHotBarSlots()
  {
    List<Slot> hotbarSlotsList = new List<Slot>();

    for (int i = 0; i < hotbarSize; i++)
    {
      Slot slot = Instantiate(slotTemplate.gameObject, hotbarContainer).GetComponent<Slot>();
      slot.UpdateSlot();
      hotbarSlotsList.Add(slot);
    }

    hotbarSlots = hotbarSlotsList.ToArray();
  }

  public void AddItem(Interactable item)
  {
    int quantityRemaining = item.StackSize;
    for (int i = 0; i < inventorySlots.Length; i++)
    {
      if (
        !inventorySlots[i].IsEmpty &&
        inventorySlots[i].Item.ItemName == item.Item.ItemName &&
        inventorySlots[i].Item.IsStackable &&
        inventorySlots[i].StackSize < item.Item.MaxStack
        )
      {
        int amountToAdd = Mathf.Min(
          item.Item.MaxStack - inventorySlots[i].StackSize,
          item.Item.IsStackable ? quantityRemaining : 1
        );

        inventorySlots[i].AddItemToSlot(item.Item, amountToAdd + inventorySlots[i].StackSize);
        quantityRemaining -= amountToAdd;

        if (quantityRemaining == 0)
        {
          return;
        }
      }
    }

    if (quantityRemaining > 0)
    {
      AddToEmptySlot(item, quantityRemaining);
    }

  }

  private void AddToEmptySlot(Interactable item, int quantity)
  {
    Slot emptySlot = null;
    for (int i = 0; i < inventorySlots.Length; i++)
    {
      if (inventorySlots[i].IsEmpty)
      {
        emptySlot = inventorySlots[i];
        break;
      }
    }

    if (emptySlot != null)
    {
      // int amountToAdd = Mathf.Min(item.Item.MaxStack - emptySlot.StackSize, quantity);

      int amountToAdd = Mathf.Min(
          item.Item.MaxStack - emptySlot.StackSize,
          item.Item.IsStackable ? quantity : 1
        );

      emptySlot.AddItemToSlot(item.Item, amountToAdd);

      if (amountToAdd == quantity)
      {
        return;
      }
      else
      {
        AddToEmptySlot(item, quantity - amountToAdd);
      }
    }
    else if (inventorySlots.Length < slotCap)
    {
      AddItemToNewSlot(item);
    }
  }

  private void AddItemToNewSlot(Interactable item)
  {
    Slot slot = Instantiate(slotTemplate.gameObject, slotContainer).GetComponent<Slot>();
    slot.AddItemToSlot(item.Item, 1);
    inventorySlots = new Slot[inventorySlots.Length + 1];
    inventorySlots[inventorySlots.Length - 1] = slot;
  }

  public void DropItem(Slot slot)
  {
    if (slot.IsEmpty)
    {
      return;
    }

    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z = 2.0f; // 2m away from the camera position
    Vector3 dropPosition = Camera.main.ScreenToWorldPoint(mousePosition);

    Interactable droppedItem = Instantiate(dropModel, dropPosition, Quaternion.Normalize(dropModel.transform.rotation)).AddComponent<Interactable>();
    droppedItem.transform.SetParent(null);

    droppedItem.Item = slot.Item;
    droppedItem.StackSize = slot.StackSize;

    slot.ClearSlot();
  }

  public void SwapSlots(Slot slotFrom, Slot slotTo)
  {
    if (slotFrom.Item != slotTo.Item)
    {
      ItemSO item = slotTo.Item;
      int stackSize = slotTo.StackSize;

      slotTo.AddItemToSlot(slotFrom.Item, slotFrom.StackSize);
      slotFrom.AddItemToSlot(item, stackSize);
    }
  }
}