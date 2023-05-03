using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
  [SerializeField] GameObject blueprintTemplate;
  [SerializeField] Transform craftingContainer;
  [SerializeField] CraftingBlueprintSO[] craftingBlueprints;

  public void GenerateBlueprints()
  {
    List<Blueprint> blueprintList = new List<Blueprint>();
    for (int i = 0; i < craftingBlueprints.Length; i++)
    {
      Blueprint bp = Instantiate(blueprintTemplate.gameObject, craftingContainer).GetComponent<Blueprint>();
      // blueprintTemplate.icon.sprite = blueprint.result.Icon;
      // blueprintTemplate.nameText.text = blueprint.blueprintName;
      // for (int j = 0; j < blueprint.requirements.Length; j++)
      // {
      //   CraftingRequirement requirement = blueprint.requirements[j];
      //   blueprintTemplate.requirementsText.text += requirement.item.ItemName + " x" + requirement.amountNeeded + "\n";
      // }
    }
  }

  //  List<Slot> inventorySlotList = new List<Slot>();

  // for (int i = 0; i < inventorySize; i++)
  // {
  //   Slot slot = Instantiate(slotTemplate.gameObject, slotContainer).GetComponent<Slot>();
  //   slot.UpdateSlot();
  //   inventorySlotList.Add(slot);
  // }

  // inventorySlots = inventorySlotList.ToArray();
}
