using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] int inventorySize = 56;
  [SerializeField] int slotCap = 100;

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
      slotList.Add(slot);
      slot.UpdateSlot();
    }

    inventorySlots = inventorySlotList.ToArray();
    allSlots = slotList.ToArray();
  }

  public void AddItem(Interactable item)
  {
    int quantityRemaining = item.StackSize;
    for (int i = 0; i < inventorySlots.Length; i++)
    {
      if (
        !inventorySlots[i].IsEmpty &&
        inventorySlots[i].Item.ItemName == item.Item.ItemName &&
        inventorySlots[i].StackSize < item.Item.MaxStack
        )
      {
        int amountToAdd = Mathf.Min(item.Item.MaxStack - inventorySlots[i].StackSize, quantityRemaining);

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
      int amountToAdd = Mathf.Min(item.Item.MaxStack - emptySlot.StackSize, quantity);


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
    allSlots = new Slot[allSlots.Length + 1];
    allSlots[allSlots.Length - 1] = slot;
  }
}