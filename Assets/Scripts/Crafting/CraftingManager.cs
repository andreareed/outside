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

  private void Start()
  {
    GenerateBlueprints();
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
    Text requirementsText = detailsContainer.Find("Requirements").GetComponent<Text>();
    requirementsText.text = "";

    for (int i = 0; i < blueprint.requirements.Length; i++)
    {
      CraftingResource resource = blueprint.requirements[i];
      requirementsText.text += resource.unit + " x" + resource.amount + "\n";
    }

    detailsContainer.gameObject.SetActive(true);
  }
}
