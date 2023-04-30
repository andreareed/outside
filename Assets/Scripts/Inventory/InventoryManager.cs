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
  private Slot[] allSlots;


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
    }

    inventorySlots = inventorySlotList.ToArray();
    allSlots = slotList.ToArray();
  }
}
