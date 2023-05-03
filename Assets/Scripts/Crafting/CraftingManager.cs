using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
  public BlueprintTemplate blueprints;
  public CraftingBlueprintSO[] craftingBlueprints;
  public Transform craftingContainer;

  public void GenerateRecipes()
  {
    for (int i = 0; i < craftingBlueprints.Length; i++)
    {
      CraftingBlueprintSO blueprint = craftingBlueprints[i];
      BlueprintTemplate recipeTemplate = Instantiate(blueprints, craftingContainer);
      recipeTemplate.icon.sprite = blueprint.result.Icon;
      recipeTemplate.nameText.text = blueprint.blueprintName;
      recipeTemplate.requirementsText.text = "";
      for (int j = 0; j < blueprint.requirements.Length; j++)
      {
        CraftingRequirement requirement = blueprint.requirements[j];
        recipeTemplate.requirementsText.text += requirement.item.ItemName + " x" + requirement.amountNeeded + "\n";
      }
    }
  }
}
