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
  [SerializeField] GameObject rightHand;
  [SerializeField] GameObject leftHand;

  private Slot[] inventorySlots;
  private Slot[] hotbarSlots;
  private Dictionary<string, int> allInventoryItems;

  // Public getters
  public Dictionary<string, int> AllInventoryItems => allInventoryItems;
  public GameObject RightHand => rightHand;
  public GameObject LeftHand => leftHand;


  private void Start()
  {
    GenerateHotBarSlots();
    GenerateInventorySlots();
  }

  private void Update()
  {
    HotbarListener();
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
    UpdateInventoryList();
  }

  private void GenerateHotBarSlots()
  {
    List<Slot> hotbarSlotsList = new List<Slot>();

    for (int i = 0; i < hotbarSize; i++)
    {
      Slot slot = Instantiate(slotTemplate.gameObject, hotbarContainer).GetComponent<Slot>();
      slot.UpdateSlot();
      slot.HotbarNumber = i + 1;
      hotbarSlotsList.Add(slot);
    }

    hotbarSlots = hotbarSlotsList.ToArray();
  }

  private void HotbarListener()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
    {
      hotbarSlots[0].UseItem();
    }
    if (Input.GetKeyDown(KeyCode.Alpha2))
    {
      hotbarSlots[1].UseItem();
    }
    if (Input.GetKeyDown(KeyCode.Alpha3))
    {
      hotbarSlots[2].UseItem();
    }
    if (Input.GetKeyDown(KeyCode.Alpha4))
    {
      hotbarSlots[3].UseItem();
    }
    if (Input.GetKeyDown(KeyCode.Alpha5))
    {
      hotbarSlots[4].UseItem();
    }
    if (Input.GetKeyDown(KeyCode.Alpha6))
    {
      hotbarSlots[5].UseItem();
    }
    if (Input.GetKeyDown(KeyCode.Alpha7))
    {
      hotbarSlots[6].UseItem();
    }
    if (Input.GetKeyDown(KeyCode.Alpha8))
    {
      hotbarSlots[7].UseItem();
    }
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

    UpdateInventoryList();
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
      AddItemToNewSlot(item, quantity);
    }
  }

  private void AddItemToNewSlot(Interactable item, int quantity)
  {
    Slot slot = Instantiate(slotTemplate.gameObject, slotContainer).GetComponent<Slot>();
    inventorySlots = new Slot[inventorySlots.Length + 1];
    inventorySlots[inventorySlots.Length - 1] = slot;
    AddToEmptySlot(item, quantity);
  }

  public void DropItem(Slot slot)
  {
    if (slot.IsEmpty)
    {
      return;
    }

    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z += 1.0f; // 1m away from the camera position
    Vector3 dropPosition = Camera.main.ScreenToWorldPoint(mousePosition);

    Interactable droppedItem = Instantiate(dropModel, dropPosition, Quaternion.Normalize(dropModel.transform.rotation)).AddComponent<Interactable>();
    droppedItem.transform.SetParent(null);

    droppedItem.Item = slot.Item;
    droppedItem.StackSize = slot.StackSize;

    slot.ClearSlot();
    UpdateInventoryList();
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

  public void UpdateInventoryList()
  {
    allInventoryItems = new Dictionary<string, int>();
    for (int i = 0; i < inventorySize; i++)
    {
      Slot slot = inventorySlots[i];
      if (!slot.IsEmpty)
      {
        string item = slot.Item.ItemName;
        if (allInventoryItems.ContainsKey(item))
        {
          allInventoryItems[item] += slot.StackSize;
        }
        else
        {
          allInventoryItems.Add(item, slot.StackSize);
        }
      }
    }
  }

  public void RemoveItem(ItemSO item, int amount)
  {
    for (int i = inventorySlots.Length - 1; i >= 0 && amount > 0; i--)
    {
      Slot slot = inventorySlots[i];
      if (!slot.IsEmpty && slot.Item == item)
      {
        if (slot.StackSize > amount)
        {
          slot.StackSize -= amount;
          slot.UpdateSlot();
          break;
        }
        else
        {
          amount -= slot.StackSize;
          slot.ClearSlot();
        }
      }
    }
    UpdateInventoryList();
  }
}