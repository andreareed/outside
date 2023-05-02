using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] int inventorySize = 56;

  [Header("Required References")]
  [SerializeField] GameObject slotTemplate;
  [SerializeField] Transform slotContainer;

  private Slot[] inventorySlots;
  [SerializeField] Slot[] allSlots;


  private void Start()
  {
    GenerateSlots();
  }

  private void GenerateSlots()
  {
    List<Slot> slotList = new List<Slot>();
    List<Slot> inventorySlotList = new List<Slot>();

    for (int i = 0; i < allSlots.Length; i++)
    {
      slotList.Add(allSlots[i]);
    }

    for (int i = 0; i < inventorySize; i++)
    {
      Slot slot = Instantiate(slotTemplate.gameObject, slotContainer).GetComponent<Slot>();
      inventorySlotList.Add(slot);
      slot.UpdateSlot();
      slotList.Add(slot);
    }

    inventorySlots = inventorySlotList.ToArray();
    allSlots = slotList.ToArray();
  }

  public void AddItem(Interactable item)
  {
    for (int i = 0; i < inventorySlots.Length; i++)
    {
      if (inventorySlots[i].IsEmpty)
      {
        inventorySlots[i].AddItemToSlot(item.Item, 1);
        return;
      }
      else if (inventorySlots[i].Item == item.Item && inventorySlots[i].StackSize < item.Item.MaxStack)
      {
        inventorySlots[i].AddItemToSlot(item.Item, inventorySlots[i].StackSize);
        return;
      }
    }

  }
}