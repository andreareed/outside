using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
  [SerializeField] GameObject blueprintTemplate;
  [SerializeField] Transform craftingContainer;
  [SerializeField] Transform detailsContainer;
  [SerializeField] CraftingBlueprintSO[] craftingBlueprints;

  private Blueprint[] blueprints;
  private InventoryManager inventoryManager;

  private void Start()
  {
    GenerateBlueprints();
    inventoryManager = GetComponent<InventoryManager>();
  }

  public void GenerateBlueprints()
  {
    List<Blueprint> blueprintList = new List<Blueprint>();
    for (int i = 0; i < craftingBlueprints.Length; i++)
    {
      Blueprint bp = Instantiate(blueprintTemplate.gameObject, craftingContainer).GetComponent<Blueprint>();
      bp.icon.sprite = craftingBlueprints[i].result.Icon;
      bp.result = craftingBlueprints[i].result;
      bp.requirements = craftingBlueprints[i].requirements;
      Button button = bp.GetComponent<Button>();
      button.onClick.AddListener(delegate { ShowCraftingInstructionsForItem(bp); });

      blueprintList.Add(bp);
    }
    blueprints = blueprintList.ToArray();

  }

  private void ShowCraftingInstructionsForItem(Blueprint blueprint)
  {

    detailsContainer.Find("ItemName").GetComponent<Text>().text = blueprint.result.ItemName;
    detailsContainer.Find("Image").GetComponent<Image>().sprite = blueprint.result.Icon;
    detailsContainer.Find("Description").GetComponent<Text>().text = blueprint.result.Description;

    Button craftButton = detailsContainer.Find("CraftButton").GetComponent<Button>();
    craftButton.onClick.AddListener(delegate { CraftItem(blueprint); });

    Text requirementsText = detailsContainer.Find("Requirements").GetComponent<Text>();
    requirementsText.text = "";

    bool canCraft = true;
    for (int i = 0; i < blueprint.requirements.Length; i++)
    {
      CraftingResource requirement = blueprint.requirements[i];
      int inInventory = inventoryManager.AllInventoryItems.ContainsKey(requirement.resource.ItemName)
        ? inventoryManager.AllInventoryItems[requirement.resource.ItemName]
        : 0;
      requirementsText.text += requirement.resource.ItemName + " (" + inInventory + "/" + requirement.amount + ")\n";

      if (inInventory < requirement.amount)
      {
        canCraft = false;
      }
    }

    craftButton.interactable = canCraft;
    detailsContainer.gameObject.SetActive(true);
  }

  private void CraftItem(Blueprint blueprint)
  {
    for (int i = 0; i < blueprint.requirements.Length; i++)
    {
      CraftingResource item = blueprint.requirements[i];
      inventoryManager.RemoveItem(item.resource, item.amount);
    }

    // Add item
    Interactable craftedItem = blueprint.gameObject.AddComponent<Interactable>();
    craftedItem.Item = blueprint.result;
    craftedItem.StackSize = blueprint.resultAmount;
    inventoryManager.AddItem(craftedItem);

    ShowCraftingInstructionsForItem(blueprint);
  }
}
